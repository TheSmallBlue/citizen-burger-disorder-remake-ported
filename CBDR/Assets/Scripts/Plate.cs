using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("bun-bottom") && other.transform.GetChild(0).Find("triggerBunStack").GetComponent<BurgerStacking>().foodOnBurger.Count > 0 /*&& other.transform.GetChild(0).Find("triggerBunStack").GetComponent<BurgerStacking>().foodCount(Food.FoodType.topBun) > 0*/ && foodOnPlate.Count == 0 && other.GetComponent<Rigidbody>() && other.GetComponent<Food>() && !other.GetComponent<PickupObject>().beingHeld/* && other.transform.parent != transform.parent && transform.parent.GetComponent<Rigidbody>().useGravity*/)
        {
            print("yeaaaah!");
            Food component = other.GetComponent<Food>();
            if (!foodOnPlate.Contains(component))
            {
                /*if (Network.isServer)
                {
                    other.networkView.RPC("SetObservedToTransform", 2, new object[]
                    {
                        other.networkView.viewID
                    });
                }*/
                component.AddFoodToPlate(other.transform, transform.parent);
                component.SetObjectPosition(other.transform.position, other.transform.rotation);
                component.SetParent(transform.parent);
                component.DestroyRigidbody();
                component.SetActive(false);
                transform.parent.GetComponent<MeshRenderer>().material.SetFloat("_Blend", 0.2f);
                if (component.type == Food.FoodType.bun)
                {
                    other.transform.Find("burger-bottom").Find("triggerBunStack").GetComponent<BurgerStacking>().enabled = false;
                }
            }
        }
    }

    public int foodCount(Food.FoodType type)
    {
        int num = 0;
        foreach (Food food in foodOnPlate)
        {
            if (food.type == type)
            {
                num++;
            }
        }
        return num;
    }

    private void Update()
    {
    }

    public List<Food> foodOnPlate = new List<Food>();
}
