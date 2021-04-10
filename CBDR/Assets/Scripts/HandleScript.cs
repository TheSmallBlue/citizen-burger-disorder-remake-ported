using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleScript : MonoBehaviour {
    public Transform transformObject;
    public Vector3 startPoint;
    public Vector3 endPoint;
    //bool isDragging;
    float timerUndrag;
    void Update() {
        if (timerUndrag <= 0) {
            //isDragging = false;
        } else {
            timerUndrag -= Time.deltaTime;
        }
    }

    public void DragHandle(Vector3 point) {
        //isDragging = true;
        transformObject.position = Vector3.Lerp(transformObject.position, new Vector3(
            Mathf.Clamp(point.x, startPoint.x, endPoint.x),
            Mathf.Clamp(point.y, startPoint.y, endPoint.y),
            Mathf.Clamp(point.z, startPoint.z, endPoint.z)
            ), 0.5f);
        timerUndrag = 1.2f;
    }
}
