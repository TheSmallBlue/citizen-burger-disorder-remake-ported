using System.Collections.Generic;
using UnityEngine;

public class FloorTrigger : MonoBehaviour
{
    int maxRatSpawns = 1;

    public static int currentRats;

    private Object ratPrefab;

    private Transform ratSpawn;

    private float ratSpawnStartTimer;

    private float ratTimerUntilSpawn = 30f;

    //private float ratSpawnCooldown = 10f;

    private float ratSpawnCooldownStart;

    public static List<GameObject> foodDropPosition = new List<GameObject>();

    private void Start()
    {
        ratSpawn = GameObject.Find("!RatGraph").transform.Find("RATSPAWN");
        ratPrefab = Resources.Load("Prefabs/NPC/rat");
    }

    private void Update()
    {
        if (Time.time > ratSpawnStartTimer + ratTimerUntilSpawn && foodDropPosition.Count > 0)
        {
            for (int i = 0; i < Mathf.Min(foodDropPosition.Count, 3); i++)
            {
                if (currentRats > maxRatSpawns)
                {
                    break;
                }
                Vector3 position;
                if (foodDropPosition.Count == 0)
                {
                    position = ratSpawn.transform.position;
                }
                else
                {
                    int index = Random.Range(0, foodDropPosition.Count - 1);
                    position = foodDropPosition[index].transform.position;
                }
                Rat component = (Instantiate(ratPrefab, ratSpawn.transform.position, Quaternion.identity) as GameObject).GetComponent<Rat>();
                SetRatTarget(component.gameObject, position);
                currentRats++;
            }
            if (Random.value > 0.1f)
            {
                ratSpawnCooldownStart = Time.time;
            }
        }
        else if (foodDropPosition.Count == 0)
        {
            ratSpawnStartTimer = Time.time;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Food>() && !other.name.Contains("rat") && !foodDropPosition.Contains(other.gameObject))
        {
            other.GetComponent<Food>().foodBeenOnFloor = true;
            foodDropPosition.Add(other.gameObject);
            ratSpawnStartTimer = Time.time;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Food>() && !other.name.Contains("rat") && foodDropPosition.Contains(other.gameObject))
        {
            foodDropPosition.Remove(other.gameObject);
        }
    }

    void SetRatTarget(GameObject rat, Vector3 target)
    {
        Rat component = rat.GetComponent<Rat>();
        component.SetTargetFood(target);
    }
}
