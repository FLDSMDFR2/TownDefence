using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeed : MonoBehaviour
{
    public Text Speed;
    private int StartSpeed = 1;
    private int currentSpeed;
    private int maxSpeed = 4;

    private void Awake()
    {
        currentSpeed = StartSpeed;
        Time.timeScale = StartSpeed;
    }

    private void Start()
    {
        UIEvents.Instance.OnGameSpeedChange += UpdateSpeed;
    }

    public void UpdateSpeed()
    {
        int newSpeed = currentSpeed += 1;
        if (newSpeed > maxSpeed)
            newSpeed = StartSpeed;

        currentSpeed = newSpeed;
        Speed.text = "x" + newSpeed.ToString();
        Time.timeScale = newSpeed;
    }

    private void OnDestroy()
    {
        UIEvents.Instance.OnGameSpeedChange -= UpdateSpeed;
    }
}
