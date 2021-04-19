using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    // Exact pixel position of the touch/mouse on screen ((0, 0) = bottom left)
    public static Vector2 pixelPosition = Vector2.zero;
    // Ordinates between 0 and 1 ((0, 0) = bottom left), default is (0, 0)
    public static Vector2 screenPosition = Vector2.zero;
    // Ordinates between (-1, -1) and (1, 1). default is (0, 0)
    public static Vector2 centeredScreenPosition = Vector2.zero;

    public static bool touched = false;
    public static InputMode inputMode;

    public enum InputMode
    {
        Mouse,
        Touch,
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touched = true;
            inputMode = InputMode.Touch;
            Touch touch = Input.GetTouch(0);
            pixelPosition = touch.position;
            screenPosition = new Vector2(
                Mathf.Min(Mathf.Max(0, pixelPosition.x / Screen.width), 1),
                Mathf.Min(Mathf.Max(0, pixelPosition.y / Screen.height), 1)
            );
            centeredScreenPosition = screenPosition * 2 - Vector2.one;
        }
        else if (Input.mousePresent && Input.GetMouseButton(0))
        {
            touched = true;
            inputMode = InputMode.Mouse;
            pixelPosition = Input.mousePosition;
            screenPosition = new Vector2(
                Mathf.Min(Mathf.Max(0, pixelPosition.x / Screen.width), 1),
                Mathf.Min(Mathf.Max(0, pixelPosition.y / Screen.height), 1)
            );
            centeredScreenPosition = screenPosition * 2 - Vector2.one;
        } else
        {
            touched = false;
            screenPosition = Vector2.zero;
            pixelPosition = Vector2.zero;
            centeredScreenPosition = Vector2.zero;
        }
    }
}
