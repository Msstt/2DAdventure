using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject {
  public UnityAction<GameSceneSO, Vector3, bool> onEventRaised;

  public void RaiseEvent(GameSceneSO scene, Vector3 position, bool faceScreen) {
    onEventRaised?.Invoke(scene, position, faceScreen);
  }
}
