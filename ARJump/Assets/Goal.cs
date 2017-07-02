using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    [SerializeField]float GoalDistance = 0.1f;
    private bool isPlaced = false;
    private GameObject mario;
    private float distance;
	// Use this for initialization
	void Start () {
        GetComponent<MeshRenderer>().enabled = false;
        mario = GameObject.FindGameObjectWithTag("Mario");
	}
	
	// Update is called once per frame
	void Update () {
		if(isPlaced)
        {
            distance = Vector3.Distance(this.transform.position, mario.transform.position);
            if(distance < GoalDistance)
            {

            }
        }
	}

    public void placeGoal()
    {

    }
}
