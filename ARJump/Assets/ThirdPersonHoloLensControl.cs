using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloLensXboxController;
using HoloToolkit.Unity;
using UnityStandardAssets.Characters.ThirdPerson;
using HoloToolkit.Unity.SpatialMapping;

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

    private bool bridgepick = false;
    private bool cubepick = false;
    private bool playerpick = false;


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
#if NETFX_CORE
        if (!m_Jump)
        {
            m_Jump = controllerInput.GetButton(ControllerButton.A);
        }
#else
        if (!m_Jump)
        {
            m_Jump = Input.GetButton("XboxA");
        }
#endif
    }


    private void FixedUpdate()
    {
#if NETFX_CORE
        controllerInput.Update();

        // read inputs
        float h = MoveHorizontalSpeed * controllerInput.GetAxisLeftThumbstickX();
        float v = MoveVerticalSpeed * controllerInput.GetAxisLeftThumbstickY();
        float rotateH = MoveHorizontalSpeed / 10 * controllerInput.GetAxisRightThumbstickY();
        bool crouch = controllerInput.GetButton(ControllerButton.B);
        bool set = controllerInput.GetButton(ControllerButton.X);
        bool box = controllerInput.GetButton(ControllerButton.RightShoulder);
        bool bridge = controllerInput.GetButton(ControllerButton.LeftShoulder);
        bool mesh = controllerInput.GetButton(ControllerButton.DPadUp);
        bool mapping = controllerInput.GetButton(ControllerButton.DPadDown);

        var curser = GameObject.FindGameObjectWithTag("Cursor");
        Vector3 newposition = curser.transform.position;
        newposition.y += 0.5f;

#else
        float h = MoveHorizontalSpeed * Input.GetAxis("Horizontal");
        float v = MoveVerticalSpeed * Input.GetAxis("Vertical");
        float ch = MoveHorizontalSpeed / 10 * Input.GetAxis("RHorizontal");
        float cv = MoveVerticalSpeed / 10 * Input.GetAxis("RVertical");
        
        bool crouch = false;
        bool set = Input.GetButton("XboxX");
        bool box = Input.GetButton("RightBumper");
        bool bridge = Input.GetButton("LeftBumper");
        bool mesh = false;
        bool mapping = false;



        var curser = GameObject.FindGameObjectWithTag("Cursor");
        Vector3 newposition = curser.transform.position;
        newposition.y += 0.5f;
#endif

        // calculate move direction to pass to character
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
#if NETFX_CORE

#else
            m_Cam.transform.rotation = new Quaternion(m_Cam.transform.rotation.x + cv, m_Cam.transform.rotation.y + ch, 0.0f, 1.0f);
#endif
            m_Move = v * m_CamForward + h * m_Cam.right;
        }
        if (set)
        {
            m_Character.transform.position = newposition;
            m_Character.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        }
        else
        {
            if (box)
            {
                var cube = GameObject.FindGameObjectWithTag("Cube");
                if (!cubepick)
                {
                    cube.GetComponent<MeshRenderer>().enabled = true;
                    cube.transform.position = newposition;
                    cubepick = true;
                }
            }
            else
            {
                cubepick = false;
            }

            var bridgeO = GameObject.FindGameObjectWithTag("Bridge");
            if (bridge)
            {

                bridgeO.GetComponent<MeshRenderer>().enabled = true;

                if (!bridgepick)
                {
                    bridgeO.GetComponent<Rigidbody>().useGravity = false;
                    bridgeO.transform.position = newposition;

                    bridgepick = true;

                    bridgeO.transform.rotation = new Quaternion(0.0f, (m_Cam.transform.rotation.y), 0.0f, 1.0f);
                }
            }
            else
            {
                bridgeO.GetComponent<Rigidbody>().useGravity = true;
                bridgepick = false;
            }
            if (mesh)
            {
                
                if (SpatialUnderstanding.Instance.UnderstandingCustomMesh.DrawProcessedMesh == true)
                {
                    SpatialUnderstanding.Instance.UnderstandingCustomMesh.DrawProcessedMesh = false;
                }
                else
                {
                    SpatialUnderstanding.Instance.UnderstandingCustomMesh.DrawProcessedMesh = true;
                }
            }
            if(mapping)
            {
                var map = GameObject.FindGameObjectWithTag("Mesh");
                if (map.GetComponent<SpatialMappingManager>().drawVisualMeshes == true)
                {
                    map.GetComponent<SpatialMappingManager>().drawVisualMeshes = false;
                }
                else
                {
                    map.GetComponent<SpatialMappingManager>().drawVisualMeshes = true;
                }
                
            }

        }


        // pass all parameters to the character control script
        m_Character.Move(m_Move, crouch, m_Jump);
        m_Jump = false;
    }
}
