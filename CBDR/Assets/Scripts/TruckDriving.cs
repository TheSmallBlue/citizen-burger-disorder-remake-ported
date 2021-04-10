using System.Collections.Generic;
using UnityEngine;

public class TruckDriving : MonoBehaviour
{
    private void Start()
    {
        truckContents = transform.Find("TruckContentsTrigger").GetComponent<TruckContents>();
        FindGoals();
        goalStartPos = transform.position;
        goalStartRot = transform.rotation.eulerAngles;
        currentGoal = truckGoals[currentIndex];
    }

    private void Update()
    {
        if (!reachedSTOP || (reachedSTOP && elapsedWaitTime >= waitTimeDuration))
        {
            if (currentGoal.name.Contains("STOP"))
            {
                TravelToGoal(3, 3);
            }
            else
            {
                TravelToGoal(0, 3);
            }
        }
        if (elapsedTime >= travelTimeBetweenNodes)
        {
            if (currentGoal.name.Contains("STOP"))
            {
                if (elapsedWaitTime >= waitTimeDuration)
                {
                    truckContents.DestroyBoxesInsideTruck();
                    travelTimeBetweenNodes = 4f;
                    currentIndex = (currentIndex + 1) % truckGoals.Count;
                    currentGoal = truckGoals[currentIndex];
                    goalStartPos = transform.position;
                    goalStartRot = transform.rotation.eulerAngles;
                    elapsedTime = 0f;
                }
                if (!reachedSTOP)
                {
                    reachedSTOP = true;
                    SpawnBoxesInsideTruck();
                }
                elapsedWaitTime += Time.deltaTime;
            }
            else if (currentGoal.name.Contains("END"))
            {
                /*if (Network.isServer)
                {
                    Network.RemoveRPCs(gameObject.networkView.viewID);
                    Network.Destroy(gameObject);
                }*/
                Destroy(gameObject);
            }
            else
            {
                currentIndex = (currentIndex + 1) % truckGoals.Count;
                currentGoal = truckGoals[currentIndex];
                goalStartPos = transform.position;
                goalStartRot = transform.rotation.eulerAngles;
                elapsedTime = 0f;
            }
        }
        elapsedTime += Time.deltaTime;
    }

