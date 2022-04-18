using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightManager : MonoBehaviour
{
    public static DayNightManager Instance;

    public Light GameLightSource;
    public string NightTheme;
    public string MainTheme;
    public int TicksRed = 3;
    public int TicksGreen = 10;
    public int TicksBlue = 5;

    public float percentComplete;

    private float lastRed = 255f;
    private float lastGreen = 255f;
    private float lastBlue = 255f;

    public float TimeBetweenTicks = .2f;
    public float countdown = .2f;

    public bool IsDay = true;

    void Awake()
    {
        if (Instance != null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Multi BuildManager");
            return;
        }
        Instance = this;
    }

    public void SetNextInterval(float percent)
    {
        AudioManager.Instance.Stop(MainTheme);
        AudioManager.Instance.Play(NightTheme);
        percentComplete =  Mathf.Clamp(percent,0,1);
        IsDay = false;
    }

    public void SetDay()
    {
        AudioManager.Instance.Stop(NightTheme);
        AudioManager.Instance.Play(MainTheme);
        percentComplete = 0;
        IsDay = true;
    }

    private void Update()
    {
        if (IsDay)
        {
            GameLightSource.color = new Color(1f, 1f, 1f);
            return;
        }

        if (((255 - lastRed) / TicksRed) / (255 / TicksRed) >= percentComplete)
        {
            return;
        }

        if (countdown >= 0f)
        {
            countdown -= Time.deltaTime;
            return;
        }
        countdown = TimeBetweenTicks;


        lastRed = (lastRed - TicksRed);
        lastGreen = (lastGreen - TicksGreen);
        lastBlue = (lastBlue - TicksBlue);

        GameLightSource.color = new Color(Mathf.Clamp(lastRed/255,0,1), Mathf.Clamp(lastGreen /255, 0, 1), Mathf.Clamp(lastBlue /255,0,1));
    }

}
