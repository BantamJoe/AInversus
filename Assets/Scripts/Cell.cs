using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

  [SerializeField]
  Material black, white = null;

  [SerializeField]
  float colorChangeTime;

  GameObject background, color;
  Vector3 originalScale;

  public CellState initialState;

  // Use this for initialization
  void Start () {
    background = transform.Find("Background").gameObject;
    color = transform.Find("Color").gameObject;
    originalScale = color.transform.localScale;

    if (initialState == CellState.White)
      color.GetComponent<Renderer>().material = white;
    else
      color.GetComponent<Renderer>().material = black;
  }

  public void SetState(CellState newState, System.Action completed) {
    routine = StartCoroutine(ChangeColor(newState, () => { }));
  }

  Coroutine routine;

  IEnumerator ChangeColor(CellState newState, System.Action completed) {
    background.GetComponent<MeshRenderer>().material = newState == CellState.White ? white : black;

    for (float i = 0; i < 1; i += Time.deltaTime / colorChangeTime) {

      color.transform.localScale = originalScale * (1 - i);
      yield return null;
    }
    color.transform.localScale = originalScale;
    color.GetComponent<MeshRenderer>().material = newState == CellState.White ? white : black;

    completed();
  }

  public void SetStateInstant(CellState newState) {

    if (routine != null)
      StopCoroutine(routine);

    background.GetComponent<MeshRenderer>().material = newState == CellState.White ? white : black;
    color.transform.localScale = originalScale;
    color.GetComponent<MeshRenderer>().material = newState == CellState.White ? white : black;
  }
}

public enum CellState {
  White,
  Black,
  Invalid
}
