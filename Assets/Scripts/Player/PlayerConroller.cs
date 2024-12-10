using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class PlayerConroller : MonoBehaviour {
  private PlayerInputControl inputControl;
  private Rigidbody2D rigidbody2D_;
  private Collider2D collider2D_;
  private PhysicsCheck physicsCheck;
  private PlayerAnimator playerAnimator;
  private Vector2 inputDir;
  private Sign confirmSign;
  private bool canControl = false;

  [Header("基础属性")]
  public float speed;
  public float jumpForce;

  [Header("受伤")]
  public float hurtForce;

  [Header("物理材质")]
  public PhysicsMaterial2D normal;
  public PhysicsMaterial2D wall;

  [Header("事件监听")]
  public SceneLoadEventSO sceneLoading;
  public VoidEventSO loadDataEvent;
  public VoidEventSO exitEvent;

  [Header("广播")]
  public VoidEventSO gameOverEvent;

  [Header("状态")]
  public bool isHurt;
  public bool isDead;
  public bool isAttack;

  private void Awake() {
    rigidbody2D_ = GetComponent<Rigidbody2D>();
    collider2D_ = GetComponent<Collider2D>();
    physicsCheck = GetComponent<PhysicsCheck>();
    playerAnimator = GetComponent<PlayerAnimator>();
    confirmSign = GetComponentInChildren<Sign>();

    inputControl = new PlayerInputControl();
    inputControl.Gameplay.Jump.started += Jump;
    inputControl.Gameplay.Attack.started += Attack;
    inputControl.Gameplay.Confirm.started += Confirm;
  }

  private void OnEnable() {
    inputControl.Enable();
    if (!canControl) {
      inputControl.Gameplay.Disable();
    }
    sceneLoading.onEventRaised += OnSceneLoading;
    loadDataEvent.onEventRaised += Restart;
    exitEvent.onEventRaised += Restart;
  }

  private void OnDisable() {
    inputControl.Disable();
    sceneLoading.onEventRaised -= OnSceneLoading;
    loadDataEvent.onEventRaised -= Restart;
    exitEvent.onEventRaised -= Restart;
  }

  private void Update() {
    inputDir = inputControl.Gameplay.Move.ReadValue<Vector2>();

    CheckState();
  }

  private void FixedUpdate() {
    if (!isHurt && !isAttack) {
      Move();
    }
  }

  private void Move() {
    if (!physicsCheck.isGround) {
      return;
    }
    rigidbody2D_.velocity = new Vector2(inputDir.x * speed * Time.fixedDeltaTime, rigidbody2D_.velocity.y);
    Vector3 scale = transform.localScale;
    if (inputDir.x > 0) {
      scale.x = Math.Abs(scale.x);
    }
    if (inputDir.x < 0) {
      scale.x = -Math.Abs(scale.x);
    }
    transform.localScale = scale;
  }

  private void Jump(InputAction.CallbackContext context) {
    if (physicsCheck.isGround) {
      rigidbody2D_.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
  }

  public void GetHurt(Transform attacker) {
    isHurt = true;
    rigidbody2D_.velocity = Vector2.zero;
    Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
    rigidbody2D_.AddForce(dir * hurtForce, ForceMode2D.Impulse);
  }

  public void PlayerDead() {
    isDead = true;
    inputControl.Gameplay.Disable();
    gameOverEvent.RaiseEvent();
  }

  private void Attack(InputAction.CallbackContext context) {
    isAttack = true;
    playerAnimator.PlayAttack();
  }

  private void CheckState() {
    collider2D_.sharedMaterial = physicsCheck.isGround ? normal : wall;
  }

  private void Confirm(InputAction.CallbackContext context) {
    confirmSign.targetItem?.TriggerAction();
  }

  private void OnSceneLoading(GameSceneSO scene, Vector3 arg1, bool arg2) {
    inputControl.Gameplay.Disable();
    canControl = (scene.type == SceneType.Location);
  }

  private void Restart() {
    isDead = false;
  }
}
