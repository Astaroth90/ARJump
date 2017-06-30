using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA.Input;
using HoloLensXboxController;
using System.Linq;
using System;

public class ControlManager : MonoBehaviour {

    public InputManager input;
    public ControllerInput controller;

    // KeywordRecognizer object.
    KeywordRecognizer keywordRecognizer;

    // Defines which function to call when a keyword is recognized.
    delegate void KeywordAction(PhraseRecognizedEventArgs args);
    Dictionary<string, KeywordAction> keywordCollection;

    // Use this for initialization
    void Start () {
        //input = InputManager.Instance;
        //GesturesInput gesture = input.GetComponent<GesturesInput>();

        // GestureRecognizer recognizer = gesture.GetComponent<GestureRecognizer>();

        keywordCollection = new Dictionary<string, KeywordAction>();

        // Add keyword to start manipulation.
        keywordCollection.Add("Restart", RestartGame);


        // Initialize KeywordRecognizer with the previously added keywords.
        keywordRecognizer = new KeywordRecognizer(keywordCollection.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        KeywordAction keywordAction;

        if (keywordCollection.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke(args);
        }
    }


    private void RestartGame(PhraseRecognizedEventArgs args)
    {
        var cube = GameObject.FindGameObjectWithTag("Cube");
        var bridge = GameObject.FindGameObjectWithTag("Bridge");
        var mario = GameObject.FindGameObjectWithTag("Mario");
        cube.GetComponent<Cube>().restart();
        bridge.GetComponent<Cube>().restart();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
