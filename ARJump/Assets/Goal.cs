﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    [SerializeField]float GoalDistance = 0.1f;

    [Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
    [SerializeField] float m_GroundCheckDistance = 0.1f;

    bool m_IsGrounded = false;
    float m_OrigGroundCheckDistance = 0.0f;
    Vector3 m_GroundNormal;
    Rigidbody m_Rigidbody;
    private GameObject firework;

    private bool isPlaced = false;
    private GameObject mario;
    private float distance;
	// Use this for initialization
	void Start () {
        firework = GameObject.FindGameObjectWithTag("Fire");
        firework.GetComponent<ParticleSystem>().Pause();
        GetComponent<MeshRenderer>().enabled = false;
        mario = GameObject.FindGameObjectWithTag("Mario");
        //m_Rigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		if(isPlaced)
        {
            distance = Vector3.Distance(this.transform.position, mario.transform.position);
            if(distance <= GoalDistance)
            {
                firework.GetComponent<ParticleSystem>().Play();
                var cube = GameObject.FindGameObjectWithTag("Cube");
                var bridge = GameObject.FindGameObjectWithTag("Bridge");
                cube.GetComponent<Cube>().restart();
                bridge.GetComponent<Bridge>().restart();
                
            }
        }
        //if (GetComponent<MeshRenderer>().enabled == true)
        //{
        //    if (!m_IsGrounded)
        //    {
        //        HandleAirborneMovement();
        //        transform.rotation = new Quaternion(0.0f, transform.rotation.y, 0.0f, 1.0f);

        //    }
        //}
    }

    public void placeGoal(Vector3 position)
    {
        if(!isPlaced)
        {
            transform.position = position;
            isPlaced = true;
        }
    }

    void HandleAirborneMovement()
    {
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
        m_Rigidbody.AddForce(extraGravityForce);

        m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
    }

    public void restart()
    {
        this.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        this.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        GetComponent<MeshRenderer>().enabled = false;
        this.isPlaced = false;
        firework = GameObject.FindGameObjectWithTag("Fire");
        firework.GetComponent<ParticleSystem>().Pause();
    }
}
