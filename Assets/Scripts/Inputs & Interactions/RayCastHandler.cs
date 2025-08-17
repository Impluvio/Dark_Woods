using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RayCastHandler : MonoBehaviour
{
    public Camera arCamera;
    public InputAction tapAction;

    public static event Action<Ray> OnRayCast;

    private void Awake()
    {
        tapAction = new InputAction(type: InputActionType.PassThrough);
        tapAction.AddBinding("<Touchscreen>/primaryTouch/press");
        tapAction.AddBinding("<Mouse>/leftButton");
    }

    private void OnEnable()
    {
        tapAction.Enable();
        tapAction.performed += HandleTap;
    }

    void OnDisable()
    {
        tapAction.performed -= HandleTap;
        tapAction.Disable();
    }

    private void HandleTap(InputAction.CallbackContext context)
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

        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        OnRayCast?.Invoke(ray);

    }


}
