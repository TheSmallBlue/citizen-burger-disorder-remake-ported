using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class FirstPersonControl : MonoBehaviour {

    public CharacterController controller;

    public Camera cam;

    public Transform arm;

    public Transform leftArm;

    public Transform leftArmObject;

    PickupObject leftArmPickup;

    public Transform rightArm;

    public Transform rightArmObject;

    PickupObject rightArmPickup;

    public float gravity = 9.81f;

    public float moveSpeed = 16f;

    public float runMultiplier = 1.333f;

    public float crouchMultiplier = 0.25f;

    private Vector3 moveDir = Vector3.zero;

    //private float gravityToApply;

    public int layerMask;

    private float maxCameraAngleFromZero = 86f;

    private float armForwardBasedOnRotation;

    private float armExtraReach = 1.2f;

    public string username = string.Empty;

    public static FirstPersonControl localPlayer;

    public Vector2 lastDrawPosition = Vector2.zero;

    //private int spawnedItems;

    //private int maxSpawnedItems = 8;

    //private menu mainMenu;

    public HandleScript leftHandle;

    public HandleScript rightHandle;

    public static bool isSpectator;

    private void Awake()
    {
        gameObject.name = "Player(Mine)";
        localPlayer = this;
        layerMask = -33025;
        if (cam == null)
        {
            cam = Camera.main;
        }
        //mainMenu = cam.GetComponent<menu>();
    }

    float spectatorSpeed;
    void Update() {
        if (Input.GetKeyDown(KeyCode.F2)) {
            isSpectator = !isSpectator;
        }
        cam.GetComponent<FollowGameObject>().enabled = !isSpectator;
        if (isSpectator) {
            SpectatorController();
        } else {
            PlayerController();
        }
        // Invisible Wall
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -50, 40), transform.position.y, Mathf.Clamp(transform.position.z, -130, 82));
    }

    void SpectatorController() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            spectatorSpeed = 2;
        } else {
            spectatorSpeed = 0.5f;
        }
        if (Input.GetKey(KeyCode.W)) {
            cam.transform.position += cam.transform.forward * spectatorSpeed;
        } else if (Input.GetKey(KeyCode.S)) {
            cam.transform.position -= cam.transform.forward * spectatorSpeed;
        }
        if (Input.GetKey(KeyCode.D)) {
            cam.transform.position += cam.transform.right * spectatorSpeed;
        } else if (Input.GetKey(KeyCode.A)) {
            cam.transform.position -= cam.transform.right * spectatorSpeed;
        }
        if (Input.GetKey(KeyCode.E)) {
            cam.transform.position += cam.transform.up * spectatorSpeed;
        } else if (Input.GetKey(KeyCode.Q)) {
            cam.transform.position -= cam.transform.up * spectatorSpeed;
        }
    }

    void PlayerController() {
        transform.localRotation = Quaternion.Euler(0f, cam.transform.localEulerAngles.y, 0f);
        float axis = Input.GetAxis("Horizontal");
        float axis2 = Input.GetAxis("Vertical");
        //moveDir = new Vector3(axis, 0f, axis2);
        /*moveDir = transform.TransformDirection(moveDir);
        moveDir *= moveSpeed;*/
        if (Input.GetButton("Run") && controller.isGrounded) {
            moveDir *= runMultiplier;
            if (cam.fieldOfView < 80f) {
                cam.fieldOfView += (80f - cam.fieldOfView) / 0.1f * Time.deltaTime;
            }
        } else if (Input.GetButton("Walk") && controller.isGrounded) {
            moveDir *= crouchMultiplier;
        } else if (cam.fieldOfView > 70f) {
            cam.fieldOfView -= Mathf.Abs(70f - cam.fieldOfView) / 0.1f * Time.deltaTime;
        }
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 70, 80);
        if ((Input.GetButtonUp("Fire1") || Input.GetButtonUp("LeftHand")) && leftArmObject) {
            //setObjectPosition(leftArm.Find("hand").transform.position + leftArm.Find("hand").transform.forward * 2f, transform.rotation * Quaternion.Euler(cam.transform.rotation.eulerAngles.x - 30f, 0f, 0f));
            setObjectGravity(true);
            setObjectCollisions(true/*, leftArmObject.gameObject*/);
            if (leftArmPickup) {
                leftArmPickup.SetBeingHeld(false, this);
            }
            if (leftArmObject.gameObject.layer == 8) {
                leftArmObject.gameObject.layer = 0;
            }

            // System Scenes
            if (SceneManager.GetSceneAt(0).buildIndex == 3) {
                leftArmObject.GetComponent<Rigidbody>().velocity = (leftArmObject.position - leftArmObjectLastPosition) * speedThrow;
                leftArmObjectLastPosition = Vector3.zero;
            }

            leftArmObject = null;
            leftArmPickup = null;
        }
        if ((Input.GetButtonUp("Fire2") || Input.GetButtonUp("RightHand")) && rightArmObject) {
            //setObjectPosition(rightArm.Find("hand").transform.position + rightArm.Find("hand").transform.forward * 2f, transform.rotation * Quaternion.Euler(cam.transform.rotation.eulerAngles.x - 30f, 0f, 0f));
            setObjectGravity(true);
            setObjectCollisions(true/*, rightArmObject.gameObject*/);
            rightArmPickup.SetBeingHeld(false, this);
            if (rightArmObject.gameObject.layer == 8)
            {
                rightArmObject.gameObject.layer = 0;
            }

            // System Scenes
            if (SceneManager.GetSceneAt(0).buildIndex == 3) {
                rightArmObject.GetComponent<Rigidbody>().velocity = (rightArmObject.position - rightArmObjectLastPosition) * speedThrow;
                rightArmObjectLastPosition = Vector3.zero;
            }

            rightArmObject = null;
            rightArmPickup = null;
        }

        // Left Mouse Button
        if (Input.GetButtonDown("LeftHand")) {
            if (leftArm == null) {
                leftArm = Instantiate(arm, transform.position - transform.right + transform.up + transform.forward, transform.rotation * Quaternion.Euler(cam.transform.rotation.eulerAngles.x, 0f, 0f));
            } else {
                leftArm.position = transform.position - transform.right + transform.up + transform.forward;
                leftArm.rotation = transform.rotation * Quaternion.Euler(cam.transform.rotation.eulerAngles.x, 0f, 0f);
                SetArmState(true, leftArm.gameObject);
            }
        } else if (Input.GetButton("LeftHand") && leftArm) {
            if (cam.transform.rotation.eulerAngles.x <= maxCameraAngleFromZero) {
                armForwardBasedOnRotation = armExtraReach * (cam.transform.rotation.eulerAngles.x / maxCameraAngleFromZero);
            } else if (cam.transform.rotation.eulerAngles.x >= 360f - maxCameraAngleFromZero) {
                armForwardBasedOnRotation = armExtraReach * (Mathf.Abs(360f - cam.transform.rotation.eulerAngles.x) / maxCameraAngleFromZero);
            }
            leftArm.position = Vector3.Lerp(leftArm.position, transform.position - transform.right + transform.up * /*1.5f*/1.0f + transform.forward/* * 0.3f*/ + cam.transform.forward * armForwardBasedOnRotation, 25f * Time.deltaTime);
            leftArm.rotation = Quaternion.Lerp(leftArm.rotation, transform.rotation * Quaternion.Euler(cam.transform.rotation.eulerAngles.x - 15f, 0f, 0f), 20f * Time.deltaTime);
        }
        else if (Input.GetButtonUp("LeftHand") && leftArm != null) {
            SetArmState(false, leftArm.gameObject);
        }

        if (Input.GetButton("Fire1") && Input.GetButton("LeftHand"))
        {
            RaycastHit raycastHit;
            if (!leftHandle)
            {
                if (leftArmObject)
                {
                    if (leftArmPickup)
                    {
                        if (!leftArmPickup.beingUsed)
                        {
                            Vector3 vector = leftArm.Find("hand").transform.position + leftArm.Find("hand").transform.forward * 2f * leftArmPickup.heldPositionOffset.z + leftArm.Find("hand").transform.right * leftArmPickup.heldPositionOffset.x + leftArm.Find("hand").transform.up * leftArmPickup.heldPositionOffset.y;
                            Quaternion quaternion = transform.rotation * Quaternion.Euler(cam.transform.rotation.eulerAngles.x + leftArmPickup.heldRotateXOffset, 0f, 0f);
                            leftArmObject.position = Vector3.Lerp(leftArmObject.position, vector, 30f * Time.deltaTime);
                            leftArmObject.rotation = Quaternion.Lerp(leftArmObject.rotation, quaternion, 30f * Time.deltaTime);
                        }
                        else
                        {
                            Vector3 vector2 = cam.transform.position + cam.transform.forward * 1.5f + cam.transform.right * (leftArmPickup.heldPositionOffset.x * 0.3f);
                            Quaternion quaternion2 = transform.rotation * Quaternion.Euler(cam.transform.rotation.eulerAngles.x, 0f, 0f);
                            leftArmObject.position = Vector3.Lerp(leftArmObject.position, vector2, 30f * Time.deltaTime);
                            leftArmObject.rotation = Quaternion.Lerp(leftArmObject.rotation, quaternion2, 30f * Time.deltaTime);
                        }
                    }
                    else
                    {
                        Vector3 vector3 = leftArm.Find("hand").transform.position + leftArm.Find("hand").transform.forward * 2f;
                        Quaternion quaternion3 = transform.rotation * Quaternion.Euler(cam.transform.rotation.eulerAngles.x - 30f, 0f, 0f);
                        leftArmObject.position = Vector3.Lerp(leftArmObject.position, vector3, 30f * Time.deltaTime);
                        leftArmObject.rotation = Quaternion.Lerp(leftArmObject.rotation, quaternion3, 30f * Time.deltaTime);
                    }
                    if (leftArmObject.GetComponent<Rigidbody>() && !leftArmObject.GetComponent<Rigidbody>().isKinematic)
                    {
                        leftArmObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        leftArmObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    }
                }
                else if (Physics.SphereCast(leftArm.Find("hand").transform.position, 0.2f, leftArm.transform.forward, out raycastHit, 3.75f/*4.75f, layerMask*/) && (raycastHit.transform.gameObject.tag.Contains("Physics") || raycastHit.transform.root.tag.Contains("Physics")))
                {
                    if (raycastHit.transform.gameObject.tag.Contains("Physics"))
                    {
                        leftArmObject = raycastHit.transform;
                    }
                    else if (raycastHit.transform.root.tag.Contains("Physics"))
                    {
                        leftArmObject = raycastHit.transform.root;
                    }
                    if (leftArmObject.gameObject.layer == 0)
                    {
                        leftArmObject.gameObject.layer = 8;
                    }
                    setObjectGravity(false);
                    leftArmPickup = leftArmObject.GetComponent<PickupObject>();
                    leftArmPickup.ResetHeldRotation();
                    leftArmPickup.SetBeingHeld(true, this);
                }
            }

            // Handle
            if (Physics.SphereCast(leftArm.Find("hand").transform.position, 0.2f, leftArm.transform.forward, out raycastHit, 3.75f)) {
                if (raycastHit.transform.GetComponent<HandleScript>()) {
                    leftHandle = raycastHit.transform.GetComponent<HandleScript>();
                }
                if (raycastHit.transform.GetComponent<LightSwitch>()) {
                    raycastHit.transform.GetComponent<LightSwitch>().Switch();
                }
            }
            if (leftHandle) {
                leftHandle.DragHandle(leftArm.position + leftArm.forward);
            }
        }

        // Right Mouse Button
        if (Input.GetButtonDown("RightHand")) {
            if (rightArm == null) {
                rightArm = Instantiate(arm, transform.position + transform.right + transform.up + transform.forward, transform.rotation * Quaternion.Euler(cam.transform.rotation.eulerAngles.x, 0f, 0f));
            } else {
                rightArm.position = transform.position + transform.right + transform.up + transform.forward;
                rightArm.rotation = transform.rotation * Quaternion.Euler(cam.transform.rotation.eulerAngles.x, 0f, cam.transform.rotation.eulerAngles.z);
                SetArmState(true, rightArm.gameObject);
            }
        } else if (Input.GetButton("RightHand") && rightArm) {
            if (cam.transform.rotation.eulerAngles.x <= maxCameraAngleFromZero) {
                armForwardBasedOnRotation = armExtraReach * (cam.transform.rotation.eulerAngles.x / maxCameraAngleFromZero);
            } else if (cam.transform.rotation.eulerAngles.x >= 360f - maxCameraAngleFromZero) {
                armForwardBasedOnRotation = armExtraReach * (Mathf.Abs(360f - cam.transform.rotation.eulerAngles.x) / maxCameraAngleFromZero);
            }
            if (!rightArmObject || !rightArmPickup.IsBeingUsed()) {
                rightArm.position = Vector3.Lerp(rightArm.position, transform.position + transform.right + transform.up * /*1.5f*/1.0f + transform.forward/* * 0.3f*/ + cam.transform.forward * armForwardBasedOnRotation, 25f * Time.deltaTime);
                rightArm.rotation = Quaternion.Lerp(rightArm.rotation, transform.rotation * Quaternion.Euler(cam.transform.rotation.eulerAngles.x - 15f, 0f, 0f), 20f * Time.deltaTime);
            } else if (rightArmObject && rightArmPickup.IsBeingUsed()) {
                rightArm.position = Vector3.Lerp(rightArm.position, transform.position + transform.right * 1.6f + transform.up * 1.5f + transform.forward + -cam.transform.forward * 1f, 25f * Time.deltaTime);
                rightArm.rotation = Quaternion.Lerp(rightArm.rotation, transform.rotation * Quaternion.Euler(cam.transform.rotation.eulerAngles.x, 0f, 0f), 20f * Time.deltaTime);
            }
        } else if (Input.GetButtonUp("RightHand") && rightArm != null) {
            SetArmState(false, rightArm.gameObject);
        }

        if (Input.GetButton("Fire2") && Input.GetButton("RightHand"))
        {
            RaycastHit raycastHit2;
            if (!rightHandle)
            {
                if (rightArmObject)
                {
                    if (rightArmPickup)
                    {
                        if (!rightArmPickup.beingUsed)
                        {
                            Vector3 vector4 = rightArm.Find("hand").transform.position + rightArm.Find("hand").transform.forward * 2f * rightArmPickup.heldPositionOffset.z + rightArm.Find("hand").transform.right * -rightArmPickup.heldPositionOffset.x + rightArm.Find("hand").transform.up * rightArmPickup.heldPositionOffset.y;
                            Quaternion quaternion4 = transform.rotation * Quaternion.Euler(cam.transform.rotation.eulerAngles.x + rightArmPickup.heldRotateXOffset, 0f, 0f);
                            rightArmObject.position = Vector3.Lerp(rightArmObject.position, vector4, 30f * Time.deltaTime);
                            rightArmObject.rotation = Quaternion.Lerp(rightArmObject.rotation, quaternion4, 30f * Time.deltaTime);
                        }
                        else
                        {
                            Vector3 vector5 = cam.transform.position + cam.transform.forward * 1.5f + cam.transform.right * (rightArmPickup.heldPositionOffset.x * 0.3f);
                            Quaternion quaternion5 = transform.rotation * Quaternion.Euler(cam.transform.rotation.eulerAngles.x, 0f, 0f);
                            rightArmObject.position = Vector3.Lerp(rightArmObject.position, vector5, 30f * Time.deltaTime);
                            rightArmObject.rotation = Quaternion.Lerp(rightArmObject.rotation, quaternion5, 30f * Time.deltaTime);
                        }
                    }
                    else
                    {
                        Vector3 vector6 = rightArm.Find("hand").transform.position + rightArm.Find("hand").transform.forward * 2f;
                        Quaternion quaternion6 = transform.rotation * Quaternion.Euler(cam.transform.rotation.eulerAngles.x - 30f, 0f, 0f);
                        rightArmObject.position = Vector3.Lerp(rightArmObject.position, vector6, 30f * Time.deltaTime);
                        rightArmObject.rotation = Quaternion.Lerp(rightArmObject.rotation, quaternion6, 30f * Time.deltaTime);
                    }
                    if (rightArmObject.GetComponent<Rigidbody>() && !rightArmObject.GetComponent<Rigidbody>().isKinematic)
                    {
                        rightArmObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        rightArmObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    }
                }
                else if (Physics.SphereCast(rightArm.Find("hand").transform.position, 0.2f, rightArm.transform.forward, out raycastHit2, 3.75f/*4.75f, layerMask*/) && (raycastHit2.transform.gameObject.tag.Contains("Physics") || raycastHit2.transform.root.tag.Contains("Physics")))
                {
                    if (raycastHit2.transform.gameObject.tag.Contains("Physics"))
                    {
                        rightArmObject = raycastHit2.transform;
                    }
                    else if (raycastHit2.transform.root.tag.Contains("Physics"))
                    {
                        rightArmObject = raycastHit2.transform.root;
                    }
                    if (rightArmObject.gameObject.layer == 0)
                    {
                        rightArmObject.gameObject.layer = 8;
                    }
                    setObjectGravity(false);
                    rightArmPickup = rightArmObject.GetComponent<PickupObject>();
                    rightArmPickup.ResetHeldRotation();
                    rightArmPickup.SetBeingHeld(true, this);
                }
            }

            // Handle
            if (Physics.SphereCast(rightArm.Find("hand").transform.position, 0.2f, rightArm.transform.forward, out raycastHit2, 3.75f)) {
                if (raycastHit2.transform.GetComponent<HandleScript>()) {
                    rightHandle = raycastHit2.transform.GetComponent<HandleScript>();
                }
                if (raycastHit2.transform.GetComponent<LightSwitch>()) {
                    raycastHit2.transform.GetComponent<LightSwitch>().Switch();
                }
            }
            if (rightHandle) {
                rightHandle.DragHandle(rightArm.position + rightArm.forward);
            }
        }
        RaycastHit raycastHit3;
        if (Input.GetButtonDown("Fire2") && Input.GetButton("RightHand") && Physics.SphereCast(rightArm.Find("hand").transform.position + rightArm.Find("hand").transform.right * 0.5f, 0.5f, rightArm.transform.forward, out raycastHit3, 15f, layerMask) && raycastHit3.transform.gameObject.tag == "NPC")
        {
            NPC component = raycastHit3.transform.GetComponent<NPC>();
            if (!component.isFollowingPlayer)
            {
                component.FollowAPlayer(gameObject);
            }
            else
            {
                component.FollowNoPlayer();
            }
        }
        
        if ((Input.GetButtonUp("Fire1") || Input.GetButtonUp("LeftHand")) && leftHandle)
        {
            // Handle
            leftHandle = null;
        }
        if ((Input.GetButtonUp("Fire2") || Input.GetButtonUp("RightHand")) && rightHandle)
        {
            // Handle
            rightHandle = null;
        }

        /*gravityToApply += gravity;
        moveDir.y = moveDir.y - gravityToApply;
        moveDir *= Time.deltaTime;
        controller.Move(moveDir);*/
        if (gameObject.GetComponent<CharacterController>().enabled)
        {
            if (controller.isGrounded)
            {
                //gravityToApply = 0f;
                moveDir = new Vector3(axis, 0f, axis2);
                moveDir = transform.TransformDirection(moveDir);
                moveDir *= moveSpeed;
                if (Input.GetButtonDown("Jump"))
                {
                    moveDir.y = 10;
                }
            }
            moveDir.y -= gravity * Time.deltaTime * 2;
            controller.Move(moveDir * Time.deltaTime);
        }

        // Fixes
        if (leftArmObject && leftArmObject.parent && leftArmObject.parent.name == "burger-bottom") {
            leftArmObject.GetComponent<Food>().SetParent(leftArmObject);
            leftArmObject = null;
        }
        if (rightArmObject && rightArmObject.parent && rightArmObject.parent.name == "burger-bottom") {
            rightArmObject.GetComponent<Food>().SetParent(rightArmObject);
            rightArmObject = null;
        }
        if (leftArmObject && leftArmObject.GetComponent<Food>() && leftArmObject.GetComponent<Food>().beingHeldByRat) {
            leftArmObject.GetComponent<Food>().SetParent(leftArmObject);
            leftArmObject = null;
        }
        if (rightArmObject && rightArmObject.GetComponent<Food>() && rightArmObject.GetComponent<Food>().beingHeldByRat) {
            rightArmObject.GetComponent<Food>().SetParent(rightArmObject);
            rightArmObject = null;
        }

        // System Scenes
        if (SceneManager.GetSceneAt(0).buildIndex == 3) {
            // Set Last Object Position
            if (leftArmObject) {
                leftArmObjectLastPosition = Vector3.Lerp(leftArmObjectLastPosition, leftArmObject.position, 0.8f);
            }
            if (rightArmObject) {
                rightArmObjectLastPosition = Vector3.Lerp(rightArmObjectLastPosition, rightArmObject.position, 0.8f);
            }
        }

        // Cursor
        lastDrawPosition = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2);
    }

    Vector3 leftArmObjectLastPosition;
    Vector3 rightArmObjectLastPosition;
    float speedThrow = 50;

    void OnDrawGizmos() {
        if (leftArm) {
            Gizmos.DrawWireSphere(leftArm.position + leftArm.forward * 3.75f, 0.4f);
        }
        if (rightArm) {
            Gizmos.DrawWireSphere(rightArm.position + rightArm.forward * 3.75f, 0.4f);
        }
    }

    /*private void OnPlayerConnected(NetworkPlayer player)
    {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Arm"))
        {
            print("Found arm");
            networkView.RPC("SetArmState", 1, new object[]
            {
                gameObject.networkView.viewID,
                gameObject.GetComponent<Collider>().GetComponent<MeshRenderer>().enabled
            });
        }
    }*/

    private void SetArmState(bool active, GameObject arm)
    {
        GameObject gameObject1 = arm;
        GameObject gameObject2 = gameObject1.transform.GetChild(0).gameObject;
        gameObject1.GetComponent<MeshRenderer>().enabled = active;
        gameObject2.GetComponent<MeshRenderer>().enabled = active;
        gameObject2.GetComponent<BoxCollider>().enabled = active;
    }

    private void DestroyObject(GameObject id)
    {
        GameObject gameObject = id.gameObject;
        if (gameObject.tag.Equals("Arm"))
        {
            //Network.RemoveRPCs(gameObject.transform.GetChild(0).networkView.viewID);
            Destroy(gameObject.transform.GetChild(0).gameObject);
        }
        //Network.RemoveRPCs(id);
        Destroy(gameObject);
    }

    private void setObjectGravity(bool grav)
    {
        Transform transform = gameObject.transform;
        if (transform.name.Equals("PlateModel") || transform.name.Equals("burger-bottom"))
        {
            transform = transform.parent;
        }
        if (transform.GetComponent<Rigidbody>())
        {
            transform.GetComponent<Rigidbody>().useGravity = grav;
        }
    }

    private void setObjectKematic(bool kematic)
    {
        Transform transform = gameObject.transform;
        if (transform.name.Equals("PlateModel") || transform.name.Equals("burger-bottom"))
        {
            transform = transform.parent;
        }
        transform.GetComponent<Rigidbody>().isKinematic = kematic;
    }

    public void setObjectCollisions(bool collide/*, GameObject id*/)
    {
        Transform transform = /*id.*/gameObject.transform;
        if (transform.name.Equals("PlateModel") || transform.name.Equals("burger-bottom"))
        {
            transform = transform.parent;
        }
        if (!transform.name.Contains("notepad") && !transform.name.Contains("StaffMenu"))
        {
            transform.GetComponent<Collider>().enabled = collide;
            if (transform.GetComponent<Collider>().isTrigger && !collide)
            {
                print("Error: unexpected isTrigger state");
            }
            //transform.GetComponent<Collider>().isTrigger = !collide;
        }
    }

    private void setObjectPosition(Vector3 pos, Quaternion rot) {
        Transform transform = gameObject.transform;
        if (transform.name.Equals("PlateModel") || transform.name.Equals("burger-bottom"))
        {
            transform = transform.parent;
        }
        if (transform.GetComponent<Rigidbody>() && !transform.GetComponent<Rigidbody>().isKinematic)
        {
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        transform.position = Vector3.Lerp(transform.position, pos, 30f * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, 30f * Time.deltaTime);
    }

    private void SetUsername(string username) {
        TextMesh component = gameObject.GetComponent<TextMesh>();
        component.transform.root.GetComponent<FirstPersonControl>().username = username;
        component.text = username;
    }
}
