using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
  [Header("组件")]
  public AudioSource BGMSource;
  public AudioSource FXSource;

  [Header("混音器")]
  public AudioMixer mixer;

  [Header("事件")]
  public PlayAudioEventSO BGMAudioEvent;
  public PlayAudioEventSO FXAudioEvent;
  public FloatEventSO volumeChangeEvent;

  private void OnEnable() {
    BGMAudioEvent.onEventRaised += PlayBGM;
    FXAudioEvent.onEventRaised += PlayFX;
    volumeChangeEvent.onEventRaised += VolumeChange;
  }

  private void OnDisable() {
    BGMAudioEvent.onEventRaised -= PlayBGM;
    FXAudioEvent.onEventRaised -= PlayFX;
    volumeChangeEvent.onEventRaised -= VolumeChange;
  }

  private void PlayBGM(AudioClip clip) {
    BGMSource.clip = clip;
    BGMSource.Play();
  }

  private void PlayFX(AudioClip clip) {
    FXSource.clip = clip;
    FXSource.Play();
  }

  private void VolumeChange(float amount) {
    mixer.SetFloat("MasterVolume", amount * 100 - 80);
  }
}
