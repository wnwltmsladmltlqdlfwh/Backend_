using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class Input_Manager
{
    private TouchInput touchInput;

    public delegate void StartToruchEvent(Vector2 position, float time);
    public event StartToruchEvent OnStartTouch;
    public delegate void EndToruchEvent(Vector2 position, float time);
    public event EndToruchEvent OnEndTouch;

    public void Init()
    {
        touchInput = new TouchInput();
    }

    public void OnEnable()
    {
        touchInput.Enable();
    }

    public void Update()
    {

    }

    public void StartInputManager()
    {
        touchInput.Touch.TouchPress.started -= ctx => StartTouch(ctx);
        touchInput.Touch.TouchPress.canceled -= ctx => EndTouch(ctx);

        touchInput.Touch.TouchPress.started += ctx => StartTouch(ctx);
        touchInput.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        if(OnStartTouch != null)
        {
            OnStartTouch(touchInput.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
        }
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null)
        {
            OnEndTouch(touchInput.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
        }
    }

    public void OnDisable()
    {
        touchInput.Disable();
    }
}
