using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/FadeEventSO")]
public class FadeEventSO : ScriptableObject {
  public UnityAction<bool, float> onEventRaised;

  public void RaiseEvent(bool InOrOut, float duration) {
    onEventRaised?.Invoke(InOrOut, duration);
  }
}
