using UnityEngine;

public class RotateToLookAtCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        if (GetComponent<MeshRenderer>().isVisible)
        {
            transform.LookAt(Camera.main.transform);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + XFlip, transform.rotation.eulerAngles.y + YFlip, transform.rotation.eulerAngles.z);
        }
    }

    public float YFlip = -180f;

    public float XFlip;
}
