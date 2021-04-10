using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour {
    public GameObject menuPause;
    public GameObject menuOptions;
    public GameObject saveLevelPanel;
    GameObject player;
    public bool isPause;

    GameObject rigidObjects;
    bool notepadUse;

    UserJson userJson = new UserJson();
    string path;

    void Start() {
        GameObject.Find("Spawn").GetComponent<SpawnPlayer>().Spawn();
        rigidObjects = GameObject.Find("Rigid-Elements");
    }
    void Update() {
        if (GameObject.Find("Player(Mine)") && !player) {
            player = GameObject.Find("Player(Mine)");
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isPause = !isPause;
            if (menuOptions.GetComponent<CanvasGroup>().alpha == 1) {
                MenuOptions(false);
            }
        }
        menuPause.GetComponent<CanvasGroup>().blocksRaycasts = isPause;

        for (int i = 0; i < rigidObjects.transform.childCount; i++) {
            if (rigidObjects.transform.GetChild(i).gameObject.name == "Notepad" && rigidObjects.transform.GetChild(i).GetComponent<ObjectUsable>().beingUsed) {
                notepadUse = true;
            }
        }

        if (player) {
            player.GetComponent<FirstPersonControl>().enabled = !isPause;
        }
        if (notepadUse) {
            Cursor.lockState = CursorLockMode.None;
            Camera.main.GetComponent<MouseLook>().canMouseMove = false;
        } else {
            if (isPause) {
                Cursor.lockState = CursorLockMode.None;
                menuPause.GetComponent<CanvasGroup>().alpha = 1;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
                menuPause.GetComponent<CanvasGroup>().alpha = 0;
            }
            Camera.main.GetComponent<MouseLook>().canMouseMove = !isPause;
        }
        Cursor.visible = isPause;
        notepadUse = false;
    }
    public void MenuOptions(bool open) {
        menuOptions.GetComponent<CanvasGroup>().blocksRaycasts = open;
        if (open) {
            menuOptions.GetComponent<CanvasGroup>().alpha = 1;
        } else {
            menuOptions.GetComponent<CanvasGroup>().alpha = 0;
        }
    }
    public void BackToMainMenu() {
        // Load User
        path = Application.persistentDataPath + "/user.json";
        if (File.Exists(path)) {
            string contents = File.ReadAllText(path);
            userJson = JsonUtility.FromJson<UserJson>(contents);
            
            if (userJson.autoSave) {
                gameObject.GetComponent<LevelScript>().SaveData();
                LoadLevel();
            } else {
                saveLevelPanel.SetActive(true);
            }
        }
    }
    public void LoadLevel() {
        if (userJson.themeIndex == 0) {
            SceneManager.LoadScene(0);
        } else {
            SceneManager.LoadScene(1);
        }
    }
    public void Continue() {
        isPause = false;
    }
}
