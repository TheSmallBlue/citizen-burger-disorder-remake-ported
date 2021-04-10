using UnityEngine;

public class PickupObject : MonoBehaviour
{
    private void Awake()
    {
        currentHeldRotateXOffset = heldRotateXOffset;
    }

    public void ResetHeldRotation()
    {
        currentHeldRotateXOffset = heldRotateXOffset;
    }

    /*private void Update()
    {
        if (beingHeld && playerHolding && armHoldingObject != null)
        {
            //Vector3 vector = armHoldingObject.transform.Find("hand").transform.position + armHoldingObject.transform.Find("hand").transform.forward * 2f * heldPositionOffset.z + armHoldingObject.transform.Find("hand").transform.right * heldPositionOffset.x + playerHolding.transform.up * heldPositionOffset.y;
            Vector3 vector = armHoldingObject.transform.Find("hand").transform.position + armHoldingObject.transform.Find("hand").transform.forward * 2f * heldPositionOffset.z + armHoldingObject.transform.Find("hand").transform.right * heldPositionOffset.x + playerHolding.transform.up * heldPositionOffset.y;
            Quaternion quaternion = armHoldingObject.transform.rotation * Quaternion.Euler(0f, 0f, 0f);
            transform.position = Vector3.Lerp(transform.position, vector, 30f * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, quaternion, 30f * Time.deltaTime);
        }
    }*/


    private void DestroyObjectBuffered()
    {
        Destroy(gameObject);
    }

    public void DestroyObject()
    {
        if (GetComponent<Flamable>())
        {
            Flamable component = GetComponent<Flamable>();
            component.FireBurnOut(false);
        }
        if (createdInScene)
        {
            DestroyObjectBuffered();
        }
        else
        {
            Destroy(gameObject);
        }

        if (GetComponent<Flamable>())
        {
            Flamable component = GetComponent<Flamable>();
            component.FireBurnOut(false);
        }
    }

    public void SetBeingHeld(bool held, FirstPersonControl firstPersonControl)
    {
        PickupObject component = transform.GetComponent<PickupObject>();
        component.beingHeld = held;
        if (held)
        {
            //armHoldingObject = gameObject;
            //playerHolding = transform.GetComponent<FirstPersonControl>();
            playerHolding = firstPersonControl;
            lastPlayerHolding = playerHolding;
        }
        else
        {
            armHoldingObject = null;
            playerHolding = null;
        }
    }

    public bool IsBeingUsed()
    {
        bool result = false;
        if (GetComponent<ObjectUsable>())
        {
            result = GetComponent<ObjectUsable>().beingUsed;
        }
        return result;
    }

    public bool createdInScene;

    public GameObject armHoldingObject;

    public FirstPersonControl playerHolding;

    public FirstPersonControl lastPlayerHolding;

    public bool beingHeld;

    public bool beingUsed;

    public Vector3 heldPositionOffset = new Vector3(0f, 1f, 1f);

    public float heldRotateXOffset = -30f;

    private float currentHeldRotateXOffset;
}
