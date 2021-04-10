using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour
{
    void Update()
    {
        if (canMouseMove) {
            float num = 1f;
            if (inversion)
            {
                num = -num;
            }
            if (axes == RotationAxes.MouseXAndY)
            {
                float num2 = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
                num2 += Input.GetAxis("Mouse X") * sensitivityX;
                rotationY += Input.GetAxis("Mouse Y") * num * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                transform.localEulerAngles = new Vector3(-rotationY, num2, 0f);
            }
            else if (axes == RotationAxes.MouseX)
            {
                transform.Rotate(0f, Input.GetAxis("Mouse X") * sensitivityX, 0f);
            }
            else
            {
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0f);
            }
            /*num2 += Input.GetAxis("Mouse X") * sensitivityX;
            rotationY += Input.GetAxis("Mouse Y") * num * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            transform.localEulerAngles = new Vector3(-rotationY, 0, 0f);*/
        }
    }

    void Start()
    {
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    public float num2;

    public RotationAxes axes;

    public float sensitivityX = 15f;

    public float sensitivityY = 15f;

    /*public float minimumX = -360f;

    public float maximumX = 360f;*/

    public float minimumY = -60f;

    public float maximumY = 60f;

    private float rotationY;

    public bool inversion;

    public enum RotationAxes
    {
        MouseXAndY,
        MouseX,
        MouseY
    }

    public bool canMouseMove;
}
