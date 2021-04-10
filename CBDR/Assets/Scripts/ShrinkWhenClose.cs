using UnityEngine;

public class ShrinkWhenClose : MonoBehaviour
{
    private Vector3 startScale;

    private void Start()
    {
        startScale = transform.localScale;
    }

    private void Update()
    {
        if ((Camera.main.transform.position - transform.position).magnitude < 10f)
        {
            transform.localScale = startScale * ((Camera.main.transform.position - transform.position).magnitude / 10f);
        }
        else
        {
            transform.localScale = startScale;
        }
    }
}
