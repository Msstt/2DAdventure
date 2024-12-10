using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateBar : MonoBehaviour {
  public Image health;
  public Image healthDelta;

  public float deltaScale;

  private void Update() {
    if (healthDelta.fillAmount > health.fillAmount) {
      healthDelta.fillAmount -= Time.deltaTime * deltaScale;
    } else {
      healthDelta.fillAmount = health.fillAmount;
    }
  }

  public void ChangeHealth(float percent) {
    health.fillAmount = percent;
  }
}
