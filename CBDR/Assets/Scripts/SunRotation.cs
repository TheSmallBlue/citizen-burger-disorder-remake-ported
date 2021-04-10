using UnityEngine;
public class SunRotation : MonoBehaviour {
    public float speed = 1;
    float originalIntensity;
    float rotation;
    float lightChanges;
    float temperatureChanges;
    void Start() {
        originalIntensity = gameObject.GetComponent<Light>().intensity;
    }
    void Update() {
        rotation = (rotation + (Time.deltaTime * speed)) % 360;
        transform.eulerAngles = new Vector3(rotation, -30, 0);
        if (rotation > 180) {
            lightChanges = 0;
        } else {
            lightChanges = originalIntensity;
        }
        if (rotation > 15 && rotation < 135) {
            temperatureChanges = 6000;
        } else {
            temperatureChanges = 1000;
        }
        gameObject.GetComponent<Light>().intensity = Mathf.MoveTowards(gameObject.GetComponent<Light>().intensity, lightChanges, 0.01f * speed);
        gameObject.GetComponent<Light>().colorTemperature = Mathf.MoveTowards(gameObject.GetComponent<Light>().colorTemperature, temperatureChanges, 5 * speed);
    }
}
