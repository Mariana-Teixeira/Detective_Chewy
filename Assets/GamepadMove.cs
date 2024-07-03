using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadMove : MonoBehaviour
{
    public Vector2 sensitivity = new Vector2(1500f, 1500f);
    public Vector2 bias = new Vector2(0f, -1f);

    // Cached variables
    Vector2 leftStick;
    Vector2 mousePosition;
    Vector2 warpPosition;

    // Stored for next frame
    Vector2 overflow;

    void Update()
    {
        if(Gamepad.current != null) { 

        // Get the joystick position
        leftStick = Gamepad.current.leftStick.ReadValue();

        // Prevent annoying jitter when not using joystick
        if (leftStick.magnitude < 0.1f) return;

        // Get the current mouse position to add to the joystick movement
        mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        // Precise value for desired cursor position, which unfortunately cannot be used directly
        warpPosition = mousePosition + bias + overflow + sensitivity * Time.deltaTime * leftStick;

        // Keep the cursor in the game screen (behavior gets weird out of bounds)
        warpPosition = new Vector2(Mathf.Clamp(warpPosition.x, 0, Screen.width), Mathf.Clamp(warpPosition.y, 0, Screen.height));

        // Store floating point values so they are not lost in WarpCursorPosition (which applies FloorToInt)
        overflow = new Vector2(warpPosition.x % 1, warpPosition.y % 1);

        // Move the cursor
        Mouse.current.WarpCursorPosition(warpPosition);
        }
    }

    }