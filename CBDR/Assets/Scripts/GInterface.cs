using System.Collections.Generic;
using UnityEngine;

public class GInterface : MonoBehaviour
{
    public GameObject elementPrefab;

    public GameObject buttonPrefab;

    public List<GElement> graphicElements = new List<GElement>();

    public GScreen screen;

    public Vector3 origin;

    public Vector3 up;

    public Vector3 left;

    public Rect bounds = new Rect(0f, 0f, 10f, 10f);

    public Vector2 scale;

    public float zLayer = 0.02f;

    public Color backgroundColor;

    public GElement CreateElement(float x, float y, float wPercent, float hPercent, string text = "", bool store = true)
    {
        GElement component = (Instantiate(elementPrefab, transform.position + transform.forward * 0.5f, transform.rotation) as GameObject).GetComponent<GElement>();
        component.SetInterface(this);
        GElement gElement = component;
        Vector2 vector = BoundsPercentageToPixels(wPercent, 0f);
        float x2 = vector.x;
        Vector2 vector2 = BoundsPercentageToPixels(0f, hPercent);
        gElement.bounds = new Rect(x, y, x2, vector2.y);
        component.text = text;
        component.zLayer = 0.07f;
        if (store)
        {
            graphicElements.Add(component);
            component.transform.parent = transform;
        }
        /*if (Network.isServer)
        {
            component.GetComponent<NetworkView>().viewID = Network.AllocateViewID();
        }*/
        return component;
    }

    public GElement CreateButton(float x, float y, float wPercent, float hPercent, string text = "", bool store = true)
    {
        GButton component = (Instantiate(buttonPrefab, transform.position + transform.forward * 0.5f, transform.rotation) as GameObject).GetComponent<GButton>();
        component.SetInterface(this);
        GButton gButton = component;
        Vector2 vector = BoundsPercentageToPixels(wPercent, 0f);
        float x2 = vector.x;
        Vector2 vector2 = BoundsPercentageToPixels(0f, hPercent);
        gButton.bounds = new Rect(x, y, x2, vector2.y);
        component.text = text;
        component.zLayer = 0.07f;
        if (store)
        {
            graphicElements.Add(component);
            component.transform.parent = transform;
        }
        /*if (Network.isServer)
        {
            component.GetComponent<NetworkView>().viewID = Network.AllocateViewID();
        }*/
        return component;
    }

    private void Update()
    {
        CalculatePositioning();
        screen.Draw(this);
        if (GetComponent<Renderer>().material.GetColor("_EmissionColor") != backgroundColor)
        {
            GetComponent<Renderer>().material.SetColor("_EmissionColor", backgroundColor);
        }
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
        Vector3 a2 = position + transform.forward * 0.02f;
        origin = a2 + up - left;
    }

    public Vector2 BoundsPercentageToPixels(float wPercent = 1f, float hPercent = 1f)
    {
        float x = wPercent * bounds.width;
        float y = hPercent * bounds.height;
        return new Vector2(x, y);
    }

    public Vector2 BoundsPixelPosition(float wPixel = 100f, float hPixel = 100f)
    {
        return screen.BoundsPixelPosition(wPixel, hPixel);
    }

    public void SetScreen(GScreen gScreen)
    {
        screen = gScreen;
        transform.parent = screen.transform;
    }

    public void SetBackgroundColor(Color c)
    {
        backgroundColor = c;
        transform.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", backgroundColor);
        transform.GetComponent<MeshRenderer>().material.color = Color.black;
    }

    public void DrawElement(GElement ge)
    {
        screen.Draw(ge);
    }

    public void DrawButton(GButton gb)
    {
        screen.Draw(gb);
    }
}
