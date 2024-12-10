using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInteractable {
  public GameSceneSO sceneToGo;
  public Vector3 positionToGo;
  public SceneLoadEventSO loadSceneRequest;

  public void TriggerAction() {
    loadSceneRequest.RaiseEvent(sceneToGo, positionToGo, true);
  }
}
