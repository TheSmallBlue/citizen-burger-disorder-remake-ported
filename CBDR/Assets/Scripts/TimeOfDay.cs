using UnityEngine;

public class TimeOfDay : MonoBehaviour
{
    private void Start()
    {
        dayColor = RenderSettings.ambientLight;
        currentColor = RenderSettings.ambientLight;
        nightColor = new Color(0.05f, 0.05f, 0.05f);
        dayStart = Time.time;
    }

    /*private void OnPlayerConnected(NetworkPlayer player)
    {
        networkView.RPC("SyncTimeOfDay", 1, new object[]
        {
            currentTimeOfDay,
            dayStart
        });
    }*/

    private void SyncTimeOfDay(float time, float timeStart)
    {
        print(string.Concat(new object[]
        {
            "Set time of day to: ",
            time,
            " and started at ",
            timeStart
        }));
        currentTimeOfDay = time;
        dayStart = timeStart;
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 12;
        GUI.Label(new Rect(0f, 0f, 500f, 20f), "Time of day: " + Mathf.Round(currentTimeOfDay * 10f) / 10f);
    }

    private void Update()
    {
        if (currentTimeOfDay > 12f)
        {
            RenderSettings.ambientLight = Color.Lerp(dayColor, nightColor, (currentTimeOfDay - 16f) / 3f);
        }
        else
        {
            RenderSettings.ambientLight = Color.Lerp(nightColor, dayColor, (currentTimeOfDay - 8f) / 2f);
        }
        currentTimeOfDay = startTimeOfDay + (Time.time - dayStart) / secondsInAnHour;
        if (currentTimeOfDay >= endTimeOfDay)
        {
            dayStart = Time.time;
            currentTimeOfDay = startTimeOfDay;
        }
    }

    public static float currentTimeOfDay;

    public static float dayStart;

    public static float startTimeOfDay = 8f;

    public static float endTimeOfDay = 22f;

    private static float secondsInAnHour = 60f;

    private Color dayColor;

    private Color nightColor;

    private Color currentColor;
}
