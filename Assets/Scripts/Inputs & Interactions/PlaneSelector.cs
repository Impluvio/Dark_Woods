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
    [SerializeField] private GameObject Marker;
    private GameObject liveMarker;

    public MapCreator mapCreator;
    public UiManager uiManager;
    public TrackableId playAreaID;

    private bool playAreaSelected = false;

    private List<ARRaycastHit> hits = new();
    public GameObject origin { get; set; }

    private void Awake()
    {
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
                playAreaID = playAreaAnchor.trackableId;
                liveMarker = Instantiate(Marker, playAreaAnchor.transform); // this works but can be repeated ad infinitum - needs to be once
                confirmGamePlayLocation(true);
                            //Todo: turn off planes, once anchor and grid is established. 
                            //Add confirm play area rule so that users can instantiate grid elsewhere. 


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

    private void confirmGamePlayLocation(bool turnOn)
    {
        uiManager.displayQueryPlacement(turnOn);

    }

    public void setPlayArea(bool areaSet)
    {
        if (!areaSet)
        {
            Destroy(liveMarker);
            confirmGamePlayLocation(false);


        }
        else
        {
            mapCreator.InitialiseMap(playAreaID);
            Debug.Log("area selected");
            confirmGamePlayLocation(false);
            playAreaSelected = true;
            turnOffPlanes();
        }
    }

    public void turnOffPlanes()
    {
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }

        planeManager.requestedDetectionMode = PlaneDetectionMode.None;
    }

}
