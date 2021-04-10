using System;
using UnityEngine;

public class Food : MonoBehaviour
{
    /*private void OnPlayerConnected(NetworkPlayer player)
    {
        networkView.RPC("SyncAllFood", player, new object[]
        {
            networkView.viewID,
            cooked,
            cookedDelay,
            overcooked,
            cookedRed,
            cookedBlue,
            cookedGreen,
            foodTemp,
            foodBeenOnFloor
        });
    }*/

    public void CallSyncFood()
    {
        SyncAllFood(cooked, cookedDelay, overcooked, cookedRed, cookedBlue, cookedGreen, foodTemp, foodBeenOnFloor);
    }

    //[RPC]
    private void SyncAllFood(/*NetworkViewID objectID, */float nCooked, float nCookedDelay, float nOvercooked, float nRed, float nBlue, float nGreen, float nFoodTemp, bool nFoodBeenOnFloor)
    {
        Food component;
        try
        {
            component = /*NetworkView.Find(objectID)*/gameObject.GetComponent<Food>();
        }
        catch (UnityException ex)
        {
            Debug.Log(ex);
            return;
        }
        component.cooked = nCooked;
        component.cookedDelay = nCookedDelay;
        component.overcooked = nOvercooked;
        component.foodTemp = nFoodTemp;
        component.foodBeenOnFloor = nFoodBeenOnFloor;
        component.cookedRed = nRed;
        component.cookedBlue = nBlue;
        component.cookedGreen = nGreen;
        component.UpdateMaterial(true);
    }

    private void Awake()
    {
        if (GetComponent<MeshRenderer>() != null && GetComponent<MeshRenderer>().material != null)
        {
            originalColor = GetComponent<MeshRenderer>().material.color;
            GetComponent<MeshRenderer>().material.color = Color.Lerp(originalColor, new Color(cookedRed, cookedGreen, cookedBlue), cooked * cookSpeedModifier * (1f - textureToKeep));
        }
        burgerStack = GetBurgerStack();
        startFoodTemp = foodTemp;
    }

    public BurgerStacking GetBurgerStack()
    {
        if (burgerStack == null)
        {
            if (type == FoodType.bun && transform.Find("burger-bottom").GetChild(0))
            {
                burgerStack = transform.Find("burger-bottom").Find("triggerBunStack").GetComponent<BurgerStacking>();
            }
            else if (type != FoodType.bun && inFood)
            {
                print("Set burger stacking for " + transform.name);
                burgerStack = transform.parent.Find("triggerBunStack").GetComponent<BurgerStacking>();
            }
        }
        return burgerStack;
    }

    public void Highlight(Color highlightColor)
    {
        if (GetComponent<MeshRenderer>() != null && GetComponent<MeshRenderer>().material != null)
        {
            GetComponent<MeshRenderer>().material.color = highlightColor;
        }
    }

    public void cook()
    {
        foodTemp += Time.deltaTime;
        UpdateMaterial(false);
    }

    private void UpdateMaterial(bool instant = false)
    {
        if (GetComponent<MeshRenderer>())
        {
            if (!instant)
            {
                if (cooked < 1f)
                {
                    if (!supportsTextureBlend)
                    {
                        GetComponent<MeshRenderer>().material.color = Color.Lerp(originalColor, new Color(cookedRed, cookedGreen, cookedBlue), cooked);
                    }
                    else
                    {
                        GetComponent<MeshRenderer>().material.SetFloat("_Blend", cooked);
                    }
                    cooked += Time.deltaTime / cookTimeIdeal;
                }
                else if (cookedDelay < 1f)
                {
                    cookedDelay += Time.deltaTime / cookTimeBurnDelay;
                }
                else if (overcooked < 1f)
                {
                    GetComponent<MeshRenderer>().material.color = Color.Lerp(GetComponent<MeshRenderer>().material.color, new Color(0.005f, 0f, 0f), overcooked * cookSpeedModifier / 80f);
                    overcooked += Time.deltaTime / cookTimeBurned;
                }
            }
            else
            {
                GetComponent<MeshRenderer>().material.color = Color.Lerp(originalColor, new Color(cookedRed, cookedGreen, cookedBlue), cooked);
                GetComponent<MeshRenderer>().material.color = Color.Lerp(GetComponent<MeshRenderer>().material.color, new Color(0.005f, 0f, 0f), overcooked * cookSpeedModifier / 80f);
            }
        }
    }

    public void MoveFoodFromBurger(/*NetworkViewID otherBurgerTransformID*/)
    {
        Transform transform = /*NetworkView.Find(otherBurgerTransformID).transform*/gameObject.transform;
        Transform transform2 = transform.transform.Find("burger-bottom").Find("triggerBunStack");
        burgerStack.foodOnBurger.AddRange(transform2.GetComponent<BurgerStacking>().foodOnBurger);
        burgerStack.transform.position = transform2.position;
        transform2.GetComponent<Collider>().enabled = false;
        transform2.GetComponent<BurgerStacking>().enabled = false;
        //transform2.GetComponent<BurgerStacking>().foodOnBurger.Clear();
    }

