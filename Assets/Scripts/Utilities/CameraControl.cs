using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraControl : MonoBehaviour {
  private CinemachineConfiner2D cinemachineConfiner2D;
  [Header("事件监听")]
  public VoidEventSO afterSceneLoad;

  [Header("相机震动")]
  public CinemachineImpulseSource cinemachineImpulseSource;
  public VoidEventSO onCameraShake;

  private void Awake() {
    cinemachineConfiner2D = GetComponent<CinemachineConfiner2D>();
  }

  private void OnEnable() {
    onCameraShake.onEventRaised += OnCameraShake;
    afterSceneLoad.onEventRaised += OnSceneLoad;
  }

  private void OnDisable() {
    onCameraShake.onEventRaised -= OnCameraShake;
    afterSceneLoad.onEventRaised -= OnSceneLoad;
  }

  private void GetBounds() {
    var bounds = GameObject.FindGameObjectWithTag("Bounds");
    if (bounds == null) {
      return;
    }
    cinemachineConfiner2D.m_BoundingShape2D = bounds.GetComponent<Collider2D>();
    cinemachineConfiner2D.InvalidateCache();
  }

  private void OnCameraShake() {
    cinemachineImpulseSource.GenerateImpulse();
  }

  private void OnSceneLoad() {
    GetBounds();
  }
}
