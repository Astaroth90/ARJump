using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using HoloToolkit.Unity;
using System;

public class OrchestrateGame : MonoBehaviour
{

    private ThirdPersonCharacter m_Character;
    private bool m_isInitialized;
    public float kMinAreaForComplete = 50.0f;
    public float kMinHorizAreaForComplete = 25.0f;
    public float kMinWallAreaForComplete = 10.0f;

    // Use this for initialization
    void Start()
    {
        Debug.Log("StartOrchestrate");
        m_Character = GetComponent<ThirdPersonCharacter>();
        SpatialUnderstanding.Instance.ScanStateChanged += Instance_ScanStateChanged;
        //HideCharacter();
    }
    private void Instance_ScanStateChanged()
    {
        Debug.Log("Instance_ScanStateChanged");
        Debug.Log((SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Done).ToString());
        Debug.Log(SpatialUnderstanding.Instance.AllowSpatialUnderstanding.ToString());
#if NETFX_CORE
        if ((SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Done) &&
        SpatialUnderstanding.Instance.AllowSpatialUnderstanding)
#else
        if ((true) &&
        SpatialUnderstanding.Instance.AllowSpatialUnderstanding)
#endif
        {
            PlaceCharacterInGame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // check if enough of the room is scanned
        if (!m_isInitialized && DoesScanMeetMinBarForCompletion)
        {
            Debug.Log("updateInside");
            // let service know we're done scanning
            SpatialUnderstanding.Instance.RequestFinishScan();
            m_isInitialized = true;
        }
    }

    public bool DoesScanMeetMinBarForCompletion
    {
        get
        {            
            // Only allow this when we are actually scanning
            if ((SpatialUnderstanding.Instance.ScanState != SpatialUnderstanding.ScanStates.Scanning) ||
                (!SpatialUnderstanding.Instance.AllowSpatialUnderstanding))
            {
                return false;
            }

            // Query the current playspace stats
            IntPtr statsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStatsPtr();
            if (SpatialUnderstandingDll.Imports.QueryPlayspaceStats(statsPtr) == 0)
            {
                return false;
            }
            SpatialUnderstandingDll.Imports.PlayspaceStats stats = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStats();

            // Check our preset requirements
            if ((stats.TotalSurfaceArea > kMinAreaForComplete) ||
                (stats.HorizSurfaceArea > kMinHorizAreaForComplete) ||
                (stats.WallSurfaceArea > kMinWallAreaForComplete))
            {
                return true;
            }
            return false;
        }
    }

    private void ShowCharacter(Vector3 placement)
    {
        var ethanBody = GameObject.Find("EthanBody");
        Debug.Log("showCharacter");
        ethanBody.GetComponent<SkinnedMeshRenderer>().enabled = true;
        m_Character.transform.position = placement;
        var rigidBody = GetComponent<Rigidbody>();
        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.velocity = Vector3.zero;
    }

    private void HideCharacter()
    {
        Debug.Log("HideCharacter");
        var ethanBody = GameObject.Find("EthanBody");
        ethanBody.GetComponent<SkinnedMeshRenderer>().enabled = false;
    }

    private void PlaceCharacterInGame()
    {
        Debug.Log("PlaceCharacter");
        // use spatial understanding to find floor
        SpatialUnderstandingDll.Imports.QueryPlayspaceAlignment(SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceAlignmentPtr());
        SpatialUnderstandingDll.Imports.PlayspaceAlignment alignment = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceAlignment();

        // find 2 meters in front of camera position
        var inFrontOfCamera = Camera.main.transform.position + Camera.main.transform.forward * 2.0f;

        // place character on floor 2 meters ahead
        ShowCharacter(new Vector3(inFrontOfCamera.x, alignment.FloorYValue, 2.69f));

        // hide mesh
        var customMesh = SpatialUnderstanding.Instance.GetComponent<SpatialUnderstandingCustomMesh>();
        customMesh.DrawProcessedMesh = true;
        customMesh.MeshMaterial.color = new Color(25.0f, 200.0f, 25.0f, 150.0f);
        

    }
}
