using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour {

    [Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
    [SerializeField] float m_GroundCheckDistance = 0.1f;

    public bool isSet;
    bool m_IsGrounded;
    float m_OrigGroundCheckDistance;
    Vector3 m_GroundNormal;
    Rigidbody m_Rigidbody;

    // Use this for initialization
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        this.GetComponent<MeshRenderer>().enabled = false;
        isSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<MeshRenderer>().enabled == true)
        {
            if (!m_IsGrounded)
            {
                HandleAirborneMovement();
            }
        }
    }

    void HandleAirborneMovement()
    {
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
        m_Rigidbody.AddForce(extraGravityForce);

        m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
    }
}
