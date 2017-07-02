using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    [SerializeField]float GoalDistance = 0.1f;

    [Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
    [SerializeField] float m_GroundCheckDistance = 0.1f;

    bool m_IsGrounded;
    float m_OrigGroundCheckDistance;
    Vector3 m_GroundNormal;
    Rigidbody m_Rigidbody;

    private bool isPlaced = true;
    private GameObject mario;
    private float distance;
	// Use this for initialization
	void Start () {
        //GetComponent<MeshRenderer>().enabled = false;
        mario = GameObject.FindGameObjectWithTag("Mario");
        m_Rigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		if(isPlaced)
        {
            distance = Vector3.Distance(this.transform.position, mario.transform.position);
            if(distance <= GoalDistance)
            {
                Debug.Log("Finish");
            }
        }
        if (GetComponent<MeshRenderer>().enabled == true)
        {
            if (!m_IsGrounded)
            {
                HandleAirborneMovement();
                transform.rotation = new Quaternion(0.0f, transform.rotation.y, 0.0f, 1.0f);

            }
        }
    }

    public void placeGoal()
    {

    }

    void HandleAirborneMovement()
    {
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
        m_Rigidbody.AddForce(extraGravityForce);

        m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
    }
}
