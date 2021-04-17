using UnityEngine;
public class AutoLightQuality : MonoBehaviour {
    void Update() {
        if (QualitySettings.GetQualityLevel() > 1) {
            GetComponent<Light>().renderMode = LightRenderMode.ForcePixel;
        } else {
            GetComponent<Light>().renderMode = LightRenderMode.Auto;
        }
    }
}
