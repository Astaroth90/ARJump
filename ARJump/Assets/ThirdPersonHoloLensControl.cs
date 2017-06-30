using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloLensXboxController;
using UnityStandardAssets.Characters.ThirdPerson;

public class ThirdPersonHoloLensControl : MonoBehaviour
{

    public ControllerInput controllerInput;
    private ThirdPersonCharacter m_Character;
    private Transform m_Cam;
    private Vector3 m_CamForward;
    private Vector3 m_Move;
    private bool m_Jump;

    public float RotateAroundYSpeed = 2.0f;
    public float RotateAroundXSpeed = 2.0f;
    public float RotateAroundZSpeed = 2.0f;

    public float MoveHorizontalSpeed = 1f;
    public float MoveVerticalSpeed = 1f;

    public float ScaleSpeed = 1f;


    void Start()
    {
        controllerInput = new ControllerInput(0, 0.19f);
        Debug.Log(controllerInput.GetType());
        Debug.Log("Controller_Start");
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }

        m_Character = GetComponent<ThirdPersonCharacter>();
        Debug.Log(m_Character.gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Jump)
        {
            m_Jump = controllerInput.GetButton(ControllerButton.A);
        }
    }


    private void FixedUpdate()
    {
        controllerInput.Update();

        // read inputs
        float h = MoveHorizontalSpeed * controllerInput.GetAxisLeftThumbstickX();
        float v = MoveVerticalSpeed * controllerInput.GetAxisLeftThumbstickY();
        bool crouch = controllerInput.GetButton(ControllerButton.B);
        bool set = controllerInput.GetButton(ControllerButton.X);
        bool box = controllerInput.GetButton(ControllerButton.RightShoulder);
        bool bridge = controllerInput.GetButton(ControllerButton.LeftShoulder);

        var curser = GameObject.FindGameObjectWithTag("Cursor");
        Vector3 newposition = curser.transform.position;
        newposition.y += 0.5f;

        // calculate move direction to pass to character
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v * m_CamForward + h * m_Cam.right;
        }
        if (set == true)
        {
            Debug.Log("set");
            Debug.Log(curser.transform.position.ToString());

            m_Character.transform.position = newposition;
            m_Character.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        }
        else
        {
            if (box)
            {
                var cube = GameObject.FindGameObjectWithTag("Cube");
                cube.GetComponent<MeshRenderer>().enabled = true;
                cube.transform.position = newposition;
            }

            if (bridge)
            {
                var bridgeO = GameObject.FindGameObjectWithTag("Bridge");
                bridgeO.GetComponent<MeshRenderer>().enabled = true;
                bridgeO.transform.position = newposition;
            }

        }
        // pass all parameters to the character control script
        m_Character.Move(m_Move, crouch, m_Jump);
        m_Jump = false;
    }
}
