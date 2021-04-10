using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    private void LateUpdate()
    {
        if (follow != null)
        {
            transform.position = follow.transform.position + distance;
        }
    }

    public GameObject follow;

    public Vector3 distance = new Vector3(0f, 0f, 0f);
}
