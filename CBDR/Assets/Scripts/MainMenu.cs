using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour {
    public GameObject[] cameras;
    public GameObject[] menus;

    public Transform group;
    public GameObject buttonSkinPrefab;
    string[] files;
    string folderSkinsPath;
    public OptionsScript optionsScript;

    UserJson userJson = new UserJson();
    string path;

    void Start() {
        folderSkinsPath = Directory.GetParent(Application.dataPath).ToString() + "\\Skins";
        if (!Directory.Exists(folderSkinsPath)) {
            Directory.CreateDirectory(folderSkinsPath);
        }

        // Load User
        path = Application.persistentDataPath + "\\user.json";
        if (File.Exists(path)) {
            string contents = File.ReadAllText(path);
            userJson = JsonUtility.FromJson<UserJson>(contents);
            if (userJson.themeIndex == 1 && SceneManager.GetSceneAt(0).buildIndex != 1) {
                SceneManager.LoadScene(1);
            }
        }
        Time.timeScale = 1;
    }

    void LoadSkins() {
        files = Directory.GetFiles(folderSkinsPath);
        for (int i = 0; i < files.Length; i++) {
            if (files[i].Contains(".png") || files[i].Contains(".jpeg")) {
                GameObject buttonPrefab = Instantiate(buttonSkinPrefab, group);
                // Text File Load
                buttonPrefab.transform.GetChild(0).GetComponent<Text>().text = files[i].Substring(folderSkinsPath.Length + 1, files[i].Length - folderSkinsPath.Length - 5);
                // Texture Load
                Texture2D textureSkin = new Texture2D(0, 0);
                textureSkin.LoadImage(File.ReadAllBytes(files[i]));
                buttonPrefab.transform.GetChild(1).GetComponent<RawImage>().texture = textureSkin;
                //buttonPrefab.GetComponent<ButtonSkin>().url = files[i];
                buttonPrefab.GetComponent<ButtonSkin>().skinName = files[i].Substring(folderSkinsPath.Length + 1, files[i].Length - folderSkinsPath.Length - 1);
            }
        }
    }

    void CleanButtons() {
        for (int i = 0; i < group.childCount; i++) {
            Destroy(group.GetChild(i).gameObject);
        }
    }

    public void Exit() {
        Application.Quit();
    }

    public void ManuChanges(int index) {
        for (int i = 0; i < menus.Length; i++) {
            if (menus[i].GetComponent<CanvasGroup>()) {
                menus[i].GetComponent<CanvasGroup>().blocksRaycasts = false;
                menus[i].GetComponent<CanvasGroup>().alpha = 0;
            } else {
                menus[i].SetActive(false);
            }
        }
        if (menus[index].GetComponent<CanvasGroup>()) {
            menus[index].GetComponent<CanvasGroup>().blocksRaycasts = true;
            menus[index].GetComponent<CanvasGroup>().alpha = 1;
        } else {
            menus[index].SetActive(true);
        }
        if (index == 1) {
            cameras[0].SetActive(false);
            cameras[1].SetActive(true);
            LoadSkins();
        } else {
            CleanButtons();
            cameras[0].SetActive(true);
            cameras[1].SetActive(false);
        }
    }

    public void PlayScene(int index) {
        if (index == 0 || index == 1) {
            optionsScript.themeIndex = index;
            string contents = JsonUtility.ToJson(userJson, true);
            File.WriteAllText(path, contents);
        }
        SceneManager.LoadScene(index);
    }
}
