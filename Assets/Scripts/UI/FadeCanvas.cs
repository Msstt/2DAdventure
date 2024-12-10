using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FadeCanvasFade : MonoBehaviour {
  public Image image;

  [Header("事件监听")]
  public FadeEventSO fadeEvent;

  private void OnEnable() {
    fadeEvent.onEventRaised += OnFadeEvent;
  }

  private void OnDisable() {
    fadeEvent.onEventRaised -= OnFadeEvent;
  }

  private void OnFadeEvent(bool InOrOut, float duration) {
    if (InOrOut) {
      image.DOBlendableColor(UnityEngine.Color.clear, duration);
    } else {
      image.DOBlendableColor(UnityEngine.Color.black, duration);
    }
  }

  public void On() {
    Debug.Log("??");
  }
}
