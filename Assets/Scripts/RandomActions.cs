using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomActions : MonoBehaviour {

  [SerializeField]
  List<InputEvent> actionPool;

  public bool automatic = false;

	// Use this for initialization
	void Start () {
    if(automatic)
      StartCoroutine(action());
	}
	
  public void DoAction() {
    actionPool[Random.Range(0, actionPool.Count)].Invoke(InputManager.InputType.Down);
  }
	
  IEnumerator action() {
    while (true) {
      yield return new WaitForSeconds(1);
      actionPool[Random.Range(0, actionPool.Count - 1)].Invoke(InputManager.InputType.Down);
    }
  }
}
