using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneSelector : MonoBehaviour
{
    
    public ARRaycastManager rayCastManager;
    public ARPlaneManager planeManager;
    public ARAnchorManager anchorManager;

    private InputAction tapAction;
    public Camera arCamera;
    private PrintPlaneID printPlaneID;
    private ARAnchor playAreaAnchor;

    private bool playAreaSelected = false;

    private List<ARRaycastHit> hits = new();

    private void Awake()
    {
        
        printPlaneID = GetComponent<PrintPlaneID>();
        tapAction = new InputAction(type: InputActionType.PassThrough);
        tapAction.AddBinding("<Touchscreen>/primaryTouch/press");
        tapAction.AddBinding("<Mouse>/leftButton");
    }

    private void OnEnable()
    {
        tapAction.Enable();
        tapAction.performed += onTapPerformed;
    }

    private void OnDisable()
    {
        tapAction.performed -= onTapPerformed;
        tapAction.Disable();
    }

    public void onTapPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("tap performed");

        Vector2 screenPosition;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            screenPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            screenPosition = Mouse.current.position.ReadValue();
        }
        else
        {
            return;
        }

        //Sets the anchor and sets play area to true.

        if (rayCastManager.Raycast(screenPosition, hits, TrackableType.Planes) && !playAreaSelected)
        {
            ARRaycastHit hit = hits[0];
            TrackableId planeID = hit.trackableId;

            if (planeManager.trackables.TryGetTrackable(planeID, out ARPlane plane))
            {
                Vector3 centrePosition = plane.transform.position + (Vector3)plane.center;
                Pose centerPose = new Pose(centrePosition, plane.transform.rotation);

                playAreaAnchor = anchorManager.AttachAnchor(plane, centerPose);

                if (playAreaAnchor != null)
                {
                    playAreaSelected = true;
                    printPlaneID.PrintMessage(playAreaAnchor.transform.ToString());
                    //call main logic process from here.
                }

            }

            // DEBUGING: printPlaneID.PrintMessage(planeID.ToString());

            //Pose pose = hit.pose;
            //Vector3 center = plane.center;
            //Quaternion rotation = plane.transform.rotation;
            //Vector3 size = plane.size;

        }
        else
        {
            printPlaneID.PrintMessage("plane or planeID not found");
        }
    }
}
