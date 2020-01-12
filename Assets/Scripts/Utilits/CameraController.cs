using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 10f;
    public float panBorderThickness = 10f;
    public float ScrollSpeed = 10f;


    public float MinZoomHeight = 2f;
    public float MaxZoomHeight = 10f;

    public float MinHeight = -5f;
    public float MaxHeight = 10f;

    public float MinWidth = 5f;
    public float MaxWidth = 10f;

    private bool canMove = true;

    void Start()
    {
        transform.rotation = Quaternion.Euler(45f, 0f, 0f);
        transform.position = new Vector3(0f, 10f, 0f);
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Escape))
            canMove = !canMove;

        if (!canMove)
            return;


        if (Input.GetKey("w") || Input.mousePosition.y >=  Screen.height - panBorderThickness)
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            //  new Vector3(0f,-1f,0f) 
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            //  new Vector3(1f,0f,0f) 
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            //  new Vector3(-1f,0f,0f) 
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }

        float scrollVal = Input.GetAxis("Mouse ScrollWheel");
        Vector3 camPos = transform.position;
        camPos.y -= scrollVal * 100 * ScrollSpeed * Time.deltaTime;

        camPos.x = Mathf.Clamp(camPos.x, MinWidth, MaxWidth);
        camPos.y = Mathf.Clamp(camPos.y, MinZoomHeight, MaxZoomHeight);
        camPos.z = Mathf.Clamp(camPos.z, MinHeight, MaxHeight);

        transform.position = camPos;
    }
}
