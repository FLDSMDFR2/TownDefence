using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectHealth : MonoBehaviour
{
    public Image HealthBarImage;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }

    public void UpdateHealthBar(float HealthPrecent)
    {
        if (HealthPrecent < .25f)
        {
            HealthBarImage.color = Color.red;
        }
        else if (HealthPrecent < .5f)
        {
            HealthBarImage.color = Color.yellow;
        }
        else
        {
            HealthBarImage.color = Color.green;
        }


        HealthBarImage.fillAmount = HealthPrecent;
    }
}
