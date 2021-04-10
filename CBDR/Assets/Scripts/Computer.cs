using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Computer : MonoBehaviour
{
    public static List<Computer> computers;

    private GScreen screen;

    private GInterface[] menus = new GInterface[2];

    private INavigation[] navBar = new INavigation[1];

    private GInterface[] tables = new GInterface[4];

    private string buttonName = string.Empty;

    private GInterface editingInterface;

    private int editingInterfaceTableIndex;

    private Rect editingInterfacePreviousBounds;

    private bool initDone;

    private bool clientInitDone;

    public bool portable;

    private float lastTruckSpawnTime;

    private float truckSpawnDelay = 60f;

    public Material mPat;

    public Material mCedric;

    public Material mGrace;

    public GameObject truckPrefab;

    public CashRegister cashRegister;

    private void Awake()
    {
        screen = GetComponent<GScreen>();
        screen.computer = this;
        cashRegister = GameObject.Find("!Register").transform.Find("RegisterTrigger").GetComponent<CashRegister>();
        if ((bool)GetComponent<ObjectUsable>())
        {
            portable = true;
        }
    }

    private void Init()
    {
        for (int i = 0; i <= 4; i++)
        {
            if (i < 3)
            {
                tables[i] = screen.CreateInterface(16f + (float)i * 158f, 5f, 0.186f, 0.78f);
                tables[i].backgroundColor = Color.white;
                GElement gElement = tables[i].CreateElement(-1f, -1f, 1.02f, 0.16f, string.Empty + (i + 1), store: false);
                gElement.textSize = 60;
                gElement.textColor = Color.white;
                if (SceneManager.GetSceneAt(0).buildIndex == 2) {
                    tables[i].zLayer = 0.07f;
                    gElement.color = Color.blue;
                    gElement.zLayer = 0.085f;
                } else {
                    tables[i].zLayer = 0.01f;
                    gElement.color = new Color(0, 0.5f, 1);
                    gElement.zLayer = 0.016f;
                }
                for (int j = 0; j < 2; j++)
                {
                    GElement gElement2 = tables[i].CreateButton(15f, 70 + j % 2 * 140, 0.8f, 0.35f, string.Empty);
                    if (SceneManager.GetSceneAt(0).buildIndex == 2) {
                        gElement2.zLayer = 0.09f;
                    } else {
                        gElement2.zLayer = 0.016f;
                    }
                    gElement2.SetColor(new Color(1f, 0.85f, 0.85f, 0.05f), "highlight");
                    gElement2.SetColor(new Color(1f, 0.7f, 0.7f, 0.5f), "pressed");
                    gElement2.SetUseable(active: false);
                }
            }
            else if (i == 3)
            {
                tables[i] = screen.CreateInterface(16f + (float)i * 158f, 5f, 0.372f, 0.78f);
                tables[i].backgroundColor = Color.white;
                tables[i].zLayer = 0.01f;
                GElement gElement3 = tables[i].CreateElement(-1f, -1f, 1.01f, 0.16f, string.Empty + (i + 1), store: false);
                gElement3.textSize = 60;
                gElement3.color = new Color(0, 0.6f, 1);
                gElement3.textColor = Color.white;
                gElement3.zLayer = 0.016f;
                if (SceneManager.GetSceneAt(0).buildIndex == 2) {
                    tables[i].zLayer = 0.07f;
                    gElement3.color = Color.blue;
                    gElement3.zLayer = 0.085f;
                } else {
                    tables[i].zLayer = 0.01f;
                    gElement3.color = new Color(0, 0.5f, 1);
                    gElement3.zLayer = 0.016f;
                }
                for (int k = 0; k < 4; k++)
                {
                    GElement gElement2 = tables[i].CreateButton(15 + ((k >= 2) ? 1 : 0) * 140, 70 + k % 2 * 140, 0.4f, 0.35f, string.Empty);
                    if (SceneManager.GetSceneAt(0).buildIndex == 2) {
                        gElement2.zLayer = 0.09f;
                    } else {
                        gElement2.zLayer = 0.016f;
                    }
                    gElement2.SetColor(new Color(1f, 0.85f, 0.85f, 0.05f), "highlight");
                    gElement2.SetColor(new Color(1f, 0.7f, 0.7f, 0.5f), "pressed");
                    gElement2.SetUseable(active: false);
                }
            }
        }
        menus[0] = screen.CreateInterface(5f, 5f, 0.9875f, 0.8f);
        menus[1] = screen.CreateInterface(5f, 5f, 0.9875f, 0.8f);
        for (int l = 0; l < 3; l++)
        {
            string empty = string.Empty;
            Color black = Color.black;
            switch (l)
            {
                default:
                    empty = "Meat!";
                    black = Color.red;
                    break;
                case 1:
                    empty = "Bread!";
                    black = new Color(32f / 51f, 32f / 45f, 0f);
                    break;
                case 2:
                    empty = "Produce! Cheese‽";
                    black = new Color(0f, 32f / 51f, 0f);
                    break;
            }
            GElement gElement2 = menus[1].CreateElement(28 * (l + 1) + 221 * l, 25f, 0.28f, 0.7f, empty);
            gElement2.SetColor(new Color(1f, 1f, 1f), string.Empty);
            gElement2.textSize = 30;
            gElement2.textColor = black;
            gElement2.transform.GetChild(0).transform.localScale += new Vector3(-0.6f, -0.6f, 0f);
            gElement2.transform.GetChild(0).transform.position += new Vector3(0f, -0.07f, 0f);
            gElement2.zLayer = 0.06f;
        }
        for (int m = 0; m < 3; m++)
        {
            GElement gElement2 = menus[1].CreateElement(28 * (m + 1) + 221 * m, 60f, 0.28f, 0.6f, string.Empty);
            switch (m)
            {
                default:
                    gElement2.gameObject.GetComponent<MeshRenderer>().material = mPat;
                    break;
                case 1:
                    gElement2.gameObject.GetComponent<MeshRenderer>().material = mCedric;
                    break;
                case 2:
                    gElement2.gameObject.GetComponent<MeshRenderer>().material = mGrace;
                    break;
            }
            gElement2.SetColor(new Color(1f, 1f, 1f), string.Empty);
            gElement2.zLayer = 0.11f;
        }
        for (int n = 0; n < 3; n++)
        {
            string empty2 = string.Empty;
            switch (n)
            {
                default:
                    empty2 = "Patt Paddington";
                    break;
                case 1:
                    empty2 = "Seedy Cedric";
                    break;
                case 2:
                    empty2 = "Green Grace";
                    break;
            }
            GElement gElement2 = menus[1].CreateButton(28 * (n + 1) + 221 * n, 280f, 0.28f, 0.2f, empty2);
            gElement2.SetColor(new Color(1f, 1f, 1f, 1f), string.Empty);
            gElement2.SetColor(new Color(0.5f, 1f, 0.5f, 0.98f), "highlight");
            gElement2.textColor = new Color(0f, 128f / 225f, 1f);
            gElement2.textSize = 60;
            gElement2.transform.GetChild(0).transform.localScale += new Vector3(-0.8f, 0f, 0f);
            gElement2.transform.GetChild(0).transform.position += new Vector3(0f, -0.15f, 0f);
            gElement2.zLayer = 0.08f;
        }
        menus[1].gameObject.SetActive(value: false);
        navBar[0] = screen.CreateNavigation(5f, 370f, 0.9875f, 0.16f);
        for (int num = 0; num <= 1; num++)
        {
            string text = string.Empty;
            switch (num)
            {
                case 0:
                    text = "ORDERS";
                    break;
                case 1:
                    text = "DELIVERY";
                    break;
            }
            GElement gElement2 = navBar[0].CreateButton(45 + num * 380, 5f, 0.38f, 0.8f, text);
            gElement2.transform.GetChild(0).transform.localScale += new Vector3(-0.3f, 1.6f, 0f);
            gElement2.textColor = Color.black;
            gElement2.textSize = 26;
        }
        if (computers == null)
        {
            computers = new List<Computer>();
        }
        computers.Add(this);
        initDone = true;
    }

    public GElement GetTableElement(int tableNumber, int elementNumber)
    {
        if (tableNumber < 3 && elementNumber > 1)
        {
            elementNumber = 1;
        }
        if (tableNumber > 3 && elementNumber > 3)
        {
            elementNumber = 3;
        }
        return tables[tableNumber--].graphicElements[elementNumber--];
    }

    private int GetTableElementIndex(int tableNumber, int elementNumber)
    {
        tableNumber = Mathf.Min(tableNumber--, 0);
        elementNumber = Mathf.Min(elementNumber--, 0);
        if (tableNumber < 3 && elementNumber > 1)
        {
            return tableNumber * 2 + 1;
        }
        if (tableNumber > 3 || elementNumber > 3)
        {
            return 9;
        }
        return tableNumber * 2 + elementNumber;
    }

    public void SetButtonDown(string name)
    {
        buttonName = name;
    }

    private bool GetButtonDown(string name)
    {
        return buttonName == name;
    }

    public void SetEditingInterface(int tableIndex)
    {
        if (tableIndex == -1)
        {
            editingInterface = null;
            return;
        }
        editingInterface = tables[tableIndex];
        editingInterfacePreviousBounds = editingInterface.bounds;
        print("old bounds: " + editingInterface.bounds);
    }

    public void SetMenusActive(bool active, int id)
    {
        menus[id].gameObject.SetActive(active);
    }

    public void SetTablesActive(bool active)
    {
        for (int i = 0; i < tables.Length; i++)
        {
            tables[i].gameObject.SetActive(active);
        }
    }

    /*private void OnPlayerConnected(NetworkPlayer player)
    {
        base.networkView.RPC("ClientInit", RPCMode.Others);
        for (int i = 0; i < menus.Length; i++)
        {
            base.networkView.RPC("SyncNetworkViewID", RPCMode.Others, "menus", i, menus[i].networkView.viewID);
            foreach (GElement graphicElement in menus[i].graphicElements)
            {
                base.networkView.RPC("SyncElementNetworkViewID", RPCMode.Others, "menus", i, menus[i].graphicElements.IndexOf(graphicElement), graphicElement.networkView.viewID);
            }
            base.networkView.RPC("SyncMenuBounds", RPCMode.Others, "menus", i, menus[i].bounds.x, menus[i].bounds.y, menus[i].bounds.width, menus[i].bounds.height, menus[i].gameObject.activeSelf);
        }
        for (int j = 0; j < navBar.Length; j++)
        {
            base.networkView.RPC("SyncNetworkViewID", RPCMode.Others, "navBar", j, navBar[j].networkView.viewID);
            foreach (GElement graphicElement2 in navBar[j].graphicElements)
            {
                base.networkView.RPC("SyncElementNetworkViewID", RPCMode.Others, "navBar", j, navBar[j].graphicElements.IndexOf(graphicElement2), graphicElement2.networkView.viewID);
            }
            base.networkView.RPC("SyncMenuBounds", RPCMode.Others, "navBar", j, navBar[j].bounds.x, navBar[j].bounds.y, navBar[j].bounds.width, navBar[j].bounds.height, navBar[j].gameObject.activeSelf);
        }
        for (int k = 0; k < tables.Length; k++)
        {
            base.networkView.RPC("SyncNetworkViewID", RPCMode.Others, "tables", k, tables[k].networkView.viewID);
            foreach (GElement graphicElement3 in tables[k].graphicElements)
            {
                base.networkView.RPC("SyncElementNetworkViewID", RPCMode.Others, "tables", k, tables[k].graphicElements.IndexOf(graphicElement3), graphicElement3.networkView.viewID);
            }
            base.networkView.RPC("SyncMenuBounds", RPCMode.Others, "tables", k, tables[k].bounds.x, tables[k].bounds.y, tables[k].bounds.width, tables[k].bounds.height, tables[k].gameObject.activeSelf);
            GameObject gameObject;
            if ((bool)(gameObject = tables[k].transform.FindChild("!Element(Clone)").gameObject))
            {
                base.networkView.RPC("SyncGameObjectID", RPCMode.Others, "tables", k, "!Element(Clone)", gameObject.networkView.viewID);
            }
            foreach (GElement graphicElement4 in tables[k].graphicElements)
            {
                if (graphicElement4.text.Contains("REMOVE_"))
                {
                    graphicElement4.networkView.RPC("SetText", RPCMode.Others, graphicElement4.text, graphicElement4.IsTextHidden());
                    graphicElement4.networkView.RPC("SetMaterialToFood", RPCMode.Others, graphicElement4.networkView.viewID, graphicElement4.text.Substring(9));
                }
            }
        }
        base.networkView.RPC("SetEditingInterface", RPCMode.Others, editingInterfaceTableIndex);
    }*/

    private void ClientInit()
    {
        if (!clientInitDone)
        {
            Init();
            print("client init done!");
            clientInitDone = true;
        }
    }

    /*private void SyncGameObjectID(string arrayName, int arrayIndex, string gameobjectName, NetworkViewID NID)
    {
        tables[arrayIndex].transform.Find("!Element(Clone)").GetComponent<NetworkView>().viewID = NID;
    }

    private void SyncElementNetworkViewID(string arrayName, int arrayIndex, int elementIndex, NetworkViewID NID)
    {
        if (arrayName == "menus")
        {
            menus[arrayIndex].graphicElements[elementIndex].GetComponent<NetworkView>().viewID = NID;
        }
        if (arrayName == "navBar")
        {
            navBar[arrayIndex].graphicElements[elementIndex].GetComponent<NetworkView>().viewID = NID;
        }
        if (arrayName == "tables")
        {
            tables[arrayIndex].graphicElements[elementIndex].GetComponent<NetworkView>().viewID = NID;
        }
    }

    private void SyncNetworkViewID(string arrayName, int arrayIndex, NetworkViewID NID)
    {
        if (arrayName == "menus")
        {
            menus[arrayIndex].GetComponent<NetworkView>().viewID = NID;
        }
        if (arrayName == "navBar")
        {
            navBar[arrayIndex].GetComponent<NetworkView>().viewID = NID;
        }
        if (arrayName == "tables")
        {
            tables[arrayIndex].GetComponent<NetworkView>().viewID = NID;
        }
    }*/

    private void SetArrayIndexToInterface(string arrayName, int arrayIndex, string elementNID)
    {
        if (name == "menus")
        {
            menus[arrayIndex] = GameObject.Find(elementNID).GetComponent<GInterface>();
        }
        if (name == "navBar")
        {
            navBar[arrayIndex] = GameObject.Find(elementNID).GetComponent<INavigation>();
        }
        if (name == "tables")
        {
            tables[arrayIndex] = GameObject.Find(elementNID).GetComponent<GInterface>();
        }
    }

    private void SyncMenuBounds(string name, int id, float x, float y, float w, float h, bool active)
    {
        if (name == "menus")
        {
            menus[id].bounds.x = x;
            menus[id].bounds.y = y;
            menus[id].bounds.width = w;
            menus[id].bounds.height = h;
            menus[id].gameObject.SetActive(active);
        }
        if (name == "navBar")
        {
            navBar[id].bounds.x = x;
            navBar[id].bounds.y = y;
            navBar[id].bounds.width = w;
            navBar[id].bounds.height = h;
            navBar[id].gameObject.SetActive(active);
        }
        if (name == "tables")
        {
            tables[id].bounds.x = x;
            tables[id].bounds.y = y;
            tables[id].bounds.width = w;
            tables[id].bounds.height = h;
            tables[id].gameObject.SetActive(active);
        }
    }

    public void AddFoodToTable(int table, string food)
    {
        int num = 0;
        while (true)
        {
            if (num < tables[table].graphicElements.Count)
            {
                if (tables[table].graphicElements[num].text == string.Empty || num == tables[table].graphicElements.Count - 1)
                {
                    break;
                }
                num++;
                continue;
            }
            return;
        }
        tables[table].graphicElements[num].SetMaterialToFood(tables[table].graphicElements[num].gameObject, food);
        tables[table].graphicElements[num].SetText("REMOVE_" + num + "_" + food, false);
    }

    public void ClearFoodFromTable(int table, string food)
    {
        int num = 0;
        while (true)
        {
            if (num < tables[table].graphicElements.Count)
            {
                if (tables[table].graphicElements[num].text.Contains(food))
                {
                    break;
                }
                num++;
                continue;
            }
            return;
        }
        tables[table].graphicElements[num].ResetMaterial(tables[table].graphicElements[num].gameObject);
        tables[table].graphicElements[num].SetText(string.Empty, true);
    }

    private void Update()
    {
        if (!initDone)
        {
            initDone = true;
            Init();
        }
        if (!initDone && !clientInitDone)
        {
            return;
        }
        if (!buttonName.Equals(string.Empty))
        {
            MonoBehaviour.print("Pressed '" + buttonName + "'");
        }
        if (GetButtonDown("ORDERS"))
        {
            SetTablesActive(true);
            SetMenusActive(true, 0);
            SetMenusActive(false, 1);
        }
        if (GetButtonDown("DELIVERY"))
        {
            SetTablesActive(false);
            SetMenusActive(false, 0);
            SetMenusActive(true, 1);
        }
        if ((buttonName.Contains("Patt Paddington") || buttonName.Contains("Seedy Cedric") || buttonName.Contains("Green Grace")) && (lastTruckSpawnTime == 0f || Time.time > truckSpawnDelay + lastTruckSpawnTime))
        {
            GameObject gameObject = Instantiate(truckPrefab, GameObject.Find("!TruckSPAWN").transform.position, Quaternion.identity * Quaternion.Euler(0f, 270f, 0f));
            int num = 0;
            switch (buttonName)
            {
                case "Patt Paddington":
                    num = 0;
                    break;
                case "Seedy Cedric":
                    num = 1;
                    break;
                case "Green Grace":
                    num = 2;
                    break;
                default:
                    num = 0;
                    break;
            }
            cashRegister.RefreshDisplay(-20f);
            gameObject.GetComponent<TruckDriving>().contents = num;
            lastTruckSpawnTime = Time.time;
        }
        if (buttonName.Contains("ADD_"))
        {
            string text = buttonName.Substring(4);
            for (int i = 0; i < editingInterface.graphicElements.Count; i++)
            {
                if (editingInterface.graphicElements[i].text == string.Empty || i == editingInterface.graphicElements.Count - 1)
                {
                    editingInterface.graphicElements[i].SetText("REMOVE_" + i + "_" + text, false);
                    editingInterface.graphicElements[i].SetMaterialToFood(editingInterface.graphicElements[i].gameObject, text);
                    break;
                }
            }
        }
        if (buttonName.Contains("REMOVE_"))
        {
            print(buttonName + " ->> " + buttonName.Substring(7, 1));
            int index = int.Parse(buttonName.Substring(7, 1));
            editingInterface.graphicElements[index].SetText(string.Empty, true);
            editingInterface.graphicElements[index].ResetMaterial(editingInterface.graphicElements[index].gameObject);
        }
        SetButtonDown(string.Empty);
    }
}
