using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    /*private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            print("Level 1 was loaded");
            Spawn();
            //Network.isMessageQueueRunning = true;
            //if (Network.isServer)
            //{
            //    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Spawner"))
            //    {
            //        gameObject.GetComponent<SpawnNPC>().enabled = true;
            //    }
            //}
        }
        Cursor.visible = true;
    }*/

    private void OnDisconnectedFromServer(/*NetworkDisconnection info*/)
    {
        /*if (Network.isServer)
        {
            disconnectedMessage = "Local server connection disconnected.";
            Debug.Log("Local server connection disconnected");
        }
        else if (info == 20)
        {
            disconnectedMessage = "Lost connection to server.";
            Debug.Log("Lost connection to the server");
        }
        else
        {
            disconnectedMessage = "Disconnected from server.";
            Debug.Log("Successfully diconnected from the server");
        }*/
        disconnected = true;
        menu component = Camera.main.GetComponent<menu>();
        component.enabled = true;
        component.currentMenu = menu.MenuGUIStates.pauseMenu;
    }

    /*private void OnPlayerDisconnected(NetworkPlayer player)
    {
        Screen.lockCursor = false;
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }*/

    private void OnGUI()
    {
        if (disconnected)
        {
            GUI.skin.label.normal.textColor = Color.white;
            GUI.Label(new Rect((float)(Screen.width / 2 - 200), 200f, 400f, 100f), disconnectedMessage);
        }
    }

    public void Spawn()
    {
        print("Spawning!");
        Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        Camera.main.transform.position = Vector3.zero;
        Transform transform = /*Network.*/Instantiate(player, gameObject.transform.position, gameObject.transform.rotation/*, 0*/);
        /*if (PlayerPrefs.GetString("Username") == "Kritz")
        {
            networkView.RPC("SetPlayerTexture", 6, new object[]
            {
                transform.networkView.viewID,
                "Kritz"
            });
        }
        else
        {
            networkView.RPC("SetPlayerTexture", 6, new object[]
            {
                transform.networkView.viewID,
                Random.Range(1, 8) + "staff"
            });
        }*/
        menu component = Camera.main.GetComponent<menu>();
        MouseLook component2 = Camera.main.GetComponent<MouseLook>();
        FollowGameObject component3 = Camera.main.GetComponent<FollowGameObject>();
        foreach (object obj in transform.transform)
        {
            Transform transform2 = (Transform)obj;
            foreach (object obj2 in transform2.transform)
            {
                Transform transform3 = (Transform)obj2;
                transform3.GetComponent<MeshRenderer>().enabled = false;
            }
            transform2.GetComponent<MeshRenderer>().enabled = false;
        }
        component.enabled = false;
        component.currentMenu = menu.MenuGUIStates.pauseMenu;
        component2.enabled = true;
        component3.follow = transform.gameObject;
        component3.enabled = true;
        /*transform.networkView.RPC("SetUsername", 6, new object[]
        {
            transform.GetChild(0).FindChild("Username").networkView.viewID,
            PlayerPrefs.GetString("Username")
        });
        component.networkView.RPC("GetPlayerUsername", 0, new object[]
        {
            Network.player.externalIP,
            PlayerPrefs.GetString("Username")
        });*/
        /*if (PlayerPrefs.GetFloat("sensitivity") == 0f)
        {
            PlayerPrefs.SetFloat("sensitivity", 10f);
        }
        component2.sensitivityX = PlayerPrefs.GetFloat("sensitivity");
        component2.sensitivityY = component2.sensitivityX;*/
        FirstPersonControl component4 = transform.GetComponent<FirstPersonControl>();
        component4.cam = Camera.main;
        Cursor.visible = true;
    }

    //[RPC]
    private void SetPlayerTexture(/*NetworkViewID playerID, */string textureName)
    {
        Transform transform = /*NetworkView.Find(playerID)*/gameObject.transform;
        transform.GetComponent<MeshRenderer>().material = (Resources.Load("Skins/Materials/" + textureName) as Material);
    }

    public Transform player;

    private bool disconnected;

    private string disconnectedMessage = string.Empty;
}
