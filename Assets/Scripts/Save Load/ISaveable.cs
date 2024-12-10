using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable {
  DataDefination GetDataId();

  void RegisterSaveData() {
    DataManager.instance.RegisterSaveData(this);
  }
  void UnRegisterSaveData() {
    DataManager.instance.UnRegisterSaveData(this);
  }

  void SaveData(Data data);
  void LoadData(Data data);
}