    public void AddFoodToBurger(Transform objectTransform)
    {
        Transform transform = objectTransform;
        Food component = transform.GetComponent<Food>();
        burgerStack.foodOnBurger.Add(component);
    }

    public void AddFoodToPlate(Transform foodTransform, Transform plateTransform)
    {
        Transform transform = foodTransform;
        Plate component = plateTransform.Find("triggerPlate").GetComponent<Plate>();
        component.foodOnPlate.Add(transform.GetComponent<Food>());
    }

    public void SetObjectPosition(Vector3 pos, Quaternion rot)
    {
        Transform transform = gameObject.transform;
        if (transform.name.Equals("PlateModel") || transform.name.Equals("burger-bottom"))
        {
            transform = transform.parent;
        }
        if (transform.GetComponent<Rigidbody>() && !transform.GetComponent<Rigidbody>().isKinematic)
        {
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        transform.position = pos;
        transform.rotation = rot;
    }

    /*[RPC]
    private void SetObservedToTransform(NetworkViewID id)
    {
        Transform transform = NetworkView.Find(id).transform;
        transform.GetComponent<NetworkView>().observed = transform.transform;
    }*/

    //[RPC]
    /*private void SetObservedToNetObj(NetworkViewID id)
    {
        Transform transform = NetworkView.Find(id).gameObject.transform;
        if (GetComponent<PickupObject>())
        {
            transform.GetComponent<PickupObject>().netObject.states = new NetworkObject.State[20];
            transform.GetComponent<NetworkView>().observed = transform.GetComponent<NetworkObject>();
        }
        else
        {
            transform.GetComponent<NetworkView>().observed = transform.GetComponent<NetworkObject>();
        }
    }*/

    //[RPC]
    private void SetCollider(bool state)
    {
        GetComponent<Collider>().enabled = state;
    }

    public void DestroyRigidbody()
    {
        /*Transform transform = gameObject.transform;
        Destroy(transform.GetComponent<Rigidbody>());*/
        Destroy(gameObject.GetComponent<Rigidbody>());
    }

    public void SetParent(Transform parent, string type = "")
    {
        /*if (type == "burger")
        {
            parent = transform.GetChild(0);
        }
        else
        {
            parent = transform;
        }*/
        transform.parent = parent;
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        //Debug.Log(transform.parent.name);
        //Debug.Log(transform.parent.childCount);
        if (transform.parent.childCount - 1 == 1) {
            if (snapsToCentreInBurger) {
                transform.localPosition = new Vector3(0, transform.parent.GetChild(0).localPosition.y + (gameObject.GetComponent<BoxCollider>().size.y / 2), 0);
            } else {
                transform.localPosition = new Vector3(
                    Mathf.Clamp(transform.localPosition.x, -0.3f, 0.3f), 
                    transform.parent.GetChild(0).localPosition.y + (gameObject.GetComponent<BoxCollider>().size.y / 2), 
                    Mathf.Clamp(transform.localPosition.z, -0.3f, 0.3f)
                    );
            }
            //transform.localPosition = new Vector3(transform.localPosition.x, transform.parent.GetChild(0).localPosition.y + (gameObject.GetComponent<BoxCollider>().size.y / 2), transform.localPosition.z);
        } else {
            if (snapsToCentreInBurger) {
                transform.localPosition = new Vector3(0, transform.parent.GetChild(transform.parent.childCount - 2).localPosition.y + (transform.parent.GetChild(transform.parent.childCount - 2).GetComponent<BoxCollider>().size.y / 2) + (gameObject.GetComponent<BoxCollider>().size.y / 2), 0);
            } else {
                transform.localPosition = new Vector3(
                    Mathf.Clamp(transform.localPosition.x, -0.3f, 0.3f), 
                    transform.parent.GetChild(transform.parent.childCount - 2).localPosition.y + (transform.parent.GetChild(transform.parent.childCount - 2).GetComponent<BoxCollider>().size.y / 2) + (gameObject.GetComponent<BoxCollider>().size.y / 2), 
                    Mathf.Clamp(transform.localPosition.z, -0.3f, 0.3f)
                    );
            }
            //transform.localPosition = new Vector3(transform.localPosition.x, transform.parent.GetChild(transform.parent.childCount - 2).localPosition.y + (transform.parent.GetChild(transform.parent.childCount - 2).GetComponent<BoxCollider>().size.y / 2) + (gameObject.GetComponent<BoxCollider>().size.y / 2), transform.localPosition.z);
            
            //Debug.Log(transform.parent.GetChild(transform.parent.childCount - 2));
        }
        //transform.localPosition = new Vector3(transform.localPosition.x, gameObject.GetComponent<BoxCollider>().size.y, transform.localPosition.z);
    }

    public void SetActive(bool active)
    {
        gameObject.GetComponent<Food>().enabled = active;
        if (gameObject.GetComponent<PickupObject>())
        {
            gameObject.GetComponent<PickupObject>().enabled = active;
        }
    }

    public bool getGrillCooked()
    {
        return grillCooked;
    }

    public bool getOvenCooked()
    {
        return ovenCooked;
    }

    public float getCooked()
    {
        return cooked;
    }

    public float getOvercooked()
    {
        return overcooked;
    }

    public void setCookingSpeedModifier(float f)
    {
        cookSpeedModifier = f;
    }

    public void setOvenCooked(bool b)
    {
        ovenCooked = b;
    }

    public void setGrillCooked(bool b)
    {
        grillCooked = b;
    }

    private void Update()
    {
        if (ignoreTriggerDelay > 0f)
        {
            ignoreTriggerDelay = Mathf.Max(0f, ignoreTriggerDelay - Time.deltaTime);
        }
        foodTemp = Mathf.Lerp(foodTemp, startFoodTemp, 0.01f * Time.deltaTime);
        if (true) {

        }
    }

    //[RPC]
    public void BurgerExplosion(float force/*, NetworkViewID targetID*/)
    {
        Transform transform = /*NetworkView.Find(targetID).transform*/gameObject.transform;
        Food component = transform.GetComponent<Food>();
        print(string.Concat(new object[]
        {
            "Getting burger stack for ",
            transform.name,
            " with ",
            burgerStack.foodOnBurger.Count,
            " items"
        }));
        int count = burgerStack.foodOnBurger.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            print(burgerStack.foodOnBurger[i].transform.name + " is exploding " + i);
            Transform transform2 = burgerStack.foodOnBurger[i].transform;
            if (transform2.tag.Contains("Food"))
            {
                /*if (networkView.isMine)
                {
                    networkView.RPC("SetActive", 2, new object[]
                    {
                        transform2.networkView.viewID,
                        true
                    });
                    networkView.RPC("SetObservedToNetObj", 2, new object[]
                    {
                        transform2.networkView.viewID
                    });
                    if (transform2.GetComponent<Food>().type == FoodType.bun && i > 0)
                    {
                        networkView.RPC("BurgerExplosion", 2, new object[]
                        {
                            force,
                            transform2.networkView.viewID
                        });
                    }
                }*/
                
                if (transform2.name.Contains("rat"))
                {
                    transform2.GetComponent<Rat>().enabled = true;
                }
                transform2.parent = null;
                transform2.GetComponent<Food>().ignoreTriggerDelay = 1f;
                if (!transform2.GetComponent<Rigidbody>())
                {
                    transform2.gameObject.AddComponent<Rigidbody>();
                    /*if (networkView.isMine)
                    {
                        transform2.GetComponent<Rigidbody>().AddExplosionForce((i / Mathf.Max(count - 1, 1)) * force, transform2.position, component.burgerStack.foodOnBurger.Count);
                    }*/
                    transform2.GetComponent<Rigidbody>().AddExplosionForce(i / Mathf.Max(count - 1, 1) * force, transform2.position, component.burgerStack.foodOnBurger.Count);
                }
            }
        }
        component.burgerStack.foodOnBurger.Clear();
        burgerStack.enabled = true;
        burgerStack.Reset();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (type != FoodType.bun /*|| !Network.isServer*/ || !GetComponent<Rigidbody>() || burgerStack.foodOnBurger.Count <= 0 || collision.relativeVelocity.magnitude > 7f)
        {
        }
    }

    public FoodType type;

    private BurgerStacking burgerStack;

    private bool ovenCooked;

    private bool grillCooked;

    public float cooked;

    private float cookedDelay;

    public float overcooked;

    public float cookTimeIdeal = 10f;

    public float cookTimeBurnDelay = 10f;

    public float cookTimeBurned = 10f;

    public float foodTemp = 20f;

    private float startFoodTemp;

    /*private float maxFoodTemp = 120f;

    private float minFoodTemp = -20f;*/

    public bool supportsTextureBlend;

    private Color highlightColor;

    private Color originalColor;

    public float textureToKeep = 0.5f;

    public float cookedRed = 0.2f;

    public float cookedGreen;

    public float cookedBlue;

    public float cookSpeedModifier = 1f;

    public bool inFood;

    public bool snapsToCentreInBurger = true;

    public float ignoreTriggerDelay;

    public bool foodBeenOnFloor;

    public Rat beingHeldByRat;

    public enum FoodType
    {
        physics,
        patty,
        potato,
        topBun,
        bun,
        lettuce,
        cheese,
        tomato,
        bacon,
        pineapple,
        rat,
        other
    }
}
