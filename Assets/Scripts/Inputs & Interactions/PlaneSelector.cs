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
    public MapCreator mapCreator;
    public UiManager uiManager;

    [SerializeField] private GameObject Marker;

    private ARAnchor playAreaAnchor;
    private GameObject liveMarker;
  
    public TrackableId playAreaID;
    private bool playAreaSelected = false;
    private List<ARRaycastHit> hits = new();


    private void OnEnable() => RayCastHandler.OnRayCast += HandleRayCast;
    private void OnDisable() => RayCastHandler.OnRayCast -= HandleRayCast;
  

    void HandleRayCast(Ray ray)
    {
        if (playAreaSelected) return;

        if (rayCastManager.Raycast(ray, hits, TrackableType.Planes))
        {
            ARRaycastHit hit = hits[0];
            TrackableId planeID = hit.trackableId; // plane to process

            if (planeManager.trackables.TryGetTrackable(planeID, out ARPlane plane))
            {
                Pose centerPoseOfPlane = new Pose(plane.transform.TransformPoint(plane.center), plane.transform.rotation);

                AttachAnchor(anchorManager, plane, centerPoseOfPlane);
                liveMarker = Instantiate(Marker, playAreaAnchor.transform);
                uiManager.displayQueryPlacement(true);
                playAreaID = playAreaAnchor.trackableId;
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

    public void setPlayArea(bool areaSet)
    {
        if (!areaSet)
        {
            Destroy(liveMarker);
            uiManager.displayQueryPlacement(false);


        }
        else
        {
            mapCreator.InitialiseMap(playAreaID);
            Debug.Log("area selected");
            uiManager.displayQueryPlacement(false);
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
