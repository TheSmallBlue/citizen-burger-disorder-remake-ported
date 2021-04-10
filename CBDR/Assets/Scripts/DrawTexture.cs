//using System;
using UnityEngine;

[RequireComponent(typeof(ObjectUsable))]
public class DrawTexture : MonoBehaviour
{
    public bool initNewTexOnStart = true;

    public bool editable = true;

    public Vector2 lastDrawPos = Vector2.zero;

    public bool seenTutorial;

    public Material generalTutorial;

    public Material leftHandTutorial;

    public Material rightHandTutorial;

    public Material usingLeftHandTutorial;

    public Material usingRightHandTutorial;

    public Material paperTutorial;

    private ObjectUsable obj;

    private void Start()
    {
        obj = GetComponent<ObjectUsable>();
        if (initNewTexOnStart)
        {
            NewTex();
        }
    }

    private void Update()
    {
        if (editable)
        {
            if (!seenTutorial)
            {
                if (obj.holdingRightHandObject)
                {
                    if (obj.beingUsed)
                    {
                        GetComponent<Renderer>().material.SetTexture("_Drawing", usingRightHandTutorial.GetTexture("_Drawing"));
                    }
                    else
                    {
                        GetComponent<Renderer>().material.SetTexture("_Drawing", rightHandTutorial.GetTexture("_Drawing"));
                    }
                }
                else if (obj.holdingLeftHandObject)
                {
                    if (obj.beingUsed)
                    {
                        GetComponent<Renderer>().material.SetTexture("_Drawing", usingLeftHandTutorial.GetTexture("_Drawing"));
                    }
                    else
                    {
                        GetComponent<Renderer>().material.SetTexture("_Drawing", leftHandTutorial.GetTexture("_Drawing"));
                    }
                }
                else
                {
                    GetComponent<Renderer>().material.SetTexture("_Drawing", generalTutorial.GetTexture("_Drawing"));
                }
            }
            if (obj.beingUsed && seenTutorial)
            {
                if (((Input.GetButton("Fire1") && obj.usingRightHandObject) || (Input.GetButton("Fire2") && obj.usingLeftHandObject)) && (UnityEngine.Input.GetAxis("Mouse X") != 0f || UnityEngine.Input.GetAxis("Mouse Y") != 0f || Input.GetButtonDown("Fire1")))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    int layerMask = 1 << LayerMask.NameToLayer("Drawable");
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 2f, layerMask))
                    {
                        DrawOnTransformTextureAtCursorPosition(hit.transform, hit);
                    }
                }
                if (Input.GetButtonUp("Fire1") || Input.GetButtonUp("Fire2")) {
                    isFirstStep = false;
                }
                if (obj.usingRightHandObject && Input.GetButton("LeftHand") && Input.GetButtonDown("Fire1"))
                {
                    GameObject instantiateGameObject = Instantiate(Resources.Load("Prefabs/Drawing/Paper"), obj.control.leftArm.GetChild(0).transform.position + obj.control.leftArm.GetChild(0).transform.forward * 0.5f, obj.control.leftArm.transform.rotation) as GameObject;
                    MoveNotePageToPaper(gameObject, instantiateGameObject, obj.control.gameObject, false);
                    obj.control.leftArmObject = instantiateGameObject.transform;
                    obj.StopUsingObject(true);
                }
                else if (obj.usingLeftHandObject && Input.GetButton("RightHand") && Input.GetButtonDown("Fire2"))
                {
                    GameObject instantiateGameObject = Instantiate(Resources.Load("Prefabs/Drawing/Paper"), obj.control.rightArm.GetChild(0).transform.position + obj.control.rightArm.GetChild(0).transform.forward * 0.5f, obj.control.rightArm.transform.rotation) as GameObject;
                    MoveNotePageToPaper(gameObject, instantiateGameObject, obj.control.gameObject, false);
                    obj.control.rightArmObject = instantiateGameObject.transform;
                    obj.StopUsingObject(true);
                }
                if (obj.usingLeftHandObject && Input.GetButtonUp("Fire2"))
                {
                    SetLastDraw(obj.control.gameObject, Vector3.zero);
                }
                if (obj.usingRightHandObject && Input.GetButtonUp("Fire1"))
                {
                    SetLastDraw(obj.control.gameObject, Vector3.zero);
                }
            }
            else if (obj.beingUsed && !seenTutorial)
            {
                if (obj.usingRightHandObject && Input.GetButton("LeftHand") && Input.GetButtonDown("Fire1"))
                {
                    GameObject gameObject3 = Instantiate(Resources.Load("Prefabs/Drawing/Paper"), obj.control.leftArm.GetChild(0).transform.position + obj.control.leftArm.GetChild(0).transform.forward * 0.5f, obj.control.leftArm.transform.rotation) as GameObject;
                    MoveNotePageToPaper(gameObject, gameObject3, obj.control.gameObject, true);
                    obj.control.leftArmObject = gameObject3.transform;
                    seenTutorial = true;
                    obj.StopUsingObject(true);
                }
                else if (obj.usingLeftHandObject && Input.GetButton("RightHand") && Input.GetButtonDown("Fire2"))
                {
                    GameObject gameObject4 = Instantiate(Resources.Load("Prefabs/Drawing/Paper"), obj.control.rightArm.GetChild(0).transform.position + obj.control.rightArm.GetChild(0).transform.forward * 0.5f, obj.control.rightArm.transform.rotation) as GameObject;
                    MoveNotePageToPaper(gameObject, gameObject4, obj.control.gameObject, true);
                    obj.control.rightArmObject = gameObject4.transform;
                    seenTutorial = true;
                    obj.StopUsingObject(true);
                }
            }
        }
    }

    private void DrawOnTransformTextureAtCursorPosition(Transform transformToDrawOn, RaycastHit hit)
    {
        /*Texture2D texture2D = transformToDrawOn.GetComponent<MeshRenderer>().material.GetTexture("_Drawing") as Texture2D;
        Vector2 textureCoord = hit.textureCoord;
        textureCoord.x *= (float)texture2D.width;
        textureCoord.y *= (float)texture2D.height;*/
        DrawOnTexture(obj.control.gameObject, transformToDrawOn.gameObject, 4);
    }

    private void MoveNotePageToPaper(GameObject targetNotepad, GameObject targetPaper, GameObject playerCreating, bool ignoreTexture = false)
    {
        targetPaper.GetComponent<MeshRenderer>().material.SetTexture("_Drawing", targetNotepad.GetComponent<MeshRenderer>().material.GetTexture("_Drawing"));
        targetPaper.GetComponent<MeshRenderer>().material.SetTextureOffset("_Drawing", new Vector2(0.44f, 0));
        targetPaper.GetComponent<MeshRenderer>().material.SetTextureScale("_Drawing", new Vector2(0.46f, 0.54f));
        targetPaper.transform.parent = GameObject.Find("Rigid-Elements").transform;
        targetPaper.name = "Paper";
        NewTex();

        /*GameObject gameObject = targetNotepad;
        GameObject gameObject2 = targetPaper;
        gameObject2.GetComponent<DrawTexture>().initNewTexOnStart = false;
        if (!seenTutorial)
        {
            seenTutorial = true;
        }
        Texture2D texture2D;
        if (!ignoreTexture)
        {
            texture2D = Instantiate(gameObject.GetComponent<MeshRenderer>().material.GetTexture("_Drawing")) as Texture2D;
        }
        else
        {
            texture2D = GetNewTex();
        }
        Texture2D texture2D2 = new Texture2D(256, 256, TextureFormat.ARGB32, false);
        texture2D2.SetPixels(texture2D.GetPixels(228, 15, 256, 256));
        texture2D2.Apply();
        if (!ignoreTexture)
        {
            gameObject2.GetComponent<MeshRenderer>().material.SetTexture("_Drawing", texture2D2);
        }
        else
        {
            gameObject2.GetComponent<MeshRenderer>().material.SetTexture("_Drawing", paperTutorial.GetTexture("_Drawing"));
        }
        gameObject.GetComponent<DrawTexture>().NewTex();*/
    }

    Vector2 lastDrawPosition;
    bool isFirstStep;
    private void DrawOnTexture(GameObject callingPlayer, GameObject targetTransform, int brushSize)
    {
        FirstPersonControl component = callingPlayer.GetComponent<FirstPersonControl>();
        Texture2D texture2D = targetTransform.GetComponent<MeshRenderer>().material.GetTexture("_Drawing") as Texture2D;
        //DrawTexture component2 = targetTransform.GetComponent<DrawTexture>();
        Color[] array = new Color[25];
        for (int i = 0; i < 25; i++)
        {
            array[i] = Color.black;
            //array[i] = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }
        if (component.lastDrawPosition != Vector2.zero)
        {
            Vector2 vector = new Vector2(component.lastDrawPosition.x - lastDrawPosition.x, component.lastDrawPosition.y - lastDrawPosition.y);
            float magnitude = vector.magnitude;
            /*if (magnitude > 3f)
            {
                float num = Mathf.Round(magnitude / 3f);
                int num2 = 0;
                while ((float)num2 < num)
                {
                    //texture2D.SetPixels((int)Mathf.Lerp(component.lastDrawPosition.x, lastDrawPosition.x, (float)num2 / num), (int)Mathf.Lerp(component.lastDrawPosition.y, lastDrawPosition.y, (float)num2 / num), brushSize, brushSize, array);

                    //texture2D.SetPixels((int)Mathf.Lerp(
                    //    Mathf.Clamp(component.lastDrawPosition.x + 320, 40,512), 
                    //    lastDrawPosition.x, num2 / num), (int)Mathf.Lerp(
                    //        Mathf.Clamp(component.lastDrawPosition.y + 128, 0, 256), 
                    //        lastDrawPosition.y, num2 / num), brushSize, brushSize, array);
                    
                    num2++;
                }
                texture2D.Apply();
            }*/
            // Point
            //texture2D.SetPixels((int)Mathf.Clamp(component.lastDrawPosition.x / 1.35f + 348, 234, 460), (int)Mathf.Clamp(component.lastDrawPosition.y / 1.35f + 122, 0, 272), brushSize, brushSize, array);
            if (!isFirstStep) {
                lastDrawPosition = component.lastDrawPosition;
                isFirstStep = true;
            }
            float num = Mathf.Round(magnitude / 3f);
            int num2 = 0;
            while (num2 < num) {
                texture2D.SetPixels((int)Mathf.Clamp(
                    Mathf.Lerp(component.lastDrawPosition.x, lastDrawPosition.x, num2 / num) / 1.35f + 348
                    , 234, 460), (int)Mathf.Clamp(
                        Mathf.Lerp(component.lastDrawPosition.y, lastDrawPosition.y, num2 / num) / 1.35f + 122
                        , 0, 272), brushSize, brushSize, array);

                num2++;
            }

            // Liner
            /*texture2D.SetPixels((int)Mathf.Clamp(
                Mathf.Lerp(component.lastDrawPosition.x, lastDrawPosition.x, 2) / 1.35f + 348
                , 234, 460), (int)Mathf.Clamp(
                    Mathf.Lerp(component.lastDrawPosition.y, lastDrawPosition.y, 2) / 1.35f + 122
                    , 0, 272), brushSize, brushSize, array);*/
            lastDrawPosition = component.lastDrawPosition;
        }
        //component.lastDrawPosition = lastDrawPosition;
        //texture2D.SetPixels(pixelX, pixelY, brushSize, brushSize, array);
        texture2D.Apply();
        //targetTransform.GetComponent<MeshRenderer>().material.SetTexture("_Drawing", texture2D);
    }

    /*private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        if (stream.isWriting)
        {
            Vector3 vector = new Vector3(lastDrawPos.x, lastDrawPos.y, 0f);
            stream.Serialize(ref vector);
        }
        else
        {
            Vector3 zero = Vector3.zero;
            stream.Serialize(ref zero);
            lastDrawPos = new Vector2(zero.x, zero.y);
        }
    }*/

    public void SetLastDraw(GameObject playerID, Vector3 lastDraw3)
    {
        FirstPersonControl component = playerID.GetComponent<FirstPersonControl>();
        component.lastDrawPosition = new Vector2(lastDraw3.x, lastDraw3.y);
    }

    public Texture2D GetNewTex()
    {
        Texture2D texture2D = new Texture2D(512, 512, TextureFormat.ARGB32, true);
        Color[] array = new Color[262144];
        for (int i = 0; i < 262144; i++)
        {
            array[i] = Color.white;
        }
        texture2D.SetPixels(array);
        texture2D.Apply();
        return texture2D;
    }

    public void NewTex()
    {
        Texture2D texture2D = new Texture2D(512, 512, TextureFormat.ARGB32, true);
        Color[] array = new Color[262144];
        for (int i = 0; i < 262144; i++)
        {
            array[i] = Color.white;
        }
        texture2D.SetPixels(array);
        texture2D.Apply();
        GetComponent<Renderer>().material.SetTexture("_Drawing", texture2D);
    }
}
