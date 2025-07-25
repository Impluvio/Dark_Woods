using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static event Action OnTap;
    private UserInput userInput;

    private void Awake()
    {
        userInput = new UserInput();
        userInput.Enable();

    }

    private void Start()
    {
        userInput.MobileTouch.Tap.performed += OnTapPerformed;

    }

    private void OnTapPerformed(InputAction.CallbackContext context)
    {
        OnTap?.Invoke();
    }

    private void OnDestroy()
    {
        userInput.MobileTouch.Tap.performed -= OnTapPerformed;
    }
}
