using System.Collections.Generic;
using UnityEngine;
public class OrderScript : MonoBehaviour {
    public GameObject speechBubble;
    public List<GameObject> npcs;
    GameObject instantiateSpeechBubble;

    void Start() {
        npcs = new List<GameObject>();
    }

    void Update() {
        if (npcs.ToArray().Length > 0) {
            instantiateSpeechBubble.transform.position = npcs[0].transform.position + new Vector3(0, 5, 0);
            instantiateSpeechBubble.SetActive((GameObject.FindGameObjectWithTag("Player").transform.position - npcs.ToArray()[0].transform.position).magnitude < 20);
            if (instantiateSpeechBubble) {
                if (npcs[0].GetComponent<NPC>().askingForOrder) {
                    instantiateSpeechBubble.transform.GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load("UI/Materials/order") as Material;
                } else {
                    instantiateSpeechBubble.transform.GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load("UI/Materials/" + Menu.ItemNames[npcs[0].GetComponent<NPC>().burgerIndex]) as Material;
                }
            }
        } else {
            if (instantiateSpeechBubble) {
                instantiateSpeechBubble.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.GetComponent<NPC>() && other.GetComponent<NPC>().currentlyWants == NPC.wants.toOrder) {
            npcs.Add(other.gameObject);
            if (npcs.ToArray().Length == 1) {
                instantiateSpeechBubble = Instantiate(speechBubble);
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.GetComponent<NPC>()) {
            for (int i = 0; i < npcs.ToArray().Length; i++) {
                if (npcs.ToArray()[i] == other.gameObject) {
                    npcs.RemoveAt(i);
                }
            }
        }
    }
}
