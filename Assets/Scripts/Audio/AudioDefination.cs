using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDefination : MonoBehaviour {
  public AudioClip clip;
  public PlayAudioEventSO playAudioEvent;
  public bool enablePlay;

  private void OnEnable() {
    if (enablePlay) {
      PlayCilp();
    }
  }

  public void PlayCilp() {
    playAudioEvent.RaiseEvent(clip);
  }
}
