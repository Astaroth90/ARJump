using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    [Range(1f, 4f)][SerializeField]float m_GravityMultiplier = 2f;

    bool m_IsGrounded;
    float m_OrigGroundCheckDistance;
    Vector3 m_GroundNormal;

    // Use this for initialization
    void Start () {
        this.GetComponent<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Awake()
    {
        
    }
}
