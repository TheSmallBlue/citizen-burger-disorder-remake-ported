using UnityEngine;
public class CashRegister : MonoBehaviour {
    void Start() {
        display = transform.parent.Find("Display").GetComponent<TextMesh>();
    }

    void Update() {
        RefreshDisplay(0f);
    }

    public void RefreshDisplay(float alterationValue = 0f)
    {
        money += alterationValue;
        display.text = "$" + money;
        //SyncMoney(money);
    }

    /*private void SyncMoney(float currentMoney)
    {
        money = currentMoney;
        RefreshDisplay(0f);
    }*/

    private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("Tip") && !other.GetComponent<PickupObject>().beingHeld)
        {
            RefreshDisplay(2f);
            other.gameObject.GetComponent<PickupObject>().DestroyObject();
        }
    }

    public float money = 100f;

    private TextMesh display;
}
