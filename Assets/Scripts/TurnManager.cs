using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

  public RandomActions RandomPlayer;
  public Player AIPlayer;

	// Use this for initialization
	void Start () {
    Application.runInBackground = true;
    StartCoroutine(gameLoop());
	}
	
  IEnumerator gameLoop() {

    while(true) {

      AIPlayer.GetComponent<AInversusAgent>().RequestDecision();
      RandomPlayer.DoAction();

      yield return new WaitForSeconds(0.2f);
    }

  }
}
