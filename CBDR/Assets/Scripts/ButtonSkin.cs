using UnityEngine;
using System.IO;
public class ButtonSkin : MonoBehaviour {
    public string skinName;
    public void SetTexture() {
        Texture2D textureSkin = new Texture2D(0, 0);
        textureSkin.LoadImage(File.ReadAllBytes(Directory.GetParent(Application.dataPath).ToString() + "\\Skins\\" + skinName));
        GameObject.Find("MeshPlayer").GetComponent<MeshRenderer>().material.mainTexture = textureSkin;
        OptionsScript.skinFile = skinName;
    }
}
