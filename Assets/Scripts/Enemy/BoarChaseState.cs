using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState {
  Enemy self;
  private float missTimer;
  private float missDuration = 2;
  public BoarChaseState(Enemy enemy) {
    self = enemy;
  }

  public override void OnEnter() {
    self.animator.SetBool("run", true);
    self.currentSpeed = self.chaseSpeed;
  }

  public override void LogicUpdate() {
    if (self.physicsCheck.touchWall || !self.physicsCheck.isGround) {
      self.FlipX();
    }
    UpdateTimer();
  }

  public override void PhysicsUpdate() {
  }

  public override void OnExit() {
    self.animator.SetBool("run", false);
  }

  private void UpdateTimer() {
    if (self.FoundPlayer()) {
      missTimer = missDuration;
    } else {
      if (missTimer > 0) {
        missTimer -= Time.deltaTime;
      }
      if (missTimer <= 0) {
        self.SwitchState(EnemyState.Patrol);
      }
    }
  }
}
