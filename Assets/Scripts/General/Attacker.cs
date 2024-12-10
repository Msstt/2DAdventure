using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour {
  public int damage;

  private void OnTriggerStay2D(Collider2D other) {
    other.GetComponent<Charactor>()?.TakeDamage(this);
  }
}
