using UnityEngine;
public class OrderTriggerNumber : MonoBehaviour {
    OrderScript orderScript;
    void Start() {
        orderScript = GameObject.Find("OrderPoint").GetComponent<OrderScript>();
    }
    void OnTriggerEnter(Collider other) {
        if (orderScript.npcs.ToArray().Length > 0 && orderScript.npcs[0].GetComponent<NPC>().askingForOrder && other.gameObject.name.Contains("OrderNumber")) {
            orderScript.npcs[0].GetComponent<NPC>().orderNumber = other.gameObject;
            orderScript.npcs[0].GetComponent<NPC>().setWants(NPC.wants.toFindSeat);
            orderScript.npcs.RemoveAt(0);
        }
    }
}
