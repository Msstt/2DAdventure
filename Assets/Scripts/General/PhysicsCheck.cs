using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour {
  private CapsuleCollider2D capsuleCollider2D;
  private Vector2 forwardOffset;

  [Header("检测参数")]
  public Vector2 buttomOffset;
  public float checkRaduis;
  public LayerMask groundLayer;

  [Header("状态")]
  public bool isGround;
  public bool touchWall;

  private float ScaleX;

  private void Awake() {
    capsuleCollider2D = GetComponent<CapsuleCollider2D>();

    forwardOffset = new Vector2(capsuleCollider2D.bounds.size.x / 2 + capsuleCollider2D.offset.x, capsuleCollider2D.offset.y);
    ScaleX = transform.localScale.x;
  }

  private void Update() {
    UpdateOffset();
    Check();
  }

  private void Check() {
    isGround = Physics2D.OverlapCircle((Vector2)transform.position + buttomOffset, checkRaduis, groundLayer);
    touchWall = Physics2D.OverlapCircle((Vector2)transform.position + forwardOffset, checkRaduis, groundLayer);
  }

  private void UpdateOffset() {
    if (transform.localScale.x * ScaleX < 0) {
      forwardOffset.x = -forwardOffset.x;
      buttomOffset.x = -buttomOffset.x;
    }
    ScaleX = transform.localScale.x;
  }

  private void OnDrawGizmosSelected() {
    Gizmos.DrawWireSphere((Vector2)transform.position + buttomOffset, checkRaduis);
    Gizmos.DrawWireSphere((Vector2)transform.position + forwardOffset, checkRaduis);
  }
}
