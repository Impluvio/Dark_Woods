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
    private ARAnchor playAreaAnchor;

    private PrintPlaneID printPlaneID;
    private MapCreator mapCreator;

    private bool playAreaSelected = false;

    private List<ARRaycastHit> hits = new();

    private void Awake()
    {
        mapCreator = GetComponent<MapCreator>();
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
       // Debug.Log("tap performed");

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


                //printPlaneID.PrintMessage("plane centre: " + worldCentre.ToString());
                //printPlaneID.PrintMessage("plane ID: " + planeID.ToString());
                //printPlaneID.PrintMessage("plane pos: " + plane.center.ToString());

                playAreaAnchor = anchorManager.AttachAnchor(plane, centerPose);

               // printPlaneID.PrintMessage("Anchor placement method passed");

                //if (mapCreator == null)
                //{
                //    Debug.Log("map creator null");
                //}
                //else if (mapCreator.worldOrigin == null)
                //{
                //    Debug.Log("world origin null");
                //}
                //else if (playAreaAnchor == null)
                //{
                //    Debug.Log("PlayerAreaAnchor is null");
                //}



                if (playAreaAnchor != null )
                {
                    printPlaneID.PrintMessage("Anchor not set to null");
                    printPlaneID.PrintMessage("Transform of anchor:" + playAreaAnchor.transform.position);

                    playAreaSelected = true;
                    mapCreator.worldOrigin = playAreaAnchor;
                    printPlaneID.PrintMessage("world origin: " + mapCreator.worldOrigin.transform.position);
                    mapCreator.updateMap();
                    printPlaneID.PrintMessage("Anchor Set");
                    
                    //printPlaneID.PrintMessage(playAreaAnchor.transform.ToString());
                    //call main logic process from here.
                }
                else
                {
                    printPlaneID.PrintMessage("plane or planeID not found");
                }
            }

           
        }
        
    }
}
