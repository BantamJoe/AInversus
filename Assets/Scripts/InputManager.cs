using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour {

  [SerializeField]
  List<InputUnit> inputs;


  public enum InputType {
    Down,
    Hold,
    Release
  }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
    for(int i = 0; i < inputs.Count; i++) {

      if (Input.GetKey(inputs[i].keycode)) {
        if (Input.GetKeyDown(inputs[i].keycode))
          inputs[i].inputEvent.Invoke(InputType.Down);
        else
          inputs[i].inputEvent.Invoke(InputType.Hold);
      }
      else if (Input.GetKeyUp(inputs[i].keycode))
        inputs[i].inputEvent.Invoke(InputType.Release);
    }

	}
}

[Serializable]
public struct InputUnit {
  public KeyCode keycode;
  public InputEvent inputEvent;
}

[System.Serializable]
public class InputEvent : UnityEvent<InputManager.InputType> { }
