using System;
using UnityEngine;

public class Grill : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (/*this.sfxMeatCooking != null && */other.GetComponent<Food>() != null)
        {
            //AudioSource.PlayClipAtPoint(this.sfxMeatCooking, other.transform.position);
            GameObject instantiateObject = Instantiate(prefab, transform.position, Quaternion.identity);
            instantiateObject.GetComponent<NewAudioSourceScript>().OnGrill(other.gameObject, transform.position, gameObject.GetComponent<BoxCollider>().size);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Food>())
        {
            Food component = other.GetComponent<Food>();
            component.cook();
            if (component.cookSpeedModifier != 1f)
            {
                component.cookSpeedModifier = 1f;
            }
        }
    }

    //public AudioClip sfxMeatCooking;
    public GameObject prefab;
}