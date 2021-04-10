using System;
using UnityEngine;

public class SpawnNPC : MonoBehaviour
{
    public static int currentNPCs;

    private int maxNPCs = 20;

    //private int maxRestaurantNPCs = 10;

    private float spawnRadius = 6f;

    private float spawnMinDelay = 100f;

    private float currentSpawnMinDelay;

    private static float spawnAdditionalTimeRand;

    private float spawnMaxRand = 40f;

    private float[] timeOfDaySpawnMultipliers = new float[]
    {
        0.5f,
        1f,
        1.2f,
        0.5f,
        2f,
        0.9f,
        1.5f,
        1f
    };

    private static float lastSpawnTime;

    private float citizenSpawnDelay = 20f;

    private float lastCitizenSpawn;

    private void Start()
    {
        spawnAdditionalTimeRand = 10f;
        currentSpawnMinDelay = spawnMinDelay;
        lastSpawnTime = Time.time - currentSpawnMinDelay;
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 12;
        GUI.Label(new Rect(0f, 20f, 500f, 20f), "Spawn: " + Mathf.Round(lastSpawnTime + currentSpawnMinDelay + spawnAdditionalTimeRand - Time.time));
    }

    private void Update()
    {
        if (true)
        {
            if (TimeOfDay.currentTimeOfDay < TimeOfDay.endTimeOfDay)
            {
                int num = Mathf.RoundToInt((TimeOfDay.currentTimeOfDay - TimeOfDay.startTimeOfDay) / (TimeOfDay.endTimeOfDay - TimeOfDay.startTimeOfDay) * (float)(timeOfDaySpawnMultipliers.Length - 1));
                currentSpawnMinDelay = spawnMinDelay * (1f / (timeOfDaySpawnMultipliers[/*num*/Mathf.Clamp(num, 0, timeOfDaySpawnMultipliers.Length)] + 0.001f)) / (float)Mathf.Max(1 + 1, 3);
            }
            if (Time.time > lastCitizenSpawn + citizenSpawnDelay)
            {
                lastCitizenSpawn = Time.time;
                if (UnityEngine.Random.value > 0.999f)
                {
                    SpawnNewNPC(0, 1, 1);
                }
                else
                {
                    SpawnNewNPC(0, 1, 0);
                }
            }
            if (Time.time > lastSpawnTime + (currentSpawnMinDelay + spawnAdditionalTimeRand))
            {
                lastSpawnTime = Time.time;
                spawnAdditionalTimeRand = UnityEngine.Random.Range(0f, spawnMaxRand + 1f);
                if (currentNPCs < maxNPCs)
                {
                    int num2 = 1;
                    int npcWants = 1;
                    float value = UnityEngine.Random.value;
                    if (value > 0.9f)
                    {
                        num2 = 3;
                    }
                    else if (value > 0.7f)
                    {
                        num2 = 4;
                    }
                    else if (value > 0.2f)
                    {
                        num2 = 2;
                    }
                    if (TableGraph.FindUnoccupiedTableForGroup(num2) == -1)
                    {
                        npcWants = 0;
                    }
                    for (int i = 0; i < num2; i++)
                    {
                        SpawnNewNPC(npcWants, num2, 0);
                    }
                }
            }
        }
    }

    private GameObject SpawnNewNPC(int npcWants = 0, int groupSize = 1, int easterEgg = 0)
    {
        Vector3 position = transform.position + UnityEngine.Random.insideUnitSphere * spawnRadius;
        position.y = transform.position.y;
        GameObject gameObject = Instantiate(Resources.Load("Prefabs/NPC/NPC"), position, transform.rotation) as GameObject;
        gameObject.GetComponent<NPC>().SetNPCTexture(gameObject, UnityEngine.Random.Range(1, 7) + string.Empty);
        if (easterEgg == 1)
        {
            string[] array = new string[]
            {
                "jorji",
                "cookServe"
            };
            gameObject.GetComponent<NPC>().SetNPCTexture(gameObject, array[UnityEngine.Random.Range(0, array.Length)]);
        }
        gameObject.GetComponent<NPC>().setWants(npcWants);
        gameObject.GetComponent<NPC>().setGroupSize(gameObject, groupSize);
        currentNPCs++;
        return gameObject;
    }
}
