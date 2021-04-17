using UnityEngine;
public class ShowNodes : MonoBehaviour {
    bool isShowingNodes;
    void Update() {
        if (Input.GetKeyDown(KeyCode.F3) ) {
            isShowingNodes = !isShowingNodes;
        }
        if (gameObject.name == "SpectatorMode") {
            GetComponent<CanvasGroup>().alpha = isShowingNodes.GetHashCode();
        } else {
            GetComponent<MeshRenderer>().enabled = isShowingNodes;
        }
    }
}
