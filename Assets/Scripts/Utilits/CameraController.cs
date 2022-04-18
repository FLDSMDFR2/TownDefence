using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#region KeyCodes Enums
public enum InputKeysNames
{
    FORWARD,
    BACK,
    LEFT,
    RIGHT,
    ROTATE_LEFT,
    ROTAE_RIGHT,
    ZOOM_IN,
    ZOOM_OUT,
    SPEED_BOOST,
    ESCAPE_MOVEMENT
}

public enum InputMouseNames
{
    PAN,
    ROTATE,
    ZOOM
}
#endregion

#region InputControls
public class InputControls
{
    private static Dictionary<InputKeysNames, KeyCode> inputKeys = new Dictionary<InputKeysNames, KeyCode>();
    private static Dictionary<InputMouseNames, int> inputMouse = new Dictionary<InputMouseNames, int>();

    private static bool init = false;

    private static void InitValues()
    {
        inputKeys.Add(InputKeysNames.FORWARD, KeyCode.W);
        inputKeys.Add(InputKeysNames.BACK, KeyCode.S);
        inputKeys.Add(InputKeysNames.LEFT, KeyCode.A);
        inputKeys.Add(InputKeysNames.RIGHT, KeyCode.D);

        inputKeys.Add(InputKeysNames.ROTATE_LEFT, KeyCode.Q);
        inputKeys.Add(InputKeysNames.ROTAE_RIGHT, KeyCode.E);

        inputKeys.Add(InputKeysNames.ZOOM_IN, KeyCode.R);
        inputKeys.Add(InputKeysNames.ZOOM_OUT, KeyCode.F);

        inputKeys.Add(InputKeysNames.SPEED_BOOST, KeyCode.LeftShift);
        inputKeys.Add(InputKeysNames.ESCAPE_MOVEMENT, KeyCode.Escape);

        // MOUSE CODES
        inputMouse.Add(InputMouseNames.PAN, 0);
        inputMouse.Add(InputMouseNames.ROTATE, 2);
        //inputMouse.Add(InputMouseNames.ZOOM, 1);

        init = true;
    }

    public static KeyCode GetInputKey(InputKeysNames name)
    {
        if (!init)
            InitValues();

        return inputKeys[name];
    }

    public static int GetMouseCodes(InputMouseNames name)
    {
        if (!init)
            InitValues();

        return inputMouse[name];
    }

}
#endregion

public class CameraController : MonoBehaviour
{
    public Camera Camera;
    public Transform ZoomTransform;

    [Header("Movement")]
    public float NoramlSpeed;
    public float FastSpeed;
    public float MovementTime;
    private float movementSpeed;

    public Vector3 newPosition;

    [Header("Rotation")]
    public float RotationAmount;

    private Quaternion newRotation;

    [Header("Zoom")]
    public Vector3 ZoomAmount;
    public float ZoomPerccent;
    private Quaternion cameraZoomAngle;
    public Vector3 newZoom;

    [Header("Mouse Inputs")]
    public Vector3 MouseZoomAmount;

    private Vector3 dragStartPositon;
    private Vector3 dragCurrentPositon;

    private Vector3 rotateStartPositon;
    private Vector3 rotateCurrentPositon;

    [Header("MinMax")]
    public float MinZoomRotation;
    public float MaxZoomRotation;

    public float MinZoomDistance;
    public float MaxZoomDistance;

    public float MinHeight;
    public float MaxHeight;

    public float MinWidth;
    public float MaxWidth;

    public float MinZoomHeight;
    public float MaxZoomHeight;

    public float MinZoomWidth;
    public float MaxZoomWidth;

    private bool canMove = true;

    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = ZoomTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleKeyboardInput();
        HandleMouseInput();
        HandleTouchScreenInput();

