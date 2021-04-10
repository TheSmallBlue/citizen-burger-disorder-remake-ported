using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelScript : MonoBehaviour {
    public Transform rigidObjects;
    public GameObject[] prefabs;
    public GameObject[] otherPrefabs;
    public CashRegister cashRegister;
    public LightSwitch[] lightSwitches;
    public TutorialScript tutorialScript;
    LevelJsonScript levelJsonScript = new LevelJsonScript();
    UserJson userJson = new UserJson();
    string path1;
    string path2;
    GameObject player;
    GameObject playerCamera;
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCamera = GameObject.Find("Main Camera");
        path1 = PlayerPrefs.GetString("LevelPath");
        PlayerPrefs.SetString("LevelPath", null);
        ReadData(path1);

        // Load User
        path2 = Application.persistentDataPath + "/user.json";
        if (File.Exists(path2)) {
            string contents = File.ReadAllText(path2);
            userJson = JsonUtility.FromJson<UserJson>(contents);
            if (userJson.autoSave) {
                StartCoroutine(TimerSave());
            }
        }
    }

    public void SaveData() {
        // New Lists
        levelJsonScript.styleIndex = SceneManager.GetSceneAt(0).buildIndex - 2;
        levelJsonScript.indexPrefabs = new List<int>();
        levelJsonScript.positionObjects = new List<Vector3>();
        levelJsonScript.rotationObjects = new List<Vector3>();
        levelJsonScript.temp = new List<float>();
        levelJsonScript.colors = new List<Color>();
        levelJsonScript.cooked = new List<float>();

        // Set values on the lists
        levelJsonScript.positionPlayer = player.transform.position;
        levelJsonScript.rotationPlayer = playerCamera.transform.eulerAngles;
        for (int i = 0; i < rigidObjects.childCount; i++) {
            for (int o = 0; o < prefabs.Length; o++) {
                if (rigidObjects.GetChild(i).gameObject.name == prefabs[o].name) {
                    levelJsonScript.indexPrefabs.Add(o);
                    levelJsonScript.positionObjects.Add(rigidObjects.GetChild(i).position);
                    levelJsonScript.rotationObjects.Add(rigidObjects.GetChild(i).rotation.eulerAngles);
                    if (rigidObjects.GetChild(i).GetComponent<Food>()) {
                        levelJsonScript.colors.Add(rigidObjects.GetChild(i).GetComponent<MeshRenderer>().material.color);
                        levelJsonScript.cooked.Add(rigidObjects.GetChild(i).GetComponent<Food>().cooked);
                        levelJsonScript.temp.Add(rigidObjects.GetChild(i).GetComponent<Food>().foodTemp);
                    } else {
                        levelJsonScript.colors.Add(Color.black);
                        levelJsonScript.cooked.Add(0);
                        levelJsonScript.temp.Add(0);
                    }
                    if (rigidObjects.GetChild(i).GetComponent<MeshRenderer>() && rigidObjects.GetChild(i).GetComponent<MeshRenderer>().material.shader.name == "TextureChange") {
                        levelJsonScript.blends.Add(rigidObjects.GetChild(i).GetComponent<MeshRenderer>().material.GetFloat("_Blend"));
                    } else {
                        levelJsonScript.blends.Add(0);
                    }
                }
            }
        }
        if (lightSwitches.Length > 0) {
            levelJsonScript.lightSwitches = new List<bool>();
            for (int i = 0; i < lightSwitches.Length; i++) {
                levelJsonScript.lightSwitches.Add(lightSwitches[i].switchBool);
            }
        }
        levelJsonScript.money = (int)cashRegister.money;
        levelJsonScript.skippedTutorial = tutorialScript.isSkipped;
        string contents = JsonUtility.ToJson(levelJsonScript, true);
        File.WriteAllText(path1, contents);

        Debug.Log(levelJsonScript.indexPrefabs.ToArray().Length);
        Debug.Log(levelJsonScript.cooked.ToArray().Length);
    }
    void ReadData(string path) {
        if (File.Exists(path)) {
            string contents = File.ReadAllText(path);
            levelJsonScript = JsonUtility.FromJson<LevelJsonScript>(contents);
            for (int i = 0; i < rigidObjects.childCount; i++) {
                Destroy(rigidObjects.GetChild(i).gameObject);
            }
            try {
                player.transform.position = levelJsonScript.positionPlayer;
                playerCamera.transform.eulerAngles = levelJsonScript.rotationPlayer;
            }
            catch {}
            for (int i = 0; i < levelJsonScript.indexPrefabs.ToArray().Length; i++) {
                GameObject instantiateObject = Instantiate(prefabs[levelJsonScript.indexPrefabs.ToArray()[i]], levelJsonScript.positionObjects.ToArray()[i], Quaternion.Euler(levelJsonScript.rotationObjects.ToArray()[i]), rigidObjects);
                instantiateObject.GetComponent<Rigidbody>().isKinematic = true;
                instantiateObject.name = prefabs[levelJsonScript.indexPrefabs.ToArray()[i]].name;
                if (instantiateObject.GetComponent<Food>() && levelJsonScript.temp.ToArray().Length > 0 && levelJsonScript.cooked.ToArray().Length > 0 && levelJsonScript.colors.ToArray().Length > 0) {
                    instantiateObject.GetComponent<Food>().foodTemp = levelJsonScript.temp.ToArray()[i];
                    instantiateObject.GetComponent<Food>().cooked = levelJsonScript.cooked.ToArray()[i];
                    instantiateObject.GetComponent<MeshRenderer>().material.color = levelJsonScript.colors.ToArray()[i];
                    if (instantiateObject.GetComponent<Food>().foodTemp >= instantiateObject.GetComponent<Flamable>().tempUntilIgniteFire) {
                        GameObject fire = Instantiate(otherPrefabs[0]);
                        fire.GetComponent<FireAnimate>().fireBase = instantiateObject.GetComponent<Flamable>();
                        fire.GetComponent<FollowGameObject>().follow = instantiateObject;
                        fire.GetComponent<FollowGameObject>().distance = Vector3.zero;
                    }
                }
                if (levelJsonScript.blends.ToArray().Length != 0 && rigidObjects.GetChild(i).GetComponent<MeshRenderer>() && instantiateObject.GetComponent<MeshRenderer>().material.shader.name == "TextureChange") {
                    instantiateObject.GetComponent<MeshRenderer>().material.SetFloat("_Blend", levelJsonScript.blends.ToArray()[i]);
                }
            }
            for (int i = 0; i < lightSwitches.Length; i++) {
                lightSwitches[i].switchBool = levelJsonScript.lightSwitches.ToArray()[i];
            }
            cashRegister.money = levelJsonScript.money;
            tutorialScript.isSkipped = levelJsonScript.skippedTutorial;
            StartCoroutine(TimerFroze());
        }
    }
    IEnumerator TimerFroze() {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < rigidObjects.childCount; i++) {
            rigidObjects.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
        }
    }
    IEnumerator TimerSave() {
        yield return new WaitForSeconds(10);
        SaveData();
        StartCoroutine(TimerSave());
    }
}
