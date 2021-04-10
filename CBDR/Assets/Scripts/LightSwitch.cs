using UnityEngine;
public class LightSwitch : MonoBehaviour {
    public GameObject[] lights;
    public Material[] materials;
    public Transform lever;
    public bool switchBool = true;
    float floatEulerAngles;
    float timerStop;
    void Update() {
        timerStop = Mathf.Max(timerStop - Time.deltaTime, 0);
        foreach (GameObject objects in lights) {
            objects.GetComponent<Light>().enabled = switchBool;
            if (switchBool) {
                objects.GetComponent<MeshRenderer>().material = materials[1];
            } else {
                objects.GetComponent<MeshRenderer>().material = materials[0];
            }
        }
        if (switchBool) {
            floatEulerAngles = Mathf.Lerp(floatEulerAngles, 45, 0.4f);
        } else {
            floatEulerAngles = Mathf.Lerp(floatEulerAngles, -45, 0.4f);
        }
        lever.localEulerAngles = new Vector3(0, 0, floatEulerAngles);
    }

    public void Switch() {
        if (timerStop == 0) {
            switchBool = !switchBool;
        }
        timerStop = 0.1f;
    }
}
