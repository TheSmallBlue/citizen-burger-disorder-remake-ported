using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Table : MonoBehaviour
{
    public List<string> foodOrder = new List<string>();

    public NPC npcAtTable;

    public TableNodes myTableNode;

    public Transform prefSpeech;

    public Transform reward;

    private Transform speechBubble;

    public int tableNumber;

    private Computer mainComputer;

    /*private void OnPlayerConnected(NetworkPlayer player)
    {
        if (foodOrder.Count > 0)
        {
            base.networkView.RPC("GetSyncFoodOrder", RPCMode.Others, base.networkView.viewID, foodOrder[0]);
        }
    }*/

    private void Update()
    {
        if (speechBubble != null && (speechBubble.position - transform.position + transform.up * 4f).magnitude > 0.01f)
        {
            speechBubble.position = Vector3.Lerp(speechBubble.position, transform.position + transform.up * 2f, 3f * Time.deltaTime);
        }
        if (foodOrder != null && foodOrder.Count > 0)
        {
            if (speechBubble == null && (Camera.main.transform.position - transform.position).magnitude < 15f)
            {
                CreateSpeechBubble();
            }
            else if (speechBubble != null && (Camera.main.transform.position - transform.position).magnitude >= 15f)
            {
                DestroySpeechBubble();
            }
        }

        if (plate) {
            plate.GetComponent<Rigidbody>().isKinematic = true;
            plate.tag = "Untagged";
            plate.transform.position = Vector3.Lerp(plate.transform.position, transform.position + new Vector3(0, 0.5f, 0), 2 * Time.deltaTime);
            plate.transform.rotation = Quaternion.Lerp(plate.transform.rotation, Quaternion.identity, 2 * Time.deltaTime);

            timer += Time.deltaTime;
            if (timer >= 1) {
                plate.transform.Find("bun-bottom").position = Vector3.Lerp(plate.transform.Find("bun-bottom").position, npcAtTable.transform.position + npcAtTable.transform.forward * 1.2f + npcAtTable.transform.up * 1.5f + npcAtTable.transform.up * (Mathf.PingPong(timer, 0.1f) * 20 - 1.2f), 2 * Time.deltaTime);
            }
            if (timer >= 20 && !stopBool) {
                plate.GetComponent<Rigidbody>().isKinematic = false;
                plate.tag = "Physics";
                plate.transform.Find("bun-bottom").position = plate.transform.position;
                FinishEating(plate);
                plate = null;
                stopBool = true;
            }
        }
    }

    public void ClearOrder()
    {
        print("Clearing food order");
        if (speechBubble != null/* && base.networkView.isMine*/)
        {
            DestroySpeechBubble();
            if (mainComputer == null && GameObject.Find("!Monitor"))
            {
                mainComputer = GameObject.Find("!Monitor").GetComponent<Computer>();
            }
            if (mainComputer) {
                mainComputer.ClearFoodFromTable(tableNumber - 1, foodOrder[0]);
            }
        }
        foodOrder.Clear();
    }

    private void CreateSpeechBubble(string order = null)
    {
        if (order == null)
        {
            order = foodOrder[0].ToLower();
        }
        Transform transform = speechBubble = Instantiate(prefSpeech, base.transform.position + base.transform.up, base.transform.rotation);
        speechBubble.GetChild(0).transform.GetComponent<MeshRenderer>().material = Resources.Load("UI/Materials/" + order) as Material;
    }

    private void CreateSpeechBubble(GameObject target, string order = null)
    {
        Transform transform = target.transform;
        Table component = transform.GetComponent<Table>();
        if (order == null)
        {
            order = component.foodOrder[0].ToLower();
        }
        Transform transform2 = speechBubble = Instantiate(prefSpeech, transform.transform.position + transform.transform.up, transform.transform.rotation);
        speechBubble.GetChild(0).transform.GetComponent<MeshRenderer>().material = Resources.Load("UI/Materials/" + order) as Material;
    }

    private void DestroySpeechBubble()
    {
        if (speechBubble != null)
        {
            Destroy(speechBubble.gameObject);
        }
    }

    private void DestroySpeechBubble(GameObject target)
    {
        Transform transform = target.transform;
        Table component = transform.GetComponent<Table>();
        if (component.speechBubble != null)
        {
            Destroy(component.speechBubble.gameObject);
        }
    }

    private void GetSyncFoodOrder(GameObject target, string newFoodOrder)
    {
        Table component = target.GetComponent<Table>();
        print("Adding: " + newFoodOrder);
        if (component.foodOrder == null)
        {
            component.foodOrder = new List<string>();
        }
        component.foodOrder.Add(newFoodOrder);
    }

    public void SetNPCAtTable(GameObject table, GameObject npc)
    {
        Table component = table.GetComponent<Table>();
        NPC nPC = component.npcAtTable = npc.GetComponent<NPC>();
    }

    private void RemoveNPCAtTable(GameObject table)
    {
        Table component = table.GetComponent<Table>();
        component.npcAtTable = null;
    }

    public void GenerateFoodOrder(int index)
    {
        
        string str = string.Empty;
        int num = 1;
        foodOrder.Clear();
        for (int i = 0; i < num; i++)
        {
            int num2;
            if (index > -1) {
                num2 = index;
            } else {
                num2 = Random.Range(0, Menu.Items.Length);
            }
            //int num2 = Random.Range(0, Menu.Items.Length);
            string empty = string.Empty;
            foodOrder.Add(Menu.ItemNames[num2]);
            if (mainComputer == null && GameObject.Find("!Monitor"))
            {
                mainComputer = GameObject.Find("!Monitor").GetComponent<Computer>();
            }
            if (mainComputer) {
                mainComputer.AddFoodToTable(tableNumber - 1, Menu.ItemNames[num2]);
            }
            empty = (i >= num - 1) ? ".\n" : ",\n";
            str = str + Menu.ItemNames[num2] + empty;
        }
        Transform transform = speechBubble = Instantiate(prefSpeech, base.transform.position + base.transform.up, base.transform.rotation);
        print(foodOrder[0].ToLower());
        speechBubble.GetChild(0).transform.GetComponent<MeshRenderer>().material = (Resources.Load("UI/Materials/" + foodOrder[0].ToLower()) as Material);
        GetSyncFoodOrder(gameObject, foodOrder[0]);
    }

    private bool matchPlateToOrder(List<Food> foodOnPlate)
    {
        return Menu.CompareAgainstFood(foodOrder[0], foodOnPlate[0]);
    }

    private void SpawnMoney(int maxMoney = 3)
    {
        int num = Random.Range(1, maxMoney + 1);
        for (int i = 0; i <= num; i++)
        {
            Transform transform = Instantiate(reward, base.transform.position + base.transform.up * 1.5f, base.transform.rotation);
            transform.GetComponent<Rigidbody>().AddForce(npcAtTable.transform.up * 700f + npcAtTable.transform.forward * 400f + Random.insideUnitSphere * 2f);
        }
    }

    float timer;
    GameObject plate;
    bool stopBool;

    private void OnTriggerEnter(Collider other) {
        if (SceneManager.GetSceneAt(0).buildIndex == 2) {
            FinishEating(other.gameObject);
        } else {
            if (foodOrder == null || foodOrder.Count <= 0 || !other.transform.Find("triggerPlate") || !other.transform.GetComponent<Rigidbody>() || !other.transform.GetComponent<Rigidbody>().useGravity) {
                return;
            }
            plate = other.gameObject;
            timer = 0;
        }
    }

    void FinishEating(GameObject other) {
        if (/*!Network.isServer || */foodOrder == null || foodOrder.Count <= 0 || !other.transform.Find("triggerPlate") || !other.transform.GetComponent<Rigidbody>() || !other.transform.GetComponent<Rigidbody>().useGravity) {
            return;
        }
        Plate component = other.transform.Find("triggerPlate").GetComponent<Plate>();

        if (component.foodOnPlate.Count > 0)
        {
            /*if (matchPlateToOrder(component.foodOnPlate))
            {
                StartCoroutine(NetworkSend.Send(component.transform.parent.GetComponent<PickupObject>().lastPlayerHolding.username, "OrdersCompleted", "1"));
            }*/
            float num = Menu.ScoreFood(foodOrder[0], component.foodOnPlate[0]);
            num = Mathf.Round(num * 1.3f);
            num *= 0.5f;
            if (Random.value > 0.8f)
            {
                num += 1f;
            }
            SpawnMoney(Mathf.RoundToInt(num));
            if (speechBubble != null)
            {
                DestroySpeechBubble();
            }
            npcAtTable.setWants(NPC.wants.toLeave);
            npcAtTable.inside = false;
            npcAtTable.FindExit(excludeNearestExit: false);
            npcAtTable.FollowNoPlayer();
            myTableNode.GetComponent<TableNodes>().SetOccupied(false);
            other.GetComponent<MeshRenderer>().material.SetFloat("_Blend", 1f);
            component.foodOnPlate.Clear();
            ClearOrder();
            npcAtTable = null;
            BurgerStacking[] componentsInChildren = other.transform.GetComponentsInChildren<BurgerStacking>();
            BurgerStacking[] array = componentsInChildren;
            foreach (BurgerStacking burgerStacking in array)
            {
                List<Food> foodOnBurger = burgerStacking.foodOnBurger;
                foreach (Food item in foodOnBurger)
                {
                    item.GetComponent<PickupObject>().DestroyObject();
                }
            }
            foreach (Transform item2 in other.transform)
            {
                if (item2.tag.Equals("PhysicsFood"))
                {
                    print(item2.gameObject.name);
                    item2.gameObject.GetComponent<PickupObject>().DestroyObject();
                }
            }
        }
    }
}
