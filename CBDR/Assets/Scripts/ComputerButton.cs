using UnityEngine;
public class ComputerButton : MonoBehaviour {
    bool isPressed;
    void Update() {
        if (isPressed) {
            transform.GetChild(0).localScale = Vector3.Lerp(transform.GetChild(0).localScale, new Vector3(1, 0.5f, 1), Time.deltaTime * 10);
        } else {
            transform.GetChild(0).localScale = Vector3.Lerp(transform.GetChild(0).localScale, Vector3.one, Time.deltaTime * 10);
        }
    }
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "hand") {
            isPressed = true;
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.gameObject.name == "hand") {
            isPressed = false;
        }
    }
}
