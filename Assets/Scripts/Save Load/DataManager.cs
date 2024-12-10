using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
public class DataManager : MonoBehaviour {
  static public DataManager instance;
  private List<ISaveable> saveables = new();
  private Data data;

  [Header("事件监听")]
  public VoidEventSO saveDataEvent;
  public VoidEventSO loadDataEvent;

  private void Awake() {
    if (instance == null) {
      instance = this;
    } else {
      Destroy(gameObject);
      return;
    }
    data = new();
  }

  private void OnEnable() {
    saveDataEvent.onEventRaised += Save;
    loadDataEvent.onEventRaised += Load;
  }

  private void OnDisable() {
    saveDataEvent.onEventRaised -= Save;
    loadDataEvent.onEventRaised -= Load;
  }

  public void RegisterSaveData(ISaveable saveable) {
    if (!saveables.Contains(saveable)) {
      saveables.Add(saveable);
    }
  }

  public void UnRegisterSaveData(ISaveable saveable) {
    saveables.Remove(saveable);
  }

  private void Save() {
    foreach (var saveable in saveables) {
      saveable.SaveData(data);
    }

    Debug.Log(data.charactorPosDict.Count);
    foreach (var item in data.charactorPosDict) {
      Debug.Log(item.Key + "     " + item.Value);
    }
  }

  private void Load() {
    foreach (var saveable in saveables) {
      saveable.LoadData(data);
    }
  }
}
