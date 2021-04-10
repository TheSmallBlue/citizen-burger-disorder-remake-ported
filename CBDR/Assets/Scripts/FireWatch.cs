using System.Collections.Generic;
using UnityEngine;

public class FireWatch : MonoBehaviour
{
    private void Start()
    {
        firePrefab = (Resources.Load("Prefabs/Fire/Fire") as GameObject);
    }

    private void OnPlayerConnected(/*NetworkPlayer player*/)
    {
        foreach (FireAnimate fireAnimate in AllFireAnimates)
        {
            /*if (fireAnimate.fireBase)
            {
                networkView.RPC("CreateFireAnimate", player, new object[]
                {
                    fireAnimate.transform.position,
                    fireAnimate.transform.rotation,
                    fireAnimate.fireBase.nearBigFire,
                    fireAnimate.fireBase.networkView.viewID,
                    fireAnimate.networkView.viewID
                });
                networkView.RPC("SyncAllFlamable", player, new object[]
                {
                    fireAnimate.fireBase.networkView.viewID,
                    fireAnimate.fireBase.isOnFire,
                    fireAnimate.fireBase.wasOnFire,
                    fireAnimate.fireBase.isFlamableAgain,
                    fireAnimate.fireBase.reflamable,
                    fireAnimate.fireBase.currentBurnHealth,
                    fireAnimate.fireBase.currentTemp,
                    fireAnimate.fireBase.nearBigFire,
                    fireAnimate.fireBase.currentFireCheckRate
                });
            }
            else
            {
                networkView.RPC("CreateBigFireAnimate", player, new object[]
                {
                    fireAnimate.transform.position,
                    fireAnimate.transform.rotation,
                    fireAnimate.networkView.viewID
                });
            }*/
        }
    }

    //[RPC]
    public void CreateFireAnimate(Vector3 position, Quaternion rotation, bool nearBigFire/*, NetworkViewID callerID, NetworkViewID newObjectID*/, GameObject objectComponent)
    {
        GameObject gameObject = Instantiate(firePrefab, position, rotation);
        /*gameObject.networkView.viewID = newObjectID;
        SetupNewFire(gameObject.networkView.viewID, callerID, false, nearBigFire);
        if (Network.isServer)
        {
            AllFireAnimates.Add(gameObject.GetComponent<FireAnimate>());
        }*/

        SetupNewFire(gameObject, objectComponent, false, nearBigFire);
        AllFireAnimates.Add(gameObject.GetComponent<FireAnimate>());
    }

    //[RPC]
    public void CreateBigFireAnimate(Vector3 position, Quaternion rotation/*, NetworkViewID newObjectID*/)
    {
        GameObject gameObject = Instantiate(firePrefab, position, rotation);
        /*gameObject.networkView.viewID = newObjectID;
        SetupNewBigFire(gameObject);
        if (Network.isServer)
        {
            AllFireAnimates.Add(gameObject.GetComponent<FireAnimate>());
        }*/
    }

    //[RPC]
    private void SetupNewFire(GameObject gameObject, GameObject objectComponent, bool isLargeFire, bool nearBigFire)
    {
        GameObject gameObject1;
        Flamable component;
        try
        {
            gameObject1 = /*NetworkView.Find(objectID).*/gameObject;
            component = /*NetworkView.Find(creatorObjectID).GetComponent<Flamable>()*/objectComponent.GetComponent<Flamable>();
        }
        catch (UnityException ex)
        {
            Debug.Log(ex);
            return;
        }
        FollowGameObject component2 = gameObject1.GetComponent<FollowGameObject>();
        component2.follow = component.gameObject;
        component2.distance = component.fireOffsetLocaiton;
        component.fireAnimate = component2.GetComponent<FireAnimate>();
        component.fireAnimate.fireBase = component;
        component.nearBigFire = nearBigFire;
    }

    private void SetupNewBigFire(GameObject fire)
    {
        fire.transform.localScale = new Vector3(fire.transform.localScale.x * 3.5f, fire.transform.localScale.y * 2.5f, fire.transform.localScale.z * 2.5f);
        fire.transform.position = fire.transform.position;
        fire.GetComponent<FireAnimate>().isLargeFire = true;
    }

    //[RPC]
    public void SyncAllFlamable(GameObject objectComponent, bool nIsOnFire, bool nWasOnFire, bool nIsFlamableAgain, bool nReflamable, float nCurrentBurnHealth, float nCurrentTemp, bool nNearBigFire, float nCurrentFireCheckRate)
    {
        Flamable component;
        component = objectComponent.GetComponent<Flamable>();
        /*try
        {
            component = NetworkView.Find(objectID)gameObject.GetComponent<Flamable>();
        }
        catch (UnityException ex)
        {
            Debug.Log(ex);
            return;
        }*/
        component.isOnFire = nIsOnFire;
        component.wasOnFire = nWasOnFire;
        component.isFlamableAgain = nIsFlamableAgain;
        component.reflamable = nReflamable;
        component.currentBurnHealth = nCurrentBurnHealth;
        component.currentTemp = nCurrentTemp;
        component.nearBigFire = nNearBigFire;
        component.currentFireCheckRate = nCurrentFireCheckRate;
        /*if (component.isOnFire)
        {
            component.FireIgnite();
        }*/
    }

    public static void AddFireReference(FireAnimate newFireRef)
    {
        AllFireAnimates.Add(newFireRef);
    }

    public static void RemoveFireReference(FireAnimate removeFireRef)
    {
        AllFireAnimates.Remove(removeFireRef);
    }

    public static List<FireAnimate> AllFireAnimates = new List<FireAnimate>();

    private GameObject firePrefab;
}
