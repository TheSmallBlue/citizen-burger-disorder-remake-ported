using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;

public class OptionsScript : MonoBehaviour {
    public Slider sliderVolume;
    public Dropdown dropdownQuality;
    public Dropdown dropdownResolution;
    Resolution[] res;
    public Toggle toggleFullScreen;
    public Toggle toggleAutoSave;

    public int themeIndex;
    public static string skinFile;
    public Slider[] sliders;

    UserJson userJson = new UserJson();
    string path;

    void Start() {
        dropdownQuality.ClearOptions();
        dropdownQuality.AddOptions(QualitySettings.names.ToList());
        dropdownQuality.value = QualitySettings.GetQualityLevel();

        Resolution[] resolution = Screen.resolutions;
        res = resolution.Distinct().ToArray();
        string[] strRes = new string[res.Length];
        for (int i = 0; i < res.Length; i++)
        {
            strRes[i] = res[i].width.ToString() + "x" + res[i].height.ToString();
        }
        dropdownResolution.ClearOptions();
        dropdownResolution.AddOptions(strRes.ToList());

        // Load User
        path = Application.persistentDataPath + "\\user.json";
        if (File.Exists(path)) {
            string contents = File.ReadAllText(path);
            userJson = JsonUtility.FromJson<UserJson>(contents);
            sliderVolume.value = userJson.volume;
            if (toggleAutoSave) {
                toggleAutoSave.isOn = userJson.autoSave;
            }
            themeIndex = userJson.themeIndex;
            skinFile = userJson.skin;
            if (sliders.Length > 0) {
                sliders[0].value = userJson.armColor.r;
                sliders[1].value = userJson.armColor.g;
                sliders[2].value = userJson.armColor.b;
                sliders[3].value = userJson.gloveColor.r;
                sliders[4].value = userJson.gloveColor.g;
                sliders[5].value = userJson.gloveColor.b;
            }
        }
    }

    void Update() {
        AudioListener.volume = sliderVolume.value;

        // Save User
        userJson.volume = sliderVolume.value;
        if (toggleAutoSave) {
            userJson.autoSave = toggleAutoSave.isOn;
        }
        userJson.themeIndex = themeIndex;
        userJson.skin = skinFile;
        if (sliders.Length > 0) {
            userJson.armColor = new Color(sliders[0].value, sliders[1].value, sliders[2].value);
            userJson.gloveColor = new Color(sliders[3].value, sliders[4].value, sliders[5].value);
        }
        string contents = JsonUtility.ToJson(userJson, true);
        File.WriteAllText(path, contents);
    }
    
	public void SetQuality () {
        QualitySettings.SetQualityLevel(dropdownQuality.value);
	}

    public void SetResolution() {
        Screen.SetResolution(res[dropdownResolution.value].width, res[dropdownResolution.value].height, toggleFullScreen.isOn);
    }
}
