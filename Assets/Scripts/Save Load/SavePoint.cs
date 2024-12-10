using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable {
  public SpriteRenderer sign;
  public Sprite closeSprite;
  public Sprite openSprite;

  [Header("广播")]
  public VoidEventSO loadGameEvent;

  [Header("灯光")]
  public GameObject signLight;

  private bool isDone = false;

  private void OnEnable() {
    sign.sprite = isDone ? openSprite : closeSprite;
    signLight.SetActive(isDone);
  }

  public void TriggerAction() {
    SaveData();
  }

  private void SaveData() {
    isDone = true;
    sign.sprite = openSprite;
    signLight.SetActive(true);
    gameObject.tag = "Untagged";
    loadGameEvent.RaiseEvent();
  }
}
