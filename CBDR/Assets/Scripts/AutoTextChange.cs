using UnityEngine;
using UnityEngine.UI;
public class AutoTextChange : MonoBehaviour {
    public Text textVersion;
    void Start() {
        textVersion.text = "v1.6";
    }
}
