using UnityEngine;

public class Sink : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Instantiate(bubbles, other.transform.position, other.transform.rotation);
        if (other.name.Contains("rat"))
        {
            other.GetComponent<Rat>().GiveUp(other.gameObject);
            other.GetComponent<Rat>().enabled = false;
        }
        if (other.GetComponent<Flamable>())
        {
            print("Yo");
            other.GetComponent<Flamable>().FireBurnOut(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Physics") && other.GetComponent<MeshRenderer>() && other.GetComponent<MeshRenderer>().material.name.Contains("plate"))
        {
            float @float = other.GetComponent<MeshRenderer>().material.GetFloat("_Blend");
            if (@float > 0f)
            {
                other.GetComponent<MeshRenderer>().material.SetFloat("_Blend", Mathf.Max(0f, @float - Time.deltaTime * this.speed));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MeshRenderer>() != null && other.GetComponent<MeshRenderer>().material.name.Contains("StaffMenuTex"))
        {
            other.GetComponent<DrawTexture>().NewTex();
        }
    }

    private void Start()
    {
        bubbles = (Resources.Load("Prefabs/Dishwashing/Bubbles") as GameObject);
    }

    private GameObject bubbles;

    private float speed = 0.1f;
}
