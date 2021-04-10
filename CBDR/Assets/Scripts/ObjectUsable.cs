using System;
using UnityEngine;

public class ObjectUsable : PickupObject
{
    public FirstPersonControl control;

    private Vector3 defaultHeldPositionOffset;

    public UnityEngine.Object pencilPrefab;

    private Transform pencil;

    private Vector2 lastDrawPosition = Vector2.zero;

    public bool usingRightHandObject;

    public bool usingLeftHandObject;

    public bool holdingRightHandObject;

    public bool holdingLeftHandObject;

    private bool stopUsingObject;

    private int layerMask;

    private void Start()
    {
        layerMask = ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Default"));
    }

    public void StopUsingObject(bool stop = true)
    {
        stopUsingObject = stop;
    }

    private void Update()
    {
        usingRightHandObject = false;
        usingLeftHandObject = false;
        if (beingHeld)
        {
            if (!control)
            {
                control = GameObject.Find("Player(Mine)").GetComponent<FirstPersonControl>();
            }
            if (Input.GetButton("Fire2") && Input.GetButton("RightHand") && !usingLeftHandObject)
            {
                holdingRightHandObject = true;
                if (Input.GetButton("RightHand") && beingUsed)
                {
                    usingRightHandObject = true;
                }
                if (Input.GetButtonDown("Fire1") && !Input.GetButton("LeftHand"))
                {
                    usingRightHandObject = true;
                    if (beingUsed)
                    {
                        RaycastHit raycastHit;
                        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 6f, layerMask))
                        {
                            print("Won't exit");
                        }
                        if (raycastHit.transform != transform)
                        {
                            print("Will exit");
                            stopUsingObject = true;
                        }
                    }
                }
            }
            else
            {
                usingRightHandObject = false;
                holdingRightHandObject = false;
            }
            if (Input.GetButton("Fire1") && Input.GetButton("LeftHand") && !usingRightHandObject)
            {
                holdingLeftHandObject = true;
                if (Input.GetButton("LeftHand") && beingUsed)
                {
                    usingLeftHandObject = true;
                }
                if (Input.GetButtonDown("Fire2") && !Input.GetButton("RightHand"))
                {
                    usingLeftHandObject = true;
                    RaycastHit raycastHit2;
                    if (beingUsed && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit2))
                    {
                        if (raycastHit2.transform != transform)
                        {
                            print("Will exit");
                            stopUsingObject = true;
                        }
                        else
                        {
                            print("Won't exit");
                        }
                    }
                }
            }
            else
            {
                usingLeftHandObject = false;
                holdingLeftHandObject = false;
            }
        }
        if ((usingRightHandObject || usingLeftHandObject) && !beingUsed)
        {
            beingUsed = true;
            /*Camera.main.GetComponent<MouseLook>().enabled = false;
            Cursor.lockState = CursorLockMode.None;*/
            if (pencil == null)
            {
                GameObject gameObject = Instantiate(pencilPrefab, transform.position, transform.rotation) as GameObject;
                pencil = gameObject.transform;
            }
            else
            {
                pencil.transform.rotation = transform.rotation;
                pencil.transform.position = transform.position;
            }
            control.setObjectCollisions(false/*, gameObject*/);
        }
        if ((stopUsingObject || !beingHeld) && beingUsed)
        {
            beingUsed = false;
            /*Camera.main.GetComponent<MouseLook>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;*/
            Destroy(pencil.gameObject);
            if (!GetComponent<Collider>().enabled)
            {
                control.setObjectCollisions(true/*, gameObject*/);
            }
        }
        if (beingUsed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit3;
            if (Physics.Raycast(ray, out raycastHit3, 5f, layerMask)) {
                pencil.position = raycastHit3.point;
            } else {
                Vector3 cursorVector = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.5f);
                cursorVector = Camera.main.ScreenToWorldPoint(cursorVector);
                pencil.position = cursorVector;
            }
        }
        stopUsingObject = false;
    }
}
