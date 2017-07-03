using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA.Input;

public class GestureAction : MonoBehaviour {

    GesturesInput gesture;
    InputManager input;

	// Use this for initialization
	void Start () {
        
        gesture = GameObject.FindGameObjectWithTag("Manager").GetComponent<GesturesInput>();
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
