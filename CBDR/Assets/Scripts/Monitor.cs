using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monitor : MonoBehaviour {
    public ComputerButton[] buttons;
    public GameObject[] pages;

    public Transform groupBurger;

    public RawImage rawImage;
    public Texture[] burgers;
    int burgerIndex;

    public RawImage[] imageTables;
    public Table[] tables;

    int pageIndex;
    bool stopBool;

    GameObject orderScript;
    void Start() {
        orderScript = GameObject.Find("OrderPoint");
        pages[pageIndex].SetActive(true);
        burgers = new Texture[Menu.ItemNames.Length];
        for (int i = 0; i < Menu.ItemNames.Length; i++) {
            burgers[i] = Resources.Load("UI/Transparent/" + Menu.ItemNames[i]) as Texture;
        }
    }

    void Update() {
        if (stopBool) {
            bool isPressed = false;
            for (int i = 0; i < buttons.Length; i++) {
                if (buttons[i].isPressed) {
                    isPressed = true;
                }
            }
            if (!isPressed) {
                stopBool = false;
            }
        } else {
            for (int i = 0; i < buttons.Length; i++) {
                if (buttons[i].isPressed) {
                    Pages(i);
                    stopBool = true;
                }
            }
        }
        pages[pageIndex].GetComponent<RectTransform>().localPosition = Vector3.Lerp(pages[pageIndex].GetComponent<RectTransform>().localPosition, Vector2.zero, 0.5f);

        burgerIndex = (int)Mathf.Repeat(burgerIndex, burgers.Length);
        rawImage.texture = burgers[burgerIndex];

        for (int i = 0; i < imageTables.Length; i++) {
            if (tables[i].foodOrder.ToArray().Length > 0) {
                for (int ib = 0; ib < Menu.ItemNames.Length; ib++) {
                    if (tables[i].foodOrder[0] == Menu.ItemNames[ib]) {
                        imageTables[i].texture = burgers[ib];
                    }
                }
            } else {
                imageTables[i].texture = null;
            }
        }
    }

    void Pages(int button) {
        Debug.Log("Test");
        foreach (GameObject page in pages) {
            page.SetActive(false);
        }
        Debug.Log(pageIndex);
        switch (pageIndex) {
            case 0:
                if (button == 0 || button == 3) {
                    ChangePages(1);
                }
                break;
            case 1:
                if (button == 0 || button == 3) {
                    ChangePages(0);
                }
                if (button == 2) {
                    ChangePages(2);
                }
                break;
            case 2:
                if (button == 0) {
                    ChangePages(4);
                }
                if (button == 3) {
                    ChangePages(3);
                }
                if (button == 2) {
                    ChangePages(5);
                }
                break;
            case 3: // Check
                if (button == 0) {
                    ChangePages(2);
                }
                if (button == 3) {
                    ChangePages(4);
                }
                if (button == 2) {
                    for (int i = 0; i < groupBurger.childCount; i++) {
                        if (orderScript.GetComponent<OrderScript>().npcs.Count >= i + 1) {
                            orderScript.GetComponent<OrderScript>().npcs[i].GetComponent<NPC>().askingForOrder = true;
                        }
                    }
                    for (int i = 0; i < groupBurger.childCount; i++) {
                        Destroy(groupBurger.GetChild(i).gameObject);
                    }
                    ChangePages(0);
                }
                break;
            case 4:
                if (button == 0) {
                    ChangePages(3);
                }
                if (button == 3) {
                    ChangePages(2);
                }
                if (button == 2) {
                    ChangePages(2);
                }
                break;
            case 5:
                if (button == 0 || button == 3) {
                    ChangePages(5);
                }
                if (button == 0) {
                    burgerIndex--;
                }
                if (button == 3) {
                    burgerIndex++;
                }
                if (button == 2) {
                    if (groupBurger.childCount < 4) {
                        GameObject image = new GameObject();
                        image.AddComponent<RawImage>().texture = burgers[burgerIndex];
                        image.transform.SetParent(groupBurger);
                        image.transform.localPosition = Vector3.zero;
                        image.transform.localRotation = Quaternion.identity;
                        image.transform.localScale = Vector3.one;
                        image.GetComponent<RectTransform>().sizeDelta = new Vector2(90, 90);
                    }
                    ChangePages(2);
                }
                break;
        }
        pages[pageIndex].SetActive(true);
    }

    void ChangePages(int index) {
        pageIndex = index;
        pages[pageIndex].GetComponent<RectTransform>().localPosition = new Vector3(0, transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.y);
    }
}
