using System.Collections.Generic;
using UnityEngine;

public class GElement : MonoBehaviour
{
    public GInterface parentInterface;

    public INavigation parentNavigation;

    public GameObject textPrefab;

    private TextMesh attachedText;

    public Vector3 origin;

    public Vector3 up;

    public Vector3 left;

    public Rect bounds = new Rect(0f, 0f, 10f, 10f);

    public float zLayer = 0.01f;

    public Color color;

    protected Color normalColor;

    public Color textColor;

    public string text;

    public int textSize;

    protected Material defaultMaterial;

    private void Awake()
    {
        text = "< INIT >";
        CreateText(new Vector2(bounds.x, bounds.y));
        normalColor = color;
        defaultMaterial = GetComponent<Renderer>().material;
    }

    public void SetText(string txt, bool hide = false)
    {
        text = txt;
        HideText(hide);
    }

    public void SetMaterialToFoodLocal(string food)
    {
        GetComponent<Renderer>().material = Menu.GetFoodMaterial(food);
    }

    public void SetMaterialToFood(GameObject id, string food)
    {
        id.GetComponent<MeshRenderer>().material = Menu.GetFoodMaterial(food);
    }

    public void ResetMaterial(GameObject id)
    {
        id.GetComponent<MeshRenderer>().material = transform.GetComponent<GElement>().defaultMaterial;
    }

    public bool SetUseable(bool active)
    {
        bool result = false;
        if ((bool)GetComponent<GButton>())
        {
            GetComponent<GButton>().usable = active;
            result = true;
        }
        return result;
    }

    public bool SetColor()
    {
        color = normalColor;
        return true;
    }

    public bool SetColor(Color c, string name = "")
    {
        bool result = false;
        if (name != null)
        {
            /*if (_003C_003Ef__switch_0024map1 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
                dictionary.Add(string.Empty, 0);
                dictionary.Add("highlight", 1);
                dictionary.Add("pressed", 2);
                _003C_003Ef__switch_0024map1 = dictionary;
            }
            if (_003C_003Ef__switch_0024map1.TryGetValue(name, out int value))
            {
                switch (value)
                {
                    case 0:
                        normalColor = c;
                        color = c;
                        result = true;
                        break;
                    case 1:
                        if ((bool)GetComponent<GButton>())
                        {
                            GetComponent<GButton>().hoverColor = c;
                            result = true;
                        }
                        break;
                    case 2:
                        if ((bool)GetComponent<GButton>())
                        {
                            GetComponent<GButton>().pressedColor = c;
                            result = true;
                        }
                        break;
                }
            }*/
        }
        return result;
    }

    public void CalculatePositioning()
    {
        Vector3 position = transform.position;
        Vector3 a = transform.up;
        Vector3 localScale = transform.localScale;
        up = a * localScale.y * 0.5f;
        Vector3 right = transform.right;
        Vector3 localScale2 = transform.localScale;
        left = right * localScale2.x * 0.5f;
        Vector3 a2 = position;
        Vector3 forward = transform.forward;
        Vector3 localScale3 = transform.localScale;
        Vector3 a3 = a2 + forward * localScale3.z * 0.51f;
        origin = a3 + up + left;
    }

    public void HideText(bool hide = true)
    {
        attachedText.gameObject.SetActive(!hide);
    }

    public bool IsTextHidden()
    {
        return attachedText.gameObject.activeSelf;
    }

    private void CreateText(Vector2 position, int textSize = 16)
    {
        if (!attachedText)
        {
            GameObject gameObject = Instantiate(textPrefab, transform.position + transform.up * 0.6f - transform.forward * 0.01f, transform.rotation);
            attachedText = gameObject.GetComponent<TextMesh>();
            gameObject.transform.parent = transform;
            attachedText.fontSize = textSize;
            attachedText.GetComponent<MeshRenderer>().material.SetColor("_Text Color", textColor);
            textSize = attachedText.fontSize;
        }
    }

    public void RefreshDisplay()
    {
        /*if (GetComponent<Renderer>().material.color != color)
        {
            GetComponent<Renderer>().material.color = color;
        }*/
        if (GetComponent<Renderer>().material.GetColor("_EmissionColor") != color)
        {
            GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
        }
        if (attachedText.GetComponent<MeshRenderer>().material.GetColor("_Color") != textColor)
        {
            attachedText.GetComponent<MeshRenderer>().material.SetColor("_Color", textColor);
        }
        if (!attachedText.text.Equals(text))
        {
            attachedText.text = text;
        }
        if (attachedText.fontSize != textSize)
        {
            attachedText.fontSize = textSize;
        }
    }

    public virtual void Update()
    {
        CalculatePositioning();
        RefreshDisplay();
        parentInterface.DrawElement(this);
    }

    public void SetInterface(GInterface gi)
    {
        parentInterface = gi;
        transform.parent = gi.transform;
    }

    public void SetInterface(INavigation inav)
    {
        parentInterface = inav;
        transform.parent = inav.transform;
    }
}
