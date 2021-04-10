using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(NetworkView))]
public class Flamable : MonoBehaviour
{
    private void Start()
    {
        fireWatch = GameObject.Find("!FireWatch").GetComponent<FireWatch>();
        if (GetComponent<Food>())
        {
            food = GetComponent<Food>();
        }
        if (GetComponent<PickupObject>())
        {
            pickup = GetComponent<PickupObject>();
        }
        firePrefab = (Resources.Load("Prefabs/Fire/Fire") as GameObject);
        if (food)
        {
            startTemp = food.foodTemp;
        }
        currentTemp = startTemp;
        currentBurnHealth = burnHealth;
        currentFireCheckRate = maxFireCheckRate;
    }

    private void Update()
    {
        /*if (Network.isServer)
        {
            if (food)
            {
                currentTemp = food.foodTemp;
            }
            if (!isOnFire)
            {
                if (wasOnFire && reflamable && !isFlamableAgain)
                {
                    currentBurnHealth = Mathf.Lerp(currentBurnHealth, burnHealth, Time.deltaTime * 0.5f);
                    if (currentBurnHealth == burnHealth)
                    {
                        isFlamableAgain = true;
                    }
                }
                if (!wasOnFire || (reflamable && isFlamableAgain))
                {
                    if (currentTemp > tempUntilIgniteFire)
                    {
                        FireIgnite();
                    }
                    if (currentBurnHealth < 0f)
                    {
                        FireIgnite();
                    }
                }
            }
            if ((isOnFire && currentBurnHealth <= burnoutAtHealth) || currentTemp < startTemp)
            {
                FireBurnOut(false);
            }
            if (isOnFire)
            {
                currentBurnHealth -= Time.deltaTime;
                if (food != null)
                {
                    food.cook();
                }
            }
        }*/
        
        if (food)
        {
            currentTemp = food.foodTemp;
        }
        if (!isOnFire)
        {
            if (wasOnFire && reflamable && !isFlamableAgain)
            {
                currentBurnHealth = Mathf.Lerp(currentBurnHealth, burnHealth, Time.deltaTime * 0.5f);
                if (currentBurnHealth == burnHealth)
                {
                    isFlamableAgain = true;
                }
            }
            if (!wasOnFire || (reflamable && isFlamableAgain))
            {
                if (currentTemp > tempUntilIgniteFire)
                {
                    FireIgnite();
                }
                if (currentBurnHealth < 0f)
                {
                    FireIgnite();
                }
            }
        }
        /*if ((isOnFire && currentBurnHealth <= burnoutAtHealth) || currentTemp < startTemp)
        {
            FireBurnOut(false);
        }*/
        if (isOnFire)
        {
            currentBurnHealth -= Time.deltaTime;
            if (food != null)
            {
                food.cook();
            }
        }
    }

    private IEnumerator FireDetect()
    {
        while (isOnFire)
        {
            if (lastCheckLocation == Vector3.zero || (lastCheckLocation - transform.position).magnitude > fireSpreadRadius)
            {
                NearFlamables.Clear();
                proximityFlamables = Physics.OverlapSphere(transform.position, fireSpreadRadius);
                foreach (Collider c in proximityFlamables)
                {
                    if (c.GetComponent<Flamable>())
                    {
                        NearFlamables.Add(c.GetComponent<Flamable>());
                    }
                }
            }
            yield return new WaitForSeconds(0.4f);
        }
        yield break;
    }

    private IEnumerator FireSpread()
    {
        while (isOnFire)
        {
            Vector3 avgPos = Vector3.zero;
            int localFireCount = 0;
            for (int i = 0; i < NearFlamables.Count; i++)
            {
                if (NearFlamables[i].isOnFire && NearFlamables[i] != this)
                {
                    localFireCount++;
                }
                else if (!NearFlamables[i].isOnFire && NearFlamables[i] != this)
                {
                    NearFlamables[i].currentBurnHealth -= 1f;
                }
                avgPos += NearFlamables[i].transform.position;
            }
            avgPos /= NearFlamables.Count;
            Debug.DrawLine(transform.position, avgPos, Color.green, 2f);
            if (!nearBigFire && localFireCount > 3 && !CheckNearBigFire())
            {
                int layermask = ~(1 << LayerMask.NameToLayer("Food") | 1 << LayerMask.NameToLayer("Fire"));
                RaycastHit hit;
                if (Physics.Raycast(avgPos + Vector3.up, Vector3.down, out hit, 2f, layermask))
                {
                    print("Hit: " + hit.transform.name);
                    /*if (Network.isServer)
                    {
                        fireWatch.networkView.RPC("CreateBigFireAnimate", 2, new object[]
                        {
                            hit.point + new Vector3(0f, Random.Range(-0.5f, 0.5f) + 1f, 0f),
                            transform.rotation,
                            Network.AllocateViewID()
                        });
                    }*/
                }
            }
            yield return new WaitForSeconds(Random.Range(0.8f, 1.2f));
        }
        yield break;
    }

