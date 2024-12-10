using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataDefination : MonoBehaviour {
  public string ID;

  private void OnValidate() {
    if (ID == string.Empty) {
      ID = System.Guid.NewGuid().ToString();
    }
  }
}
