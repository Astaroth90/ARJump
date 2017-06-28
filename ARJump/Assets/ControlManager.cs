using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA.Input;
using HoloLensXboxController;

public class ControlManager : MonoBehaviour {

    public InputManager input;
    public ControllerInput controller;

	// Use this for initialization
	void Start () {
        input = InputManager.Instance;
        GesturesInput gesture = input.GetComponent<GesturesInput>();

       // GestureRecognizer recognizer = gesture.GetComponent<GestureRecognizer>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
