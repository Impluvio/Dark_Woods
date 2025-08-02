using System;
using System.Collections;
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
    private ARAnchor playAreaAnchor;

    private PrintPlaneID printPlaneID;
    public MapCreator mapCreator;

    private bool playAreaSelected = false;

    private List<ARRaycastHit> hits = new();

    
    public GameObject origin { get; set; } 

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
                Vector3 worldCentre = plane.transform.TransformPoint(plane.center);
                Pose centerPose = new Pose(worldCentre, plane.transform.rotation);

                AttachAnchor(anchorManager, plane, centerPose);
                //printPlaneID.PrintMessage("Transform of anchor:" + playAreaAnchor.transform.position);
                TrackableId playAreaID = playAreaAnchor.trackableId;
                mapCreator.InitialiseMap(playAreaID);
                playAreaSelected = true;

            }
        }
    }

     void AttachAnchor(ARAnchorManager arAnchorManager, ARPlane plane, Pose pose)
    {
        if (arAnchorManager.descriptor.supportsTrackableAttachments)
        {
            playAreaAnchor = arAnchorManager.AttachAnchor(plane, pose);
            
        }
    }



}
