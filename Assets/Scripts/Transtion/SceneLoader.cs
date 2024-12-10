using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, ISaveable {
  [Header("玩家坐标")]
  public Transform playerTransform;
  public Vector3 playerInitPosition;
  public Vector3 menuPosition;

  [Header("事件监听")]
  public SceneLoadEventSO loadSceneRequest;
  public VoidEventSO newGameEvent;
  public VoidEventSO exitEvent;

  [Header("广播")]
  public SceneLoadEventSO SceneUnloadEvent;
  public VoidEventSO afterSceneLoad;
  public FadeEventSO fadeEvent;

  [Header("场景")]
  public GameSceneSO firstScene;
  public GameSceneSO menuScene;
  private GameSceneSO currentScene;

  [Header("淡入淡出")]
  public float faceScreenDuartion;

  private GameSceneSO newScene;
  private Vector3 positionToGo;
  private bool faceScreen;
  private bool isLoading;

  private void Start() {
    loadSceneRequest.RaiseEvent(menuScene, menuPosition, true);
  }

  private void OnEnable() {
    loadSceneRequest.onEventRaised += LoadNewScene;
    newGameEvent.onEventRaised += NewGame;
    exitEvent.onEventRaised += OnExit;
    ISaveable saveable = this;
    saveable.RegisterSaveData();
  }

  private void OnDisable() {
    loadSceneRequest.onEventRaised -= LoadNewScene;
    newGameEvent.onEventRaised -= NewGame;
    exitEvent.onEventRaised -= OnExit;
    ISaveable saveable = this;
    saveable.UnRegisterSaveData();
  }

  private void NewGame() {
    loadSceneRequest.RaiseEvent(firstScene, playerInitPosition, true);
  }

  private void LoadNewScene(GameSceneSO arg0, Vector3 arg1, bool arg2) {
    if (isLoading) {
      return;
    }
    isLoading = true;
    newScene = arg0;
    positionToGo = arg1;
    faceScreen = arg2;
    StartCoroutine(UnloadScene());
  }

  private IEnumerator UnloadScene() {
    yield return new WaitForEndOfFrame();

    playerTransform.gameObject.SetActive(false);
    if (faceScreen) {
      fadeEvent.RaiseEvent(false, faceScreenDuartion);
      yield return new WaitForSeconds(faceScreenDuartion);
    }
    if (currentScene != null) {
      currentScene.scene.UnLoadScene();
    }
    SceneUnloadEvent.RaiseEvent(newScene, positionToGo, faceScreen);

    LoadScene();
  }

  private void LoadScene() {
    currentScene = newScene;
    var loading = currentScene.scene.LoadSceneAsync(LoadSceneMode.Additive);
    loading.Completed += OnLoadCompleted;
  }

  private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle) {
    playerTransform.position = positionToGo;
    playerTransform.gameObject.SetActive(true);

    afterSceneLoad.RaiseEvent();
    fadeEvent.RaiseEvent(true, faceScreenDuartion);

    isLoading = false;
  }

  public DataDefination GetDataId() {
    return GetComponent<DataDefination>();
  }

  public void SaveData(Data data) {
    data.SaveScene(currentScene);
  }

  public void LoadData(Data data) {
    var playerID = playerTransform.GetComponent<DataDefination>().ID;
    if (data.hadSave && data.charactorPosDict.ContainsKey(playerID)) {
      newScene = data.GetScene();
      positionToGo = data.charactorPosDict[playerID];
      loadSceneRequest.RaiseEvent(newScene, positionToGo, true);
    }
  }

  private void OnExit() {
    loadSceneRequest.RaiseEvent(menuScene, menuPosition, true);
  }
}
