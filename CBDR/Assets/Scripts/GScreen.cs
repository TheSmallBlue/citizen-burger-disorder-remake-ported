using UnityEngine;
using UnityEngine.SceneManagement;

public class GScreen : MonoBehaviour
{
    public GameObject InterfacePrefab;

    public GameObject NavigationPrefab;

    public Computer computer;

    public Vector3 screenBasePos;

    public Vector3 screenOrigin;

    private Vector3 screenCentre;

    private Vector3 screenUp;

    private Vector3 screenDown;

    private Vector3 screenLeft;

    private Vector3 screenRight;

    private Vector2 screenBounds;

    public Vector2 screenResolution = new Vector2(800f, 450f);

    public float pixelsPerUnit;

    private INavigation navigationBar;

    private GInterface mainDisplay;

    private void Awake()
    {
        Init();
    }

    public GInterface CreateInterface(float x, float y, float wPercent, float hPercent)
    {
        GInterface component = (Instantiate(InterfacePrefab, screenCentre, transform.rotation) as GameObject).GetComponent<GInterface>();
        component.SetScreen(this);
        if (SceneManager.GetSceneAt(0).buildIndex == 2) {
            component.SetBackgroundColor(Color.black);
        } else {
            component.SetBackgroundColor(new Color(0, 0.5f, 1));
        }
        GInterface gInterface = component;
        Vector2 vector = BoundsPercentageToPixels(wPercent, 0f);
        float x2 = vector.x;
        Vector2 vector2 = BoundsPercentageToPixels(0f, hPercent);
        gInterface.bounds = new Rect(x, y, x2, vector2.y);
        /*if (Network.isServer)
        {
            component.GetComponent<NetworkView>().viewID = Network.AllocateViewID();
        }*/
        return component;
    }

    public INavigation CreateNavigation(float x, float y, float wPercent, float hPercent)
    {
        INavigation component = (Instantiate(NavigationPrefab, screenCentre, transform.rotation) as GameObject).GetComponent<INavigation>();
        component.SetScreen(this);
        if (SceneManager.GetSceneAt(0).buildIndex == 2) {
            component.SetBackgroundColor(Color.black);
        } else {
            component.SetBackgroundColor(new Color(0, 0.8f, 1));
        }
        INavigation navigation = component;
        Vector2 vector = BoundsPercentageToPixels(wPercent, 0f);
        float x2 = vector.x;
        Vector2 vector2 = BoundsPercentageToPixels(0f, hPercent);
        navigation.bounds = new Rect(x, y, x2, vector2.y);
        /*if (Network.isServer)
        {
            component.GetComponent<NetworkView>().viewID = Network.AllocateViewID();
        }*/
        return component;
    }

    private void Init()
    {
        screenBasePos = transform.position;
        Vector3 up = transform.up;
        Vector3 localScale = transform.localScale;
        screenUp = up * localScale.y * 0.5f;
        Vector3 a = -transform.right;
        Vector3 localScale2 = transform.localScale;
        screenLeft = a * localScale2.x * 0.5f;
        screenDown = -screenUp;
        screenRight = -screenLeft;
        Vector3 a2 = screenBasePos;
        Vector3 a3 = -transform.forward;
        Vector3 localScale3 = transform.localScale;
        screenCentre = a2 + a3 * localScale3.z * 0.51f;
        screenOrigin = screenCentre + screenUp + screenLeft;
        screenBounds = new Vector2((screenLeft - screenRight).magnitude, (screenUp - screenDown).magnitude);
        pixelsPerUnit = screenResolution.x / screenBounds.x;
    }

    private void Update()
    {
        Init();
    }

    public Vector2 BoundsPercentageToPixels(float wPercent = 1f, float hPercent = 1f)
    {
        float x = wPercent * screenBounds.x;
        float y = hPercent * screenBounds.y;
        return new Vector2(x, y) * pixelsPerUnit;
    }

    public Vector2 BoundsPixelPosition(float wPixel = 100f, float hPixel = 100f)
    {
        float x = screenBounds.x / (screenResolution.x / wPixel);
        float y = screenBounds.y / (screenResolution.y / hPixel);
        return new Vector2(x, y) * pixelsPerUnit;
    }

    public void Draw(GInterface gi)
    {
        Vector3 zero = Vector3.zero;
        Vector3 b = screenRight * (gi.bounds.x / screenResolution.x) * 2f;
        Vector3 b2 = screenDown * (gi.bounds.y / screenResolution.y) * 2f;
        zero = screenOrigin + b + b2;
        gi.transform.parent = null;
        Transform transform = gi.transform;
        float x = gi.bounds.width / pixelsPerUnit;
        float y = gi.bounds.height / pixelsPerUnit;
        Vector3 localScale = gi.transform.localScale;
        transform.localScale = new Vector3(x, y, localScale.z);
        gi.CalculatePositioning();
        gi.transform.parent = base.transform;
        gi.transform.position = zero - gi.up + gi.left + -base.transform.forward * gi.zLayer;
    }

    public void Draw(GElement ge)
    {
        Vector3 zero = Vector3.zero;
        Vector3 b = screenRight * ((ge.bounds.x + ge.parentInterface.bounds.x) / screenResolution.x) * 2f;
        Vector3 b2 = screenDown * ((ge.bounds.y + ge.parentInterface.bounds.y) / screenResolution.y) * 2f;
        zero = screenOrigin + -base.transform.forward * ge.zLayer + b + b2;
        ge.transform.parent = null;
        Transform transform = ge.transform;
        float x = ge.bounds.width / pixelsPerUnit;
        float y = ge.bounds.height / pixelsPerUnit;
        Vector3 localScale = ge.transform.localScale;
        transform.localScale = new Vector3(x, y, localScale.z);
        ge.CalculatePositioning();
        ge.transform.parent = ge.parentInterface.transform;
        ge.transform.position = zero - ge.up + ge.left;
    }
}
