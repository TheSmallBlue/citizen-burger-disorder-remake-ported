using UnityEngine;

public class FireInputController : MonoBehaviour
{
    public bool WaterOn;

    private PickupObject pickup;

    private ParticleSystem waterEmitter;

    private ParticleSystem foamEmitter;

    bool stopBool;

    private void OnPlayerConnected(/*NetworkPlayer player*/)
    {
        //base.GetComponent<NetworkView>().RPC("SyncAllFireInput", player, base.GetComponent<NetworkView>().viewID, WaterOn);
        SyncAllFireInput(WaterOn);
    }

    //[RPC]
    private void SyncAllFireInput(/*NetworkViewID objectID, */bool nWaterOn)
    {
        FireInputController component;
        try
        {
            component = /*NetworkView.Find(objectID)*/gameObject.GetComponent<FireInputController>();
        }
        catch (UnityException message)
        {
            Debug.Log(message);
            return;
        }
        component.WaterOn = nWaterOn;
    }

    private void Start()
    {
        waterEmitter = transform.Find("WaterEmitter").GetComponent<ParticleSystem>();
        foamEmitter = transform.Find("Foam").GetComponent<ParticleSystem>();
        if (WaterOn) {
            waterEmitter.Play();
            foamEmitter.Play();
        } else {
            waterEmitter.Stop();
            foamEmitter.Stop();
        }
        pickup = GetComponent<PickupObject>();
    }

    private void Update()
    {
        if (pickup.beingHeld/* && pickup.playerHolding == FirstPersonControl.localPlayer*/)
        {
            if (Input.GetKey(OppositeHandKey()))
            {
                WaterOn = true;
                EmitToggle(WaterOn);
            }
            else
            {
                WaterOn = false;
                EmitToggle(WaterOn);
            }
        }
        if (!pickup.playerHolding)
        {
            WaterOn = false;
        }
    }

    private void EmitToggle(bool nWaterOn)
    {
        FireInputController component;
        try
        {
            component = gameObject.GetComponent<FireInputController>();
        }
        catch (UnityException message)
        {
            Debug.Log(message);
            return;
        }
        component.WaterOn = nWaterOn;
    }

    private void LateUpdate()
    {
        /*
        if (WaterOn) {
            waterEmitter.Play();
            foamEmitter.Play();
        } else {
            waterEmitter.Stop();
            foamEmitter.Stop();
        }
        */
        if (WaterOn) {
            if (!stopBool) {
                waterEmitter.Play();
                foamEmitter.Play();
                transform.Find("WaterTrigger").gameObject.SetActive(true);
                stopBool = !stopBool;
            }
        } else {
            if (stopBool) {
                waterEmitter.Stop();
                foamEmitter.Stop();
                transform.Find("WaterTrigger").gameObject.SetActive(false);
                stopBool = !stopBool;
            }
        }
    }

    private KeyCode OppositeHandKey()
    {
        /*if (pickup.playerHolding.leftArmObject && pickup.playerHolding.leftArmObject == transform)
        {
            return KeyCode.Mouse1;
        }
        if (pickup.playerHolding.rightArmObject && pickup.playerHolding.rightArmObject == transform)
        {
            return KeyCode.Mouse0;
        }*/
        if (pickup.playerHolding && pickup.playerHolding.leftArmObject && pickup.playerHolding.leftArmObject == transform)
        {
            return KeyCode.Mouse1;
        }
        if (pickup.playerHolding && pickup.playerHolding.rightArmObject && pickup.playerHolding.rightArmObject == transform)
        {
            return KeyCode.Mouse0;
        }
        return KeyCode.P;
    }
}
