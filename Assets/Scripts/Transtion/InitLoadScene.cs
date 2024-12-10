using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class InitLoadScene : MonoBehaviour {
  public AssetReference PersistentScene;

  private void Awake() {
    PersistentScene.LoadSceneAsync();
  }
}
