using UnityEngine;
using UnityEngine.SceneManagement;

public class GButton : GElement
{
    private void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Button");
        /*portable = transform.root.GetComponent<Computer>().portable;
        if (portable)
        {
            obj = transform.root.GetComponent<ObjectUsable>();
        }*/
        
        if (SceneManager.GetSceneAt(0).buildIndex == 2) {
            hoverColor = Color.Lerp(Color.blue, Color.white, 0.7f);
        } else {
            hoverColor = new Color(0, 0.5f, 1);
        }
    }

    public override void Update()
    {
        if (usable)
        {
            if (player)
            {
                /*if (portable)
                {
                    if (obj.beingUsed)
                    {
                        if ((Input.GetButton("Fire1") && obj.usingRightHandObject) || (Input.GetButton("Fire2") && obj.usingLeftHandObject))
                        {
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            RaycastHit raycastHit;
                            if (Physics.Raycast(ray, out raycastHit, 10f, layerMask) && raycastHit.collider.transform == GetComponent<Collider>().transform)
                            {
                                playerPressed = true;
                                if (obj.usingRightHandObject)
                                {
                                    playerHoveringLeft = true;
                                }
                                else
                                {
                                    playerHoveringRight = true;
                                }
                            }
                        }
                        else
                        {
                            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                            RaycastHit raycastHit2;
                            if (Physics.Raycast(ray2, out raycastHit2, 10f, layerMask))
                            {
                                if (raycastHit2.collider.transform == GetComponent<Collider>().transform)
                                {
                                    if (obj.usingRightHandObject)
                                    {
                                        playerHoveringLeft = true;
                                    }
                                    else
                                    {
                                        playerHoveringRight = true;
                                    }
                                }
                            }
                            else
                            {
                                playerHoveringLeft = false;
                                playerHoveringRight = false;
                            }
                        }
                    }
                }
                else
                {
                    if (Input.GetButton("LeftHand") && player.leftArm)
                    {
                        Ray ray3;
                        ray3 = new Ray(player.leftArm.transform.position, player.leftArm.transform.forward);
                        RaycastHit raycastHit3;
                        if (Physics.Raycast(ray3, out raycastHit3, 14f, layerMask))
                        {
                            if (raycastHit3.collider.transform == GetComponent<Collider>().transform)
                            {
                                playerHoveringLeft = true;
                            }
                        }
                        else
                        {
                            playerHoveringLeft = false;
                            playerPressed = false;
                        }
                    }
                    if (Input.GetButton("RightHand") && player.rightArm)
                    {
                        RaycastHit raycastHit4;
                        if (Physics.Raycast(player.rightArm.transform.position, player.rightArm.transform.forward, out raycastHit4, 6f, layerMask))
                        {
                            if (raycastHit4.collider.transform == GetComponent<Collider>().transform)
                            {
                                playerHoveringRight = true;
                            }
                        }
                        else
                        {
                            playerHoveringRight = false;
                            playerPressed = false;
                        }
                    }
                    if (!Input.GetButton("LeftHand") && !Input.GetButton("RightHand"))
                    {
                        playerHoveringLeft = false;
                        playerHoveringRight = false;
                    }
                    if ((Input.GetButton("Fire1") && playerHoveringLeft) || (Input.GetButton("Fire2") && playerHoveringRight))
                    {
                        playerPressed = true;
                    }
                }*/
                if (Input.GetButton("LeftHand") && player.leftArm)
                {
                    Ray ray3;
                    ray3 = new Ray(player.leftArm.transform.position, player.leftArm.transform.forward);
                    RaycastHit raycastHit3;
                    if (Physics.Raycast(ray3, out raycastHit3, 14f, layerMask))
                    {
                        if (raycastHit3.collider.transform == GetComponent<Collider>().transform)
                        {
                            playerHoveringLeft = true;
                        }
                    }
                    else
                    {
                        playerHoveringLeft = false;
                        playerPressed = false;
                    }
                }
                if (Input.GetButton("RightHand") && player.rightArm)
                {
                    RaycastHit raycastHit4;
                    if (Physics.Raycast(player.rightArm.transform.position, player.rightArm.transform.forward, out raycastHit4, 6f, layerMask))
                    {
                        if (raycastHit4.collider.transform == GetComponent<Collider>().transform)
                        {
                            playerHoveringRight = true;
                        }
                    }
                    else
                    {
                        playerHoveringRight = false;
                        playerPressed = false;
                    }
                }
                if (!Input.GetButton("LeftHand") && !Input.GetButton("RightHand"))
                {
                    playerHoveringLeft = false;
                    playerHoveringRight = false;
                }
                if ((Input.GetButton("Fire1") && playerHoveringLeft) || (Input.GetButton("Fire2") && playerHoveringRight))
                {
                    playerPressed = true;
                }
            } else if (Time.frameCount % 10 == 0)
            {
                foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (gameObject.GetComponent<FirstPersonControl>())
                    {
                        player = gameObject.GetComponent<FirstPersonControl>();
                    }
                }
            }
            if (playerPressed)
            {
                color = pressedColor;
                if ((Input.GetButtonUp("Fire1") && playerHoveringLeft) || (Input.GetButtonUp("Fire2") && playerHoveringRight))
                {
                    if (text == string.Empty)
                    {
                        print("Error: no name.");
                    }
                    foreach (Computer computer in Computer.computers)
                    {
                        computer.SetButtonDown(text);
                    }
                    playerPressed = false;
                }
            }
            else if (playerHoveringRight || playerHoveringLeft)
            {
                color = Color.Lerp(hoverColor, pressedColor, 0.6f);
            }
            else
            {
                color = Color.Lerp(Color.Lerp(normalColor, hoverColor, 0.5f), Color.Lerp(normalColor, hoverColor, 1f), Mathf.Sin(Time.time * 2f) * 1f + 0.3f);
            }
        }
        playerHoveringLeft = false;
        playerHoveringRight = false;
        CalculatePositioning();
        RefreshDisplay();
        parentInterface.DrawButton(this);
    }

    private FirstPersonControl player;

    public bool usable = true;

    //public Color hoverColor = Color.Lerp(Color.blue, Color.white, 0.7f);
    public Color hoverColor;

    public Color pressedColor = Color.Lerp(Color.green, Color.white, 0.6f);

    private bool playerHoveringLeft;

    private bool playerHoveringRight;

    private bool playerPressed;

    private int layerMask;

    private bool portable;

    private ObjectUsable obj;
}
