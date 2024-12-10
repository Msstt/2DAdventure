using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Charactor : MonoBehaviour, ISaveable {
  [Header("基础属性")]
  public int maxHealth;
  public int currentHealth;

  [Header("受伤无敌")]
  public float invulnerableDuration;
  private float invulnerableCounter;
  [SerializeField] private bool isInvulnerable;

  [Header("受伤")]
  public UnityEvent<Transform> onTakeDamage;

  [Header("死亡")]
  public UnityEvent onDie;

  [Header("事件监听")]
  public CharactorEventSO onHealthChange;
  public VoidEventSO newGameEvent;

  private void OnEnable() {
    newGameEvent.onEventRaised += NewGame;
    ISaveable saveable = this;
    saveable.RegisterSaveData();
  }

  private void OnDisable() {
    newGameEvent.onEventRaised -= NewGame;
    ISaveable saveable = this;
    saveable.UnRegisterSaveData();
  }

  private void NewGame() {
    currentHealth = maxHealth;
    if (onHealthChange != null) {
      onHealthChange.RaiseEvent(this);
    }
  }

  private void Update() {
    if (isInvulnerable) {
      invulnerableCounter -= Time.deltaTime;
      if (invulnerableCounter <= 0) {
        isInvulnerable = false;
      }
    }
  }

  public void TakeDamage(Attacker attacker) {
    if (isInvulnerable) {
      return;
    }

    if (currentHealth - attacker.damage > 0) {
      Hurt(attacker.damage, attacker.transform);
    } else {
      Die();
    }
  }

  private void InvulnerStart() {
    isInvulnerable = true;
    invulnerableCounter = invulnerableDuration;
  }

  private void Hurt(int damage, Transform transform) {
    currentHealth -= damage;
    InvulnerStart();
    onTakeDamage?.Invoke(transform);

    if (onHealthChange != null) {
      onHealthChange.RaiseEvent(this);
    }
  }

  private void Die() {
    currentHealth = 0;
    onDie?.Invoke();

    if (onHealthChange != null) {
      onHealthChange.RaiseEvent(this);
    }
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Water")) {
      Die();
    }
  }

  public DataDefination GetDataId() {
    return GetComponent<DataDefination>();
  }

  public void SaveData(Data data) {
    data.charactorPosDict[GetDataId().ID] = transform.position;
    data.charactorHealthDict[GetDataId().ID] = currentHealth;
  }

  public void LoadData(Data data) {
    if (data.charactorPosDict.ContainsKey(GetDataId().ID)) {
      transform.position = data.charactorPosDict[GetDataId().ID];
    }
    if (data.charactorHealthDict.ContainsKey(GetDataId().ID)) {
      currentHealth = data.charactorHealthDict[GetDataId().ID];
      if (onHealthChange != null) {
        onHealthChange.RaiseEvent(this);
      }
    }
  }
}
