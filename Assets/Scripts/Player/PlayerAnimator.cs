using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
  private Animator animator;
  private Rigidbody2D rigidbody2D_;
  private PhysicsCheck physicsCheck;
  private PlayerConroller playerConroller;
  private void Awake() {
    animator = GetComponent<Animator>();
    rigidbody2D_ = GetComponent<Rigidbody2D>();
    physicsCheck = GetComponent<PhysicsCheck>();
    playerConroller = GetComponent<PlayerConroller>();
  }

  private void Update() {
    SetAnimation();
  }

  private void SetAnimation() {
    animator.SetFloat("velocityX", Mathf.Abs(rigidbody2D_.velocity.x));
    animator.SetFloat("velocityY", rigidbody2D_.velocity.y);
    animator.SetBool("isGround", physicsCheck.isGround);
    animator.SetBool("isDead", playerConroller.isDead);
    animator.SetBool("isAttack", playerConroller.isAttack);
  }

  public void PlayHurt() {
    animator.SetTrigger("hurt");
  }

  public void PlayAttack() {
    animator.SetTrigger("attack");
  }
}
