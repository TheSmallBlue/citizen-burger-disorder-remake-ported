using UnityEngine;
using UnityEngine.UI;
public class TimeControl : MonoBehaviour {
    float timeSpeed = 1;
    float timeLerp;
    float alphaColor;
    float timer;
    void Update() {
        if (!GameObject.Find("Canvas").GetComponent<MenuManager>().isPause) {
            if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
                timeSpeed /= 2;
                timer = 3;
            }
            if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
                timeSpeed *= 2;
                timer = 3;
            }
            timeSpeed = Mathf.Clamp(timeSpeed, 0.125f, 8);
            timeLerp = Mathf.Lerp(timeLerp, timeSpeed, Time.unscaledDeltaTime * 2);
            timer -= Time.unscaledDeltaTime;
            alphaColor = Mathf.Clamp(alphaColor, 0, 1);
            if (timer > 0) {
                alphaColor += Time.unscaledDeltaTime * 10;
            } else {
                alphaColor -= Time.unscaledDeltaTime * 10;
            }
            gameObject.GetComponent<Text>().color = new Color(1, 1, 1, alphaColor);
            gameObject.GetComponent<Text>().text = "Time: " + timeLerp.ToString("0.000");
            Time.timeScale = timeLerp;
        }
    }
}
