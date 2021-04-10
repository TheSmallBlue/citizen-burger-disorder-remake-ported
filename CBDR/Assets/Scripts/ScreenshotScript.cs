using System.IO;
using UnityEngine;
public class ScreenshotScript : MonoBehaviour {
    string path;
    void Start() {
        path = Directory.GetParent(Application.dataPath).ToString() + "\\Screenshots";
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            ScreenCapture.CaptureScreenshot(ScreenshotName());
        }
    }
		
	public string ScreenshotName() {
		string strPath = string.Format("{0}/screen_{1}.png", path, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
		return strPath;
	}
}
