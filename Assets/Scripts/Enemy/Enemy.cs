using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Enemy : MonoBehaviour {
  protected Rigidbody2D rigidbody2D_;
  [HideInInspector] public Animator animator;
  [HideInInspector] public PhysicsCheck physicsCheck;

  [Header("基础属性")]
  public float normalSpeed;
  public float chaseSpeed;
  [HideInInspector] public float currentSpeed;

  [Header("受伤")]
  public float hurtForce;

  [Header("检测玩家")]
  public Vector2 findOffset;
  public Vector2 findSize;
  public float findDistance;
  public LayerMask findLayerMark;

  [Header("状态")]
  public bool isHurt;

  private Vector3 faceDir;

  protected BaseState currentState;
  protected BaseState patrolState;
  protected BaseState chaseState;

  public virtual void Awake() {
    rigidbody2D_ = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    physicsCheck = GetComponent<PhysicsCheck>();
  }

  private void OnEnable() {
    currentState.OnEnter();
  }

  private void OnDisable() {
    currentState.OnExit();
  }

  private void Update() {
    faceDir = new Vector3(transform.localScale.x, 0, 0).normalized;

    currentState.LogicUpdate();
  }

  private void FixedUpdate() {
    if (!isHurt) {
      Move();
    }
    currentState.PhysicsUpdate();
  }

  protected virtual void Move() {
    rigidbody2D_.velocity = new Vector2(currentSpeed * faceDir.x * Time.fixedDeltaTime, rigidbody2D_.velocity.y);
  }

  public void FlipX() {
    Vector3 scale = transform.localScale;
    scale.x = -scale.x;
    transform.localScale = scale;
  }

  #region 事件
  public void OnTakeDamage(Transform attacker) {
    if ((attacker.position.x - transform.position.x) * transform.localScale.x < 0) {
      FlipX();
    }
    isHurt = true;
    gameObject.layer = 2;
    animator.SetTrigger("hurt");
    Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
    rigidbody2D_.velocity = Vector2.zero;
    rigidbody2D_.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    StartCoroutine(HurtTrigger());
  }

  IEnumerator HurtTrigger() {
    yield return new WaitForSeconds(0.5f);
    isHurt = false;
    gameObject.layer = 8;
  }

  public void OnDie() {
    gameObject.layer = 2;
    animator.SetBool("dead", true);
  }

  public void DestroySelf() {
    Destroy(gameObject);
  }
  #endregion

  public void SwitchState(EnemyState state) {
    var newState = state switch {
      EnemyState.Patrol => patrolState,
      EnemyState.Chase => chaseState,
      _ => null
    };
    currentState.OnExit();
    currentState = newState;
    currentState.OnEnter();
  }

  public bool FoundPlayer() {
    return Physics2D.BoxCast((Vector2)transform.position + findOffset, findSize, 0, faceDir, findDistance, findLayerMark);
  }

  private void OnDrawGizmosSelected() {
    Gizmos.DrawWireCube((Vector2)transform.position + findOffset, findSize);
    Gizmos.DrawWireCube((Vector2)transform.position + findOffset + (Vector2)faceDir * findDistance, findSize);
  }
}
