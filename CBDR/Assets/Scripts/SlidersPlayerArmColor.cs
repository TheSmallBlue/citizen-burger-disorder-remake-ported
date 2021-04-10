using UnityEngine;
using UnityEngine.UI;
public class SlidersPlayerArmColor : MonoBehaviour {
    public MeshRenderer[] meshRenderers;
    public Slider[] sliders;
    void Update() {
        meshRenderers[0].material.color = new Color(sliders[0].value, sliders[1].value, sliders[2].value);
        meshRenderers[1].material.color = new Color(sliders[3].value, sliders[4].value, sliders[5].value);
    }
}
