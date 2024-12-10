using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarPatrolState : BaseState {
  Enemy self;

  private float touchWallWaitDuration = 1;
  private float touchWallWaitTimer = 0;
  private bool touchWallWait = false;

  public BoarPatrolState(Enemy enemy) {
    self = enemy;
  }
  public override void OnEnter() {
    self.currentSpeed = self.normalSpeed;
    self.animator.SetBool("walk", true);
  }

  public override void LogicUpdate() {
    if (self.FoundPlayer()) {
      self.SwitchState(EnemyState.Chase);
      return;
    }
    TouchWallCheck();
  }

  public override void PhysicsUpdate() {
  }

  public override void OnExit() {
    self.animator.SetBool("walk", false);
  }

  private void TouchWallCheck() {
    if (!touchWallWait && (self.physicsCheck.touchWall || !self.physicsCheck.isGround)) {
      TouchWallWait();
      self.animator.SetBool("walk", false);
    }
    if (touchWallWaitTimer > 0) {
      touchWallWaitTimer -= Time.deltaTime;
      if (touchWallWaitTimer <= 0) {
        touchWallWait = false;
        self.FlipX();
        self.animator.SetBool("walk", true);
        self.currentSpeed = self.normalSpeed;
      }
    }
  }

  protected virtual void TouchWallWait() {
    touchWallWait = true;
    touchWallWaitTimer = touchWallWaitDuration;
    self.currentSpeed = 0;
  }
}
