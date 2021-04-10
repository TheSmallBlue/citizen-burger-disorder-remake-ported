using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class LevelJsonScript {
    public int styleIndex;
    public Vector3 positionPlayer;
    public Vector3 rotationPlayer;
    public List<int> indexPrefabs;
    public List<Vector3> positionObjects;
    public List<Vector3> rotationObjects;
    public List<float> cooked;
    public List<float> temp;
    public List<Color> colors;
    public List<bool> lightSwitches;
    public List<float> blends;
    public int money;
    public bool skippedTutorial;
}
