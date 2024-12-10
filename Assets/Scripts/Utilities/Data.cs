using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data {
  public Dictionary<string, Vector3> charactorPosDict = new();
  public Dictionary<string, int> charactorHealthDict = new();
  private string scene;
  public bool hadSave { private set; get; } = false;

  public void SaveScene(GameSceneSO sceneSO) {
    scene = JsonUtility.ToJson(sceneSO);
    hadSave = true;
  }

  public GameSceneSO GetScene() {
    var sceneSO = ScriptableObject.CreateInstance<GameSceneSO>();
    JsonUtility.FromJsonOverwrite(scene, sceneSO);
    return sceneSO;
  }
}
