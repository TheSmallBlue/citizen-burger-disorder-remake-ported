using UnityEngine;

public class LockMouse : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
        m = Camera.main.GetComponent<menu>();
        ml = Camera.main.GetComponent<MouseLook>();
    }

    private void Update()
    {
        if (m.enabled)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
        /*if (Input.GetKeyDown(27))
        {
            m.enabled = !Cursor.visible;
            ml.enabled = false;
        }
        if (Input.GetKeyDown(282))
        {
            print("opened main menu");
            m.enabled = true;
        }
        if (Input.GetKeyDown(287))
        {
            Application.LoadLevel(0);
        }*/
    }

    private menu m;

    private MouseLook ml;
}
