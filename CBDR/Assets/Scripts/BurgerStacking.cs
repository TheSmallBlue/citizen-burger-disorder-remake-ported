using System.Collections.Generic;
using UnityEngine;

public class BurgerStacking : MonoBehaviour
{
    private void Start()
    {
        //layerMask = 1 << LayerMask.NameToLayer("Food");
        originalPosition = transform.localPosition;
    }

    public void Reset()
    {
        GetComponent<Collider>().enabled = true;
        transform.localPosition = originalPosition;
        foodOnBurger.Clear();
    }

    private void LateUpdate()
    {
        /*if (Time.frameCount % 30 == 0 && checkCount < maxChecks)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position + transform.up * 2f, Vector3.down, out raycastHit, 6f, layerMask) && raycastHit.transform != transform && raycastHit.transform.IsChildOf(transform.root))
            {
                Vector3 position;
                position = new Vector3(transform.position.x, raycastHit.point.y, transform.position.z);
                if (position.y > transform.position.y + 0.1f)
                {
                    transform.position = position;
                }
            }
            checkCount++;
        }
        RaycastHit raycastHit2;
        if (lateUpdateRequired && foodOnBurger.Count > 0 && Physics.Raycast(transform.position + transform.up * 2f, Vector3.down, out raycastHit2, 6f, layerMask) && raycastHit2.transform.IsChildOf(transform.root))
        {
            Vector3 position2;
            position2 = new Vector3(transform.position.x, raycastHit2.point.y, transform.position.z);
            if (position2.y > transform.position.y)
            {
                transform.position = position2;
            }
        }
        lateUpdateRequired = false;*/
        if (transform.parent.childCount - 1 > 0) {
            Transform childObject = transform.parent.GetChild(transform.parent.childCount - 1);
            transform.localPosition = new Vector3(0, childObject.localPosition.y + (childObject.GetComponent<BoxCollider>().size.y / 2), 0);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        /*if (other.GetComponent<Food>()) {
            Debug.Log("Object: " + other.gameObject.name + " | " +
                (!other.name.Contains("bun-bottom") || (other.name.Contains("bun-bottom") && foodOnBurger.Count > 0)) + " | " + 
                (!other.name.Contains("bun-top") || (other.name.Contains("bun-top") && foodOnBurger.Count > 0)));
        }*/
        //GetComponent<Collider>().enabled && other.GetComponent<Collider>().enabled && other.GetComponent<Food>() != null && (!other.name.Contains("bun-bottom") || (other.name.Contains("bun-bottom") && foodOnBurger.Count > 0)) && (!other.name.Contains("bun-top") || (other.name.Contains("bun-top") && foodOnBurger.Count > 0)) && other.tag == "PhysicsFood" && other.GetComponent<Rigidbody>() && other.GetComponent<Food>() != null && !transform.parent.parent.GetComponent<PickupObject>().beingHeld && other.GetComponent<Rigidbody>().useGravity && other.GetComponent<Food>().ignoreTriggerDelay <= 0f
        /*
         GetComponent<Collider>().enabled && 
         other.GetComponent<Collider>().enabled && 
         other.GetComponent<Food>() && 
         (!other.name.Contains("bun-bottom") || (other.name.Contains("bun-bottom") && foodOnBurger.Count > 0)) && 
         (!other.name.Contains("bun-top") || (other.name.Contains("bun-top") && foodOnBurger.Count > 0)) && 
         other.tag == "PhysicsFood" && 
         other.GetComponent<Rigidbody>() && 
         !transform.parent.parent.GetComponent<PickupObject>().beingHeld && 
         other.GetComponent<Rigidbody>().useGravity && 
         other.GetComponent<Food>().ignoreTriggerDelay <= 0f
        */
        if (GetComponent<Collider>().enabled && other.GetComponent<Collider>().enabled && other.GetComponent<Food>() && (!other.name.Contains("bun-bottom") || (other.name.Contains("bun-bottom") && foodOnBurger.Count > 0)) && (!other.name.Contains("bun-top") || (other.name.Contains("bun-top") && foodOnBurger.Count > 0)) && other.tag == "PhysicsFood" && other.GetComponent<Rigidbody>() && !transform.parent.parent.GetComponent<PickupObject>().beingHeld && other.GetComponent<Rigidbody>().useGravity && other.GetComponent<Food>().ignoreTriggerDelay <= 0f)
        {
            if (/*peerType == 1*/true)
            {
                Food component = other.GetComponent<Food>();
                if (other.GetComponent<Collider>()/*other.GetComponent<Collider>() != null && transform.parent.parent.GetComponent<Rigidbody>() != null && other.GetComponent<PickupObject>().lastPlayerHolding != null*/)
                {
                    /*transform.parent.parent.networkView.RPC("AddFoodToBurger", 6, new object[]
                    {
                        other.networkView.viewID
                    });*/
                    transform.parent.parent.GetComponent<Food>().AddFoodToBurger(other.transform);
                    //other.transform.rotation = Quaternion.Euler(Mathf.Clamp(other.transform.rotation.eulerAngles.x, -2.5f, 2.5f), other.transform.rotation.eulerAngles.y, Mathf.Clamp(other.transform.rotation.eulerAngles.z, -2.5f, 2.5f));
                    //other.transform.rotation = Quaternion.Euler(0, other.transform.rotation.eulerAngles.y, 0);
                    other.transform.eulerAngles = new Vector3(0, other.transform.rotation.eulerAngles.y, 0);
                    /*if (component.snapsToCentreInBurger)
                    {
                        other.transform.position = transform.position + other.transform.up * (other.transform.GetComponent<Collider>().bounds.size.y / 3f);
                    }
                    else
                    {
                        other.transform.position = new Vector3(Mathf.Clamp(other.transform.position.x, transform.position.x - transform.localScale.x / 3f, transform.position.x + transform.localScale.x / 3f), transform.position.y, Mathf.Clamp(other.transform.position.z, transform.position.z - transform.localScale.z / 3f, transform.position.z + transform.localScale.z / 3f));
                    }*/
                    if (other.name.Contains("bun-bottom"))
                    {
                        /*transform.parent.parent.networkView.RPC("MoveFoodFromBurger", 6, new object[]
                        {
                            other.networkView.viewID
                        });*/
                        transform.parent.parent.GetComponent<Food>().MoveFoodFromBurger();
                    }
                    if (other.name.Contains("rat"))
                    {
                        other.GetComponent<Rat>().enabled = false;
                    }
                    component.inFood = true;
                    /*other.networkView.RPC("SetObjectPosition", 2, new object[]
                    {
                        other.transform.position,
                        other.transform.rotation,
                        other.networkView.viewID
                    });
                    other.networkView.RPC("SetParent", 2, new object[]
                    {
                        transform.parent.parent.networkView.viewID,
                        "burger"
                    });
                    other.networkView.RPC("DestroyRigidbody", 2, new object[]
                    {
                        other.networkView.viewID
                    });
                    other.networkView.RPC("SetActive", 2, new object[]
                    {
                        other.networkView.viewID,
                        false
                    });
                    other.networkView.RPC("SetObservedToTransform", 2, new object[]
                    {
                        other.networkView.viewID
                    });*/
                    //component.SetObjectPosition(other.transform.position, other.transform.rotation);
                    component.SetParent(transform.parent);
                    component.DestroyRigidbody();
                    component.SetActive(false);
                    /*if (GetComponent<Collider>().enabled)
                    {
                        lateUpdateRequired = true;
                    }
                    checkCount = 0;*/
                }
                if (foodCount(Food.FoodType.topBun) > 0)
                {
                    GetComponent<Collider>().enabled = false;
                    enabled = false;
                    Debug.Log("TopBun is worked?");
                }
            }/*
            else
            {
                Food component2 = other.GetComponent<Food>();
                if (component2.snapsToCentreInBurger)
                {
                    other.transform.position = transform.position + other.transform.up * (other.transform.GetComponent<Collider>().bounds.size.y / 3f);
                }
                else
                {
                    other.transform.position = new Vector3(Mathf.Clamp(other.transform.position.x, transform.position.x - transform.localScale.x / 3f, transform.position.x + transform.localScale.x / 3f), transform.position.y, Mathf.Clamp(other.transform.position.z, transform.position.z - transform.localScale.z / 3f, transform.position.z + transform.localScale.z / 3f));
                }
            }*/
        }
    }

    public int foodCount(Food.FoodType type)
    {
        int num = 0;
        foreach (Food food in foodOnBurger)
        {
            if (food.type == type)
            {
                num++;
            }
        }
        return num;
    }

    public List<Food> foodOnBurger = new List<Food>();

    //private LayerMask layerMask;

    private Vector3 originalPosition;

    /*private bool lateUpdateRequired;

    private int checkCount;

    private int maxChecks = 4;*/

    private bool complete;
}
