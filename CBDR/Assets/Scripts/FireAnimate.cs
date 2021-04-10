using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAnimate : MonoBehaviour
{
    /*private void OnParticleCollision(GameObject other)
    {
        if (other.name.Equals("WaterEmitter"))
        {
            PutOut(false);
        }
    }*/

    private void OnTriggerStay(Collider other) {
        if (other.name == "WaterTrigger") {
            PutOut(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Flamable>() && !other.GetComponent<Flamable>().isOnFire)
        {
            other.GetComponent<Flamable>().currentBurnHealth -= 4f;
        }
    }

    private void Start()
    {
        //smoke = transform.Find("Smoke").GetComponent<ParticleEmitter>();
        fireMat = GetComponent<MeshRenderer>().material;
        fireGlow = GetComponent<Light>();
        AllFires.Add(this);
        StartCoroutine(RenderFires());
        Vector3 localScale = transform.localScale;
        localScale.y = -localScale.y;
        transform.localScale = localScale;
    }

    private void Update()
    {
        //smoke.emit = isLargeFire;
        if (lastMaterialSwapTime + materialSwapDuration < Time.time)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            lastMaterialSwapTime = Time.time;
            materialSwapDuration = Random.Range(0.1f, 0.3f);
        }
    }

    private IEnumerator RenderFires()
    {
        while (this)
        {
            FlameOn();
            colliderProximity = Physics.OverlapSphere(transform.position, 1f);
            foreach (Collider c in colliderProximity)
            {
                if (c.GetComponent<FireAnimate>() && c.GetComponent<FireAnimate>().isLargeFire && c != GetComponent<Collider>())
                {
                    FlameOff();
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield break;
    }

    private void FlameOn()
    {
        if (!GetComponent<MeshRenderer>().enabled)
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

    private void FlameOff()
    {
        if (GetComponent<MeshRenderer>().enabled && fireBase != null && fireBase.pickup != null && !fireBase.pickup.beingHeld)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void PutOut(bool sentFromFlamable = false)
    {
        /*if (Network.isServer)
        {
            FireWatch.RemoveFireReference(this);
            if (isLargeFire)
            {
                RemoveFromAllFires();
            }
            else if (!sentFromFlamable)
            {
                fireBase.FireBurnOut(false);
            }
            networkView.RPC("PutOutFire", 1, new object[]
            {
                networkView.viewID
            });
            Destroy(gameObject);
        }*/
        FireWatch.RemoveFireReference(this);
        if (isLargeFire)
        {
            RemoveFromAllFires();
        }
        else if (!sentFromFlamable)
        {
            fireBase.FireBurnOut(false);
        }
        Destroy(gameObject);
    }

    //[RPC]
    private void PutOutFire(/*NetworkViewID objectID*/)
    {
        FireAnimate component;
        try
        {
            component = /*NetworkView.Find(objectID)*/gameObject.GetComponent<FireAnimate>();
        }
        catch (UnityException ex)
        {
            Debug.Log(ex);
            return;
        }
        if (component.isLargeFire)
        {
            /*component.smoke.transform.parent = transform.root;
            component.smoke.emit = false;
            component.smoke.GetComponent<ParticleAnimator>().autodestruct = true;*/
        }
        Destroy(component.gameObject);
    }

    public void RemoveFromAllFires()
    {
        AllFires.Remove(this);
    }

    public static List<FireAnimate> AllFires = new List<FireAnimate>();

    public Flamable fireBase;

    public bool isLargeFire;

    private Material fireMat;

    /*private Light fireLight;

    private float lightSpeed = 1f;

    private float lightIntensity = 5f;

    private float lightOffset = 1f;

    private float scaleSpeed = 1f;

    private float scaleIntensity = 0.2f;

    private float scaleOffset = 1f;*/

    private float lastMaterialSwapTime;

    private float materialSwapDuration;

    private Light fireGlow;

    private Collider[] colliderProximity;

    //private ParticleEmitter smoke;
}
