using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class Sign : MonoBehaviour {
  private Animator animator;
  public SpriteRenderer signSprite;
  public GameObject player;
  private bool canPress;
  public IInteractable targetItem;

  private void Awake() {
    animator = GetComponentInChildren<Animator>();
  }

  private void OnEnable() {
    InputSystem.onActionChange += SwitchSign;
  }

  private void OnDisable() {
    InputSystem.onActionChange -= SwitchSign;
  }

  private void Update() {
    signSprite.enabled = canPress;
    transform.localScale = player.transform.localScale;
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Interactable")) {
      canPress = true;
      targetItem = other.GetComponent<IInteractable>();
    }
  }

  private void OnTriggerExit2D(Collider2D other) {
    canPress = false;
    targetItem = null;
  }

  private void SwitchSign(object arg1, InputActionChange change) {
    if (change == InputActionChange.ActionStarted) {
      switch (((InputAction)arg1).activeControl.device) {
        case Keyboard:
          animator.Play("Keyboard");
          break;
        case Gamepad:
          animator.Play("XBox");
          break;
      }
    }
  }
}
