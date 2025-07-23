using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneSelector : MonoBehaviour
{
    private InputAction tapAction;
    public ARRaycastManager rayCastManager;
    public ARPlaneManager planeManager;
    public Camera arCamera;
    private PrintPlaneID printPlaneID;
    private GameObject mainCanvas;

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



        if (rayCastManager.Raycast(screenPosition, hits, TrackableType.Planes))
        {
            ARRaycastHit hit = hits[0];
            TrackableId planeID = hit.trackableId;

            printPlaneID.PrintMessage(planeID.ToString());
            


        }
    }
}
