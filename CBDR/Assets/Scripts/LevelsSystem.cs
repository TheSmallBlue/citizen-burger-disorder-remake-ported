using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelsSystem : MonoBehaviour {
    public Transform levelMenu;
    public GameObject prefab;
    public GameObject panelDelete;
    string path;
    string[] levels;

    public Image[] checkedStyle;
    public Color originalColor;
    public int indexStyle;
    public InputField inputFieldNameLevel;
    public Button buttonCreate;

    public GameObject getLevel;
    public bool panelDeleteBool;

    void Start() {
        SelectStyle(0);
    }

    void Update() {
        panelDelete.SetActive(panelDeleteBool);
    }

    void OnEnable() {
        // Load Level Files
        path = Directory.GetParent(Application.dataPath).ToString() + "\\Levels";
        if (Directory.Exists(path)) {
            levels = Directory.GetFiles(path);
            for (int i = 0; i < levelMenu.childCount; i++) {
                Destroy(levelMenu.GetChild(i).gameObject);
            }
            for (int i = 0; i < levels.Length; i++) {
                if (levels[i].Contains(".json")) {
                    GameObject level = Instantiate(prefab, levelMenu);
                    level.GetComponent<LevelFileScript>().pathLevel = levels[i];
                    level.GetComponent<LevelFileScript>().menuLevels = this;
                    level.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = levels[i].Substring(path.Length + 1, levels[i].Length - path.Length - 6);
                    level.transform.GetChild(1).GetChild(0).GetComponent<InputField>().text = levels[i].Substring(path.Length + 1, levels[i].Length - path.Length - 6);
                }
            }
        } else {
            Directory.CreateDirectory(path);
        }
    }

    public void DeleteLevel(bool apply) {
        if (apply) {
            File.Delete(getLevel.GetComponent<LevelFileScript>().pathLevel);
            Destroy(getLevel);
        }
        getLevel = null;
        panelDeleteBool = false;
    }

    public void OpenFolder() {
        Application.OpenURL(path);
    }

    public void SelectStyle(int style) {
        foreach (Image panel in checkedStyle) {
            panel.color = originalColor;
        }
        checkedStyle[style].color = new Color(1, 0, 0);
        indexStyle = style;
    }

    public void CreateLevel() {
        SceneManager.LoadScene(indexStyle + 2);
        PlayerPrefs.SetString("LevelPath", path + "\\" + inputFieldNameLevel.text + ".json");
    }

    public void CheckInput() {
        string[] symbols = {"!","@","#","$","%","^","&","*",".",",","/","<",">","?",":",";","'","\"","[","]","{","}","\\","|"};
        bool tryBool = false;
        for (int i = 0; i < symbols.Length; i++) {
            if (!tryBool) {
                if (inputFieldNameLevel.text.Contains(symbols[i])) {
                    buttonCreate.interactable = false;
                    tryBool = true;
                } else {
                    buttonCreate.interactable = inputFieldNameLevel.text.Length > 0 && !File.Exists(path + "\\" + inputFieldNameLevel.text + ".json");
                }
            }
        }
    }
}
