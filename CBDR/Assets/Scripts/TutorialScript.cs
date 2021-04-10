using UnityEngine;
public class TutorialScript : MonoBehaviour {
    public bool isSkipped;
    bool isShowing;
    float alpha;
    float timer = 1;
    bool stopBool;
    void Update() {
        if (!isSkipped) {
            if (!stopBool) {
                timer -= 0.01f;
                if (timer <= 0) {
                    isShowing = true;
                    stopBool = true;
                }
            }
            if (Input.GetMouseButtonDown(0)) {
                isShowing = false;
                isSkipped = true;
            }
        }
        if (isShowing) {
            alpha += Time.unscaledDeltaTime * 2;
        } else {
            alpha -= Time.unscaledDeltaTime * 2;
        }
        alpha = Mathf.Clamp(alpha, 0, 1);
        gameObject.GetComponent<CanvasGroup>().alpha = alpha;
    }
}