    private IEnumerator LoopCheckNearBigFire()
    {
        while (isOnFire)
        {
            CheckNearBigFire();
            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
        yield break;
    }

    private bool CheckNearBigFire()
    {
        if (!nearBigFire || (nearBigFire && (lastCheckLocation - transform.position).magnitude > fireSpreadRadius))
        {
            proximityBigFires = Physics.OverlapSphere(transform.position, fireSpreadRadius * 1f);
            bool flag = false;
            foreach (Collider collider in proximityBigFires)
            {
                if (collider.GetComponent<FireAnimate>() && collider.GetComponent<FireAnimate>().isLargeFire)
                {
                    flag = true;
                }
            }
            nearBigFire = flag;
        }
        return nearBigFire;
    }

    public void FireIgnite()
    {
        /*if (Network.isServer)
        {
            isOnFire = true;
            StartCoroutine(FireSpread());
            StartCoroutine(FireDetect());
            StartCoroutine(LoopCheckNearBigFire());
            fireWatch.networkView.RPC("CreateFireAnimate", 2, new object[]
            {
                transform.position + fireOffsetLocaiton,
                transform.rotation,
                false,
                networkView.viewID,
                Network.AllocateViewID()
            });
            currentFireCheckRate = minFireCheckRate;
            AllFires.Add(this);
            fireWatch.networkView.RPC("SyncAllFlamable", 1, new object[]
            {
                networkView.viewID,
                isOnFire,
                wasOnFire,
                isFlamableAgain,
                reflamable,
                currentBurnHealth,
                currentTemp,
                nearBigFire,
                currentFireCheckRate
            });
        }*/

        isOnFire = true;
        StartCoroutine(FireSpread());
        StartCoroutine(FireDetect());
        StartCoroutine(LoopCheckNearBigFire());
        fireWatch.CreateFireAnimate(transform.position + fireOffsetLocaiton, transform.rotation, false, gameObject);
        currentFireCheckRate = minFireCheckRate;
        AllFires.Add(this);
        fireWatch.SyncAllFlamable(gameObject, isOnFire, wasOnFire, isFlamableAgain, reflamable, currentBurnHealth, currentTemp, nearBigFire, currentFireCheckRate);
    }

    public void FireBurnOut(bool resetBurnTemp = false)
    {
        /*if (Network.isServer)
        {
            isOnFire = false;
            wasOnFire = true;
            isFlamableAgain = false;
            StopCoroutine(FireSpread());
            StopCoroutine(FireDetect());
            StopCoroutine(LoopCheckNearBigFire());
            if (fireAnimate)
            {
                fireAnimate.PutOut(true);
            }
            Destroy(FireGameObject);
            FireGameObject = null;
            currentFireCheckRate = maxFireCheckRate;
            if (resetBurnTemp)
            {
                currentBurnHealth = burnHealth;
                currentTemp = startTemp;
                if (food != null)
                {
                    food.foodTemp = startTemp;
                    food.CallSyncFood();
                }
            }
            AllFires.Remove(this);
            fireWatch.networkView.RPC("SyncAllFlamable", 1, new object[]
            {
                networkView.viewID,
                isOnFire,
                wasOnFire,
                isFlamableAgain,
                reflamable,
                currentBurnHealth,
                currentTemp,
                nearBigFire,
                currentFireCheckRate
            });
        }*/

        isOnFire = false;
        wasOnFire = true;
        isFlamableAgain = false;
        StopCoroutine(FireSpread());
        StopCoroutine(FireDetect());
        StopCoroutine(LoopCheckNearBigFire());
        if (fireAnimate)
        {
            fireAnimate.PutOut(true);
        }
        Destroy(FireGameObject);
        FireGameObject = null;
        currentFireCheckRate = maxFireCheckRate;
        if (resetBurnTemp)
        {
            currentBurnHealth = burnHealth;
            currentTemp = startTemp;
            if (food != null)
            {
                food.foodTemp = startTemp;
            }
        }
        AllFires.Remove(this);
        /*fireWatch.networkView.RPC("SyncAllFlamable", 1, new object[]
        {
                isOnFire,
                wasOnFire,
                isFlamableAgain,
                reflamable,
                currentBurnHealth,
                currentTemp,
                nearBigFire,
                currentFireCheckRate
        });*/
    }

    public static List<Flamable> AllFires = new List<Flamable>();

    public List<Flamable> NearFlamables = new List<Flamable>();

    private Collider[] proximityBigFires;

    private Collider[] proximityFlamables;

    Vector3 lastCheckLocation = Vector3.zero;

    public bool isOnFire;

    public Vector3 fireOffsetLocaiton = Vector3.zero;

    public bool wasOnFire;

    public bool isFlamableAgain = true;

    public bool reflamable;

    public float burnHealth = 20f;

    public float burnoutAtHealth = -300f;

    public float currentBurnHealth;

    public float tempUntilIgniteFire = 100f;

    public float startTemp = 10f;

    public float currentTemp;

    private float fireSpreadRadius = 3f;

    private Food food;

    public PickupObject pickup;

    private GameObject FireGameObject;

    public FireAnimate fireAnimate;

    private static GameObject firePrefab;

    public static FireWatch fireWatch;

    public bool nearBigFire;

    public float currentFireCheckRate;

    public float minFireCheckRate = 0.9f;

    public float maxFireCheckRate = 2f;
}
