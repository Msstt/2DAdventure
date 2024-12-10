using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable {
  public Sprite openSprite;
  public Sprite closeSprite;
  private SpriteRenderer spriteRenderer;
  private bool isDone;

  private void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  private void OnEnable() {
    spriteRenderer.sprite = isDone ? openSprite : closeSprite;
  }

  public void TriggerAction() {
    if (!isDone) {
      OpenChest();
    }
  }

  public void OpenChest() {
    isDone = true;
    spriteRenderer.sprite = openSprite;
    gameObject.tag = "Untagged";
    GetComponent<AudioDefination>()?.PlayCilp();
  }
}
