using System.IO;
using UnityEngine;
public class SkinSetPlayer : MonoBehaviour {
    public Part part;
    UserJson userJson = new UserJson();
    string path;
    public enum Part {
        Body,
        Arm
    }
    void Start() {
        // Load User
        path = Application.persistentDataPath + "\\user.json";
        if (File.Exists(path)) {
            string contents = File.ReadAllText(path);
            userJson = JsonUtility.FromJson<UserJson>(contents);
            
            if (part == Part.Body) {
                string folderSkins = Directory.GetParent(Application.dataPath).ToString() + "\\Skins\\";
                if (File.Exists(folderSkins + userJson.skin)) {
                    Texture2D textureSkin = new Texture2D(0, 0);
                    textureSkin.LoadImage(File.ReadAllBytes(folderSkins + userJson.skin));
                    gameObject.GetComponent<MeshRenderer>().material.mainTexture = textureSkin;
                }
            }
            if (part == Part.Arm) {
                gameObject.GetComponent<MeshRenderer>().material.color = userJson.armColor;
                transform.GetChild(0).GetComponent<MeshRenderer>().material.color = userJson.gloveColor;
            }
        }
        if (part == Part.Body) {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
