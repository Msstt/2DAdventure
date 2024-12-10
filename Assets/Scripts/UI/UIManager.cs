using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {

  [Header("玩家血条")]
  public CharactorEventSO onHealthChange;
  public GameObject playerStateBar;

  [Header("结束面板")]
  public GameObject gameOverPanel;
  public GameObject restartButton;

  [Header("事件监听")]
  public SceneLoadEventSO sceneUnloadEvent;
  public VoidEventSO gameOverEvent;
  public VoidEventSO loadDataEvent;
  public VoidEventSO exitEvent;

  [Header("移动端按键")]
  public GameObject mobileButton;

  [Header("暂停面板")]
  public GameObject pausePanel;
  public Button pauseButton;
  public Button continueButton;

  private void Awake() {
    pauseButton.onClick.AddListener(OpenPausePanel);
    continueButton.onClick.AddListener(ClosePausePanel);
  }

  private void OnEnable() {
    sceneUnloadEvent.onEventRaised += OnSceneLoading;
    onHealthChange.onEventRaised += ChangeHealth;
    gameOverEvent.onEventRaised += OnGameOver;
    loadDataEvent.onEventRaised += Restart;
    exitEvent.onEventRaised += Restart;
  }

  private void OnDisable() {
    sceneUnloadEvent.onEventRaised -= OnSceneLoading;
    onHealthChange.onEventRaised -= ChangeHealth;
    gameOverEvent.onEventRaised -= OnGameOver;
    loadDataEvent.onEventRaised -= Restart;
    exitEvent.onEventRaised -= Restart;
  }

  private void ChangeHealth(Charactor player) {
    playerStateBar.GetComponent<PlayerStateBar>().ChangeHealth((float)player.currentHealth / player.maxHealth);
  }

  private void OnSceneLoading(GameSceneSO scene, Vector3 arg1, bool arg2) {
    playerStateBar.SetActive(scene.type == SceneType.Location);
    if (scene.type == SceneType.Location) {
      mobileButton.SetActive(true);
#if UNITY_STANDALONE
    mobileButton.SetActive(false);
#endif
    }
  }

  private void OnGameOver() {
    gameOverPanel.SetActive(true);
    EventSystem.current.SetSelectedGameObject(restartButton);
  }

  private void Restart() {
    gameOverPanel.SetActive(false);
  }

  private void OpenPausePanel() {
    pausePanel.SetActive(true);
  }

  private void ClosePausePanel() {
    pausePanel.SetActive(false);
  }
}
