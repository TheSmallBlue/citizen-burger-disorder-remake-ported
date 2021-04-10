using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelFileScript : MonoBehaviour {
    public GameObject[] menus;
    public InputField inputFieldName;
    public string pathLevel;
    public LevelsSystem menuLevels;
    
    public Color colorOld;
    public Font fontOld;

    LevelJsonScript levelJson;

    void Start() {
        if (SceneManager.GetSceneAt(0).buildIndex == 1) {
            gameObject.GetComponent<Image>().color = colorOld;
            transform.GetChild(0).GetChild(0).GetComponent<Text>().color = Color.white;
            transform.GetChild(0).GetChild(0).GetComponent<Text>().font = fontOld;
            for (int i = 0; i < 3; i++) {
                ColorBlock colorBlock = transform.GetChild(0).GetChild(i + 1).GetComponent<Button>().colors;
                colorBlock.normalColor = Color.white;
                transform.GetChild(0).GetChild(i + 1).GetComponent<Button>().colors = colorBlock;
                transform.GetChild(0).GetChild(i + 1).GetChild(0).GetComponent<Image>().color = new Color(55f / 255, 55f / 255, 55f / 255);
            }
            for (int i = 0; i < 2; i++) {
                ColorBlock colorBlock = transform.GetChild(1).GetChild(i + 1).GetComponent<Button>().colors;
                colorBlock.normalColor = Color.white;
                transform.GetChild(1).GetChild(i + 1).GetComponent<Button>().colors = colorBlock;
            }
        }
    }

    void Update() {
        menus[0].GetComponent<CanvasGroup>().interactable = !menuLevels.panelDeleteBool;
    }

    public void Play() {
        string contents = File.ReadAllText(pathLevel);
        levelJson = JsonUtility.FromJson<LevelJsonScript>(contents);
        PlayerPrefs.SetString("LevelPath", pathLevel);
        if (levelJson.styleIndex == 0) {
            SceneManager.LoadScene(2);
        } else {
            SceneManager.LoadScene(3);
        }
    }

    public void SaveEditName() {
        string levelTexts;
        levelTexts = File.ReadAllText(pathLevel);
        System.DateTime dateTime;

        dateTime = File.GetCreationTime(pathLevel);
        File.Delete(pathLevel);
        pathLevel = pathLevel.Substring(0, pathLevel.Length - transform.GetChild(0).GetChild(0).GetComponent<Text>().text.Length - 5) + inputFieldName.text + ".json";

        File.WriteAllText(pathLevel, levelTexts);
        File.SetCreationTime(pathLevel, dateTime);

        transform.GetChild(0).GetChild(0).GetComponent<Text>().text = inputFieldName.text;
        MenuChanges(0);
    }

    public void MenuChanges(int index) {
        if (index == 2) {
            menuLevels.panelDeleteBool = true;
            menuLevels.getLevel = gameObject;
        } else {
            foreach(GameObject menu in menus) {
                menu.SetActive(false);
            }
            menus[index].SetActive(true);
        }
    }
}
