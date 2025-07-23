using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneSelector : MonoBehaviour
{
    public InputAction tapAction;
    public ARRaycastManager rayCastManager;
    public ARPlaneManager planeManager;
    public Camera arCamera;
    private PrintPlaneID printPlaneID;
    private GameObject mainCanvas;

    private List<ARRaycastHit> hits = new();

    private void Awake()
    {
        mainCanvas = GameObject.Find("MainCanvas");
        printPlaneID = mainCanvas.GetComponent<PrintPlaneID>();
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
        Vector2 screenPosition = context.ReadValue<Vector2>();

        if (rayCastManager.Raycast(screenPosition, hits, TrackableType.Planes))
        {
            ARRaycastHit hit = hits[0];
            TrackableId planeID = hit.trackableId;

            printPlaneID.PrintMessage(planeID.ToString());
            


        }
    }
}
