using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour {
  
  public RandomActions RandomPlayer;
  public Player AIPlayer;
  public Player AIOpponent;

  public bool UseRandomOpponent = true;

	// Use this for initialization
	void Start () {
    Application.runInBackground = true;
    StartCoroutine(gameLoop());
	}
	
  IEnumerator gameLoop() {

    while(true) {

      AIPlayer.GetComponent<AInversusAgent>().RequestDecision();

      if (UseRandomOpponent)
        RandomPlayer.DoAction();
      else
        AIOpponent.GetComponent<AInversusAgent>().RequestDecision();

      yield return new WaitForSeconds(0.2f);
    }

  }

  private void OnGUI() {
    Time.timeScale = GUI.HorizontalSlider(new Rect(5, 10, 80, 10), Time.timeScale, 1, 100);
  }
}