    private void SpawnBoxesInsideTruck()
    {
        Vector3 vector = transform.position + transform.up * 7.5f;
        int num = Random.Range(4, 9);
        int num2 = 0;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    if (num2 >= num || Random.value <= 0.4f)
                    {
                        break;
                    }
                    Vector3 vector2 = vector + transform.right * 2.5f * j + (-transform.forward * 2.5f + transform.forward * 5f * i + transform.up * 2.5f * k);
                    GameObject gameObject = Instantiate(boxPrefab, vector2, transform.rotation * Quaternion.Euler(0f, Random.Range(-20f, 20f), 0f));
                    Box component = gameObject.GetComponent<Box>();
                    component.SyncContents(contents);
                    num2++;
                }
            }
        }
    }

    private void FindGoals()
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag("TruckGoal");
        truckGoals.Clear();
        for (int i = 1; i < array.Length; i++)
        {
            GameObject gameObject = array[i];
            int num = i;
            while (num > 0 && GetDistance(array[num - 1]) > GetDistance(gameObject))
            {
                array[num] = array[num - 1];
                num--;
            }
            array[num] = gameObject;
        }
        for (int i = 0; i < array.Length; i++)
        {
            truckGoals.Add(array[i]);
        }
    }

    private float GetDistance(GameObject to)
    {
        return (transform.position - to.transform.position).magnitude;
    }

    private void Boxes()
    {
        int num = Random.Range(1, 3);
        print(num);
        for (int i = 0; i < num; i++)
        {
            GameObject gameObject = Instantiate(boxPrefab, transform.position - transform.forward * 1.6f + transform.up * 10f + Random.insideUnitSphere * 2f, transform.rotation * Quaternion.Euler(0f, 270f, 0f));
            gameObject.GetComponent<Rigidbody>().AddForce(-transform.forward * 300f + transform.up * 300f + Random.insideUnitSphere * 30f);
        }
    }

    private void TravelToGoal(int posLerpStyle = 0, int rotLerpStyle = 0)
    {
        Vector3 zero = Vector3.zero;
        Vector3 zero2 = Vector3.zero;
        Vector3 position = currentGoal.transform.position;
        Vector3 eulerAngles = currentGoal.transform.rotation.eulerAngles;
        switch (posLerpStyle)
        {
            case 1:
                zero.x = Sinerp(goalStartPos.x, position.x, elapsedTime / travelTimeBetweenNodes);
                zero.y = Sinerp(goalStartPos.y, position.y, elapsedTime / travelTimeBetweenNodes);
                zero.z = Sinerp(goalStartPos.z, position.z, elapsedTime / travelTimeBetweenNodes);
                zero2.x = Sinerp(goalStartRot.x, eulerAngles.x, elapsedTime / travelTimeBetweenNodes);
                zero2.y = Sinerp(goalStartRot.y, eulerAngles.y, elapsedTime / travelTimeBetweenNodes);
                zero2.z = Sinerp(goalStartRot.z, eulerAngles.z, elapsedTime / travelTimeBetweenNodes);
                break;
            case 2:
                zero.x = Coserp(goalStartPos.x, position.x, elapsedTime / travelTimeBetweenNodes);
                zero.y = Coserp(goalStartPos.y, position.y, elapsedTime / travelTimeBetweenNodes);
                zero.z = Coserp(goalStartPos.z, position.z, elapsedTime / travelTimeBetweenNodes);
                zero2.x = Coserp(goalStartRot.x, eulerAngles.x, elapsedTime / travelTimeBetweenNodes);
                zero2.y = Coserp(goalStartRot.y, eulerAngles.y, elapsedTime / travelTimeBetweenNodes);
                zero2.z = Coserp(goalStartRot.z, eulerAngles.z, elapsedTime / travelTimeBetweenNodes);
                break;
            case 3:
                zero.x = Berp(goalStartPos.x, position.x, elapsedTime / travelTimeBetweenNodes);
                zero.y = Berp(goalStartPos.y, position.y, elapsedTime / travelTimeBetweenNodes);
                zero.z = Berp(goalStartPos.z, position.z, elapsedTime / travelTimeBetweenNodes);
                zero2.x = Berp(goalStartRot.x, eulerAngles.x, elapsedTime / travelTimeBetweenNodes);
                zero2.y = Berp(goalStartRot.y, eulerAngles.y, elapsedTime / travelTimeBetweenNodes);
                zero2.z = Berp(goalStartRot.z, eulerAngles.z, elapsedTime / travelTimeBetweenNodes);
                break;
            default:
                zero.x = Mathf.Lerp(goalStartPos.x, position.x, elapsedTime / travelTimeBetweenNodes);
                zero.y = Mathf.Lerp(goalStartPos.y, position.y, elapsedTime / travelTimeBetweenNodes);
                zero.z = Mathf.Lerp(goalStartPos.z, position.z, elapsedTime / travelTimeBetweenNodes);
                zero2.x = Mathf.Lerp(goalStartRot.x, eulerAngles.x, elapsedTime / travelTimeBetweenNodes);
                zero2.y = Mathf.Lerp(goalStartRot.y, eulerAngles.y, elapsedTime / travelTimeBetweenNodes);
                zero2.z = Mathf.Lerp(goalStartRot.z, eulerAngles.z, elapsedTime / travelTimeBetweenNodes);
                break;
        }
        transform.position = zero;
        transform.rotation = Quaternion.Euler(zero2);
    }

    private float Berp(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);
        value = (Mathf.Sin(value * 3.14159274f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
        return start + (end - start) * value;
    }

    private float Sinerp(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, Mathf.Sin(value * 3.14159274f * 0.5f));
    }

    private float Coserp(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, 1f - Mathf.Cos(value * 3.14159274f * 0.5f));
    }

    private TruckContents truckContents;

    public List<GameObject> truckGoals = new List<GameObject>();

    private GameObject currentGoal;

    private int currentIndex;

    public bool reachedSTOP;

    private Vector3 goalStartPos;

    private Vector3 goalStartRot;

    private float travelTimeBetweenNodes = 1.75f;

    private float waitTimeDuration = 30f;

    private float elapsedWaitTime;

    private float elapsedTime;

    public GameObject boxPrefab;

    public int contents;
}