        MoveToNewPosition();
    }

    #region "Keyboard Input"
    private void HandleKeyboardInput()
    {
        if (Input.GetKey(InputControls.GetInputKey(InputKeysNames.ESCAPE_MOVEMENT)))
            canMove = !canMove;

        if (!canMove)
            return;

        // Movement
        if (Input.GetKey(InputControls.GetInputKey(InputKeysNames.SPEED_BOOST)))
        {
            movementSpeed = FastSpeed;
        }
        else
        {
            movementSpeed = NoramlSpeed;
        }


        if (Input.GetKey(InputControls.GetInputKey(InputKeysNames.FORWARD)))
        {
            SetNewPosition(transform.forward * movementSpeed, true);
        }

        if (Input.GetKey(InputControls.GetInputKey(InputKeysNames.BACK)))
        {
            SetNewPosition(transform.forward * -movementSpeed, true);
        }

        if (Input.GetKey(InputControls.GetInputKey(InputKeysNames.RIGHT)))
        {
            SetNewPosition(transform.right * movementSpeed, true);
        }

        if (Input.GetKey(InputControls.GetInputKey(InputKeysNames.LEFT)))
        {
            SetNewPosition(transform.right * -movementSpeed, true);
        }

        // ROTAION
        if (Input.GetKey(InputControls.GetInputKey(InputKeysNames.ROTATE_LEFT)))
        {
            newRotation *= Quaternion.Euler(Vector3.up * RotationAmount);
        }

        if (Input.GetKey(InputControls.GetInputKey(InputKeysNames.ROTAE_RIGHT)))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -RotationAmount);
        }

        // ZOOM
        if (Input.GetKey(InputControls.GetInputKey(InputKeysNames.ZOOM_IN)))
        {
            SetNewZoom(ZoomAmount, true);
        }
        if (Input.GetKey(InputControls.GetInputKey(InputKeysNames.ZOOM_OUT)))
        {
            SetNewZoom(-ZoomAmount, true);
        }

    }
    #endregion

    #region "Mouse Input"
    private void HandleMouseInput()
    {

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetKey(InputControls.GetInputKey(InputKeysNames.ESCAPE_MOVEMENT)))
            canMove = !canMove;

        if (!canMove)
            return;

        //  ZOOM
        if (Input.mouseScrollDelta.y != 0)
        {
            SetNewZoom(Input.mouseScrollDelta.y * MouseZoomAmount, true);
        }

        // PAN
        if (Input.GetMouseButtonDown(InputControls.GetMouseCodes(InputMouseNames.PAN)))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragStartPositon = ray.GetPoint(entry);
            }
        }
        else if (Input.GetMouseButton(InputControls.GetMouseCodes(InputMouseNames.PAN)))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPositon = ray.GetPoint(entry);

                SetNewPosition(transform.position + (dragStartPositon - dragCurrentPositon), false);
            }
        }

        // ROTATE
        if (Input.GetMouseButtonDown(InputControls.GetMouseCodes(InputMouseNames.ROTATE)))
        {
           rotateStartPositon = Input.mousePosition;
        }
        else if (Input.GetMouseButton(InputControls.GetMouseCodes(InputMouseNames.ROTATE)))
        {
            rotateCurrentPositon = Input.mousePosition;
            Vector3 diff = rotateStartPositon - rotateCurrentPositon;
            rotateStartPositon = rotateCurrentPositon;
            newRotation *= Quaternion.Euler(Vector3.up * (-diff.x / 5f));
        }

    }
    #endregion

    #region "TouchScreen Input"
    private void HandleTouchScreenInput()
    {

    }
    #endregion

    private void MoveToNewPosition()
    {
        GetCameraZoomUpdates();
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * MovementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * MovementTime);
        ZoomTransform.localPosition = Vector3.Lerp(ZoomTransform.localPosition, newZoom, Time.deltaTime * MovementTime);

        Camera.transform.localRotation = Quaternion.Lerp(Camera.transform.localRotation, cameraZoomAngle, Time.deltaTime * MovementTime);
    }

    private void GetCameraZoomUpdates()
    {
        ZoomPerccent =  Mathf.Abs((newZoom.y - MinZoomHeight) / (MaxZoomDistance - MinZoomHeight) - 1);

        cameraZoomAngle = Quaternion.Euler(Mathf.Clamp(-(ZoomPerccent * (MaxZoomRotation - MinZoomRotation)), MinZoomRotation, MaxZoomRotation),0f,0f);
    }

    private void SetNewPosition(Vector3 newpos, bool update)
    {

        if (update)
        {
            newPosition += newpos;
        }
        else
        {
            newPosition = newpos;
        }

        newPosition.x = Mathf.Clamp(newPosition.x, MinWidth, MaxWidth);
        //newPosition.y = Mathf.Clamp(newPosition.y, MinZoomHeight, MaxZoomHeight);
        newPosition.z = Mathf.Clamp(newPosition.z, MinHeight, MaxHeight);
    }
    private void SetNewZoom(Vector3 newZ, bool update)
    {

        if (update)
        {
            newZoom += newZ;
        }
        else
        {
            newZoom = newZ;
        }

        newZoom.x = Mathf.Clamp(newZoom.x, 0, 0);
        newZoom.y = Mathf.Clamp(newZoom.y, MinZoomDistance, MaxZoomDistance);
        newZoom.z = Mathf.Clamp(newZoom.z, -MaxZoomDistance ,-MinZoomDistance);
    }
}
