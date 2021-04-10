using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu : MonoBehaviour
{
    /*private void Awake()
    {
        MasterServer.ipAddress = IPAddress;
        MasterServer.port = MasterPort;
        Network.natFacilitatorIP = IPAddress;
        Network.natFacilitatorPort = FacilitatorPort;
        MasterServer.RequestHostList(versionName);
        username = PlayerPrefs.GetString("Username");
        fontEras = (Resources.Load("Text/ERASBD") as Font);
        GetLogin();
        port = string.Empty + ListenPort;
    }*/

    //[RPC]
    private void GetPlayerUsername(string ip, string newUsername)
    {
        print(newUsername + " reported ip of " + ip);
        int num = PlayerIPs.IndexOf(ip);
        Players[num] = newUsername;
    }

    /*private void OnPlayerConnected(NetworkPlayer player)
    {
        if (BannedPlayers.IndexOf(player.ipAddress) != -1)
        {
            Network.CloseConnection(player, true);
        }
        else
        {
            PlayerIPs.Add(player.ipAddress);
            Players.Add("error: waiting for username");
        }
    }*/

    /*private void OnServerInitialized()
    {
        StopAllCoroutines();
        if (Application.loadedLevel == 0)
        {
            Application.LoadLevel(1);
        }
        else
        {
            GameObject.Find("Spawn").GetComponent<SpawnPlayer>().Spawn();
        }
    }*/

    /*private void OnConnectedToServer()
    {
        StopAllCoroutines();
        Network.isMessageQueueRunning = false;
        Application.LoadLevel(1);
    }

    private void Update()
    {
        if (host == null || host.Length <= 0)
        {
            host = MasterServer.PollHostList();
        }
        if (loginStartTime > 0f && loginStartTime + loginTimeoutDuration < Time.time)
        {
            loginMessage += "\n\nConnection to the master server is taking a while.\n The website may be under heavy load.\n";
            loginStartTime = 0f;
        }
    }

    private void GetLogin()
    {
        Application.ExternalCall("UnityShake", new object[0]);
    }*/

    private void GiveUsername(string webUsername)
    {
        username = webUsername;
        print(webUsername);
        gotUsernameFromWebsite = true;
        PlayerPrefs.SetString("Username", username);
    }

    /*private void RefreshPings()
    {
        if (host.Length > 0)
        {
            StartCoroutine(PingBatch(host));
        }
    }

    private IEnumerator PingBatch(HostData[] hostsToPing)
    {
        pingTimes = new Ping[host.Length];
        int index = 0;
        foreach (HostData server in hostsToPing)
        {
            string IP = server.ip[0];
            StartCoroutine(PingHost(IP, index));
            index++;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
        }
        yield break;
    }*/

    private IEnumerator PingHost(string IP, int index)
    {
        float timeStarted = Time.time;
        Ping p = new Ping(IP);
        while (!p.isDone && Time.time - timeStarted < 10f)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
        print(string.Concat(new object[]
        {
            "Pinged ",
            IP,
            ": ",
            p.time
        }));
        pingTimes[index] = p;
        yield break;
    }

    //[System.Obsolete]
    private IEnumerator Authenticate()
    {
        /*WWWForm form = new WWWForm();
        form.AddField("unity", 1);
        form.AddField("username", username);
        form.AddField("password", password);
        WWW download = new WWW(loginURL, form);
        yield return download;
        if (!string.IsNullOrEmpty(download.error))
        {
            print("Error downloading: " + download.error);
        }
        else
        {
            print("Request success");
            if (download.text.Contains("success"))
            {
                print("User authenticated");
                userAuthenticated = true;
                //MasterServer.RequestHostList(versionName);
                //RefreshPings();
                timerStartLooking = Time.time;
                currentMenu = MenuGUIStates.searching;
            }
            else
            {
                loginMessage = "Incorrect username or password";
            }
            loginStartTime = 0f;
        }*/
        yield break;
    }

    private void OnGUI()
    {
        GUI.skin = guiSkin;
        GUIStyle guistyle = new GUIStyle();
        guistyle.font = fontEras;
        guistyle.normal.textColor = Color.white;
        guistyle.fontSize = 64;
        //guistyle.alignment = 4;
        Event current = Event.current;
        if (currentMenu == MenuGUIStates.username)
        {
            guistyle.normal.textColor = Color.Lerp(Color.black, Color.grey, 0.1f);
            //guistyle.alignment = 4;
            GUI.Label(new Rect((Screen.width / 2 - 48), (Screen.height / 20 - 3), 100f, 25f), "CITIZEN BURGER DISORDER", guistyle);
            guistyle.normal.textColor = Color.white;
            GUI.Label(new Rect((Screen.width / 2 - 50), (Screen.height / 20), 100f, 25f), "CITIZEN BURGER DISORDER", guistyle);
            guistyle.fontSize = 20;
            guistyle.normal.textColor = Color.black;
            GUI.Label(new Rect((Screen.width - 112), (Screen.height - 32), 100f, 25f), "V" + versionName.Substring(versionName.IndexOf("CBD") + 4), guistyle);
            guistyle.normal.textColor = Color.white;
            GUI.Label(new Rect((Screen.width - 110), (Screen.height - 30), 100f, 25f), "V" + versionName.Substring(versionName.IndexOf("CBD") + 4), guistyle);
            guistyle.fontSize = 40;
            if (gotUsernameFromWebsite || username.Equals("!"))
            {
                //guistyle.alignment = 5;
                GUI.Label(new Rect((Screen.width / 2 + 2 - 325), (Screen.height / 20 + 120), 100f, 25f), "Password:", guistyle);
                //guistyle.alignment = 4;
                guistyle.normal.textColor = Color.Lerp(Color.black, Color.grey, 0.1f);
                GUI.Label(new Rect((Screen.width / 2 + 2 - 50), (Screen.height / 20 + 75), 100f, 25f), username, guistyle);
                guistyle.normal.textColor = Color.white;
                GUI.Label(new Rect((Screen.width / 2 - 50), (Screen.height / 20 + 75), 100f, 25f), username, guistyle);
                if (GUI.Button(new Rect(Screen.width / 2f + 200f, (Screen.height / 20 + 75), 100f, 43f), "Change"))
                {
                    gotUsernameFromWebsite = false;
                }
            }
            else
            {
                //guistyle.alignment = 5;
                GUI.Label(new Rect((Screen.width / 2 + 2 - 325), (Screen.height / 20 + 75), 100f, 25f), "Username:", guistyle);
                GUI.Label(new Rect((Screen.width / 2 + 2 - 325), (Screen.height / 20 + 120), 100f, 25f), "Password:", guistyle);
                //guistyle.alignment = 4;
                GUI.skin.textField.fontSize = 42;
                //GUI.skin.textField.alignment = 4;
                username = GUI.TextField(new Rect((Screen.width / 2 - 200), (Screen.height / 20 + 70), 400f, 45f), username, 25);
            }
            //GUI.skin.textField.alignment = 4;
            //password = GUI.PasswordField(new Rect((Screen.width / 2 - 200), (Screen.height / 20 + 120), 400f, 42f), password, "*".get_Chars(0));
            if ((loginStartTime == 0f/* && current.keyCode == 13*/) || GUI.Button(new Rect(Screen.width / 2f + 200f, (Screen.height / 20 + 119), 100f, 43f), "Login"))
            {
                loginMessage = "Logging in...";
                StartCoroutine(Authenticate());
                loginStartTime = Time.time;
            }
            guistyle.fontSize = 38;
            GUI.Label(new Rect((Screen.width / 2 + 2 - 50), (Screen.height / 20 + 200), 100f, 25f), loginMessage, guistyle);
            guistyle.fontSize = 48;
        }
        else if (currentMenu == MenuGUIStates.searching)
        {
            if (PlayerPrefs.GetString("Username") != username)
            {
                PlayerPrefs.SetString("Username", username);
                print("Updated username to " + username);
            }
            string text = "Lookin'...";
            currentMenu = MenuGUIStates.serverlist;
            guistyle.normal.textColor = Color.Lerp(Color.black, Color.grey, 0.1f);
            GUI.Label(new Rect((Screen.width / 2 + 2 - 50), (Screen.height / 3 - 3), 100f, 25f), text, guistyle);
            guistyle.normal.textColor = Color.white;
            GUI.Label(new Rect((Screen.width / 2 - 50), (Screen.height / 3), 100f, 25f), text, guistyle);
        }
        else if (currentMenu == MenuGUIStates.serverlist)
        {
            /*if (host != null)
            {
                string text2 = "Servers:";
                guistyle.normal.textColor = Color.Lerp(Color.black, Color.grey, 0.1f);
                GUI.Label(new Rect((Screen.width / 2 + 2 - 50), 10f, 100f, 25f), text2, guistyle);
                guistyle.normal.textColor = Color.white;
                GUI.Label(new Rect((Screen.width / 2 - 50), 10f, 100f, 25f), text2, guistyle);
                guistyle.fontSize = 26;
                if (host.Length > 6)
                {
                    baseServerIndex = GUI.VerticalScrollbar(new Rect((Screen.width - 150), 60f, 20f, Screen.height / 1.2f), baseServerIndex, 1f, -1f, (host.Length - 1));
                }
                else
                {
                    baseServerIndex = -1f;
                }
                guistyle.alignment = 3;
                guistyle.fontSize = 14;
                GUI.Label(new Rect((Screen.width / 2 - 220), 60f, 250f, 25f), "Server name", guistyle);
                GUI.Label(new Rect((Screen.width / 2 + 100), 60f, 250f, 25f), "Players", guistyle);
                GUI.Label(new Rect((Screen.width / 2 + 200), 60f, 250f, 25f), "Ping", guistyle);
                guistyle.alignment = 4;
                guistyle.fontSize = 26;
                int num = 0;
                foreach (HostData hostData in host)
                {
                    bool flag = false;
                    string gameName = hostData.gameName;
                    string text3 = string.Empty + hostData.connectedPlayers;
                    string text4 = string.Empty;
                    float num2 = -1f;
                    if (pingTimes != null && pingTimes.Length > num && pingTimes[num] != null && pingTimes[num].time > 0)
                    {
                        text4 = string.Empty + pingTimes[num].time + "ms";
                        num2 = pingTimes[num].time;
                    }
                    else if (pingTimes != null && pingTimes.Length > num && pingTimes[num] != null && pingTimes[num].time == -1)
                    {
                        text4 += "???";
                    }
                    else
                    {
                        text4 += "...";
                    }
                    if (joiningServerName == gameName)
                    {
                        flag = true;
                    }
                    guistyle.alignment = 3;
                    if (!flag || (flag && ServerPassword.Length <= 0))
                    {
                        GUI.Label(new Rect((Screen.width / 2 - 220), (-baseServerIndex + num) * 50f + 60f, 250f, 25f), gameName, guistyle);
                        GUI.Label(new Rect((Screen.width / 2 + 100), (-baseServerIndex + num) * 50f + 60f, 250f, 25f), text3, guistyle);
                        if (num2 > 300f)
                        {
                            guistyle.normal.textColor = Color.Lerp(Color.red, Color.black, 0.5f);
                        }
                        else if (num2 > 200f)
                        {
                            guistyle.normal.textColor = Color.red;
                        }
                        else if (num2 > 100f)
                        {
                            guistyle.normal.textColor = Color.yellow;
                        }
                        else if (num2 > 1f)
                        {
                            guistyle.normal.textColor = Color.green;
                        }
                        else if (num2 < 0f)
                        {
                            guistyle.normal.textColor = Color.grey;
                        }
                        GUI.Label(new Rect((Screen.width / 2 + 200), (-baseServerIndex + num) * 50f + 60f, 250f, 25f), text4, guistyle);
                        guistyle.normal.textColor = Color.white;
                        guistyle.alignment = 4;
                    }
                    if (!flag)
                    {
                        if (GUI.Button(new Rect((Screen.width / 2 - 355), (-baseServerIndex + num) * 50f + 62f, 60f, 25f), "Join"))
                        {
                            joiningServerName = gameName;
                            if (hostData.comment.Length <= 1)
                            {
                                Network.Connect(hostData);
                            }
                        }
                        if (username == "Kritz")
                        {
                            string text5 = string.Empty;
                            if (hostData.comment.Substring(0, 1) == "0")
                            {
                                text5 = "Locked to admins";
                            }
                            else
                            {
                                text5 = "Allows admins";
                            }
                            GUI.Label(new Rect((Screen.width / 2 + 400), (-baseServerIndex + num) * 50f + 62f, 60f, 25f), text5);
                        }
                    }
                    else if (hostData.comment.Length > 1)
                    {
                        if (!joiningPasswordProtected)
                        {
                            GUI.skin.textField.fontSize = 30;
                            GUI.skin.textField.alignment = 4;
                            ServerPassword = GUI.TextField(new Rect((Screen.width / 2 - 225), (-baseServerIndex + num) * 50f + 60f, 500f, 30f), ServerPassword, 25);
                        }
                        if ((ServerPassword.Length > 0 && ServerPassword == hostData.comment.Substring(1)) || (hostData.comment.Substring(0, 1).Equals("1") && username.Equals("Kritz")))
                        {
                            if (!joiningPasswordProtected)
                            {
                                if (GUI.Button(new Rect((Screen.width / 2 - 355), (-baseServerIndex + num) * 50f + 62f, 60f, 25f), "Alright!"))
                                {
                                    joiningServerName = gameName;
                                    joiningPasswordProtected = true;
                                    Network.Connect(hostData);
                                }
                            }
                            else
                            {
                                GUI.Label(new Rect((Screen.width / 2 - 355), (-baseServerIndex + num) * 50f + 62f, 60f, 25f), "Joinin'...");
                            }
                        }
                        else if (ServerPassword.Length > 0 && GUI.Button(new Rect((Screen.width / 2 - 355), (-baseServerIndex + num) * 50f + 62f, 60f, 25f), "Alright!"))
                        {
                            ServerPassword = string.Empty;
                            joiningPasswordProtected = false;
                            flag = false;
                            joiningServerName = string.Empty;
                        }
                    }
                    else
                    {
                        GUI.Label(new Rect((Screen.width / 2 - 355), (-baseServerIndex + num) * 50f + 62f, 60f, 25f), "Joinin'...");
                    }
                    if (hostData.comment.Length > 1)
                    {
                        if (!flag && !joiningPasswordProtected)
                        {
                            GUI.skin.label.fontSize = 12;
                            GUI.Label(new Rect((Screen.width / 2 - 290), (-baseServerIndex + num) * 50f + 62f, 70f, 25f), "(Protected)");
                        }
                        else if (!joiningPasswordProtected)
                        {
                            GUI.skin.label.fontSize = 14;
                            GUI.skin.label.alignment = 3;
                            GUI.Label(new Rect((Screen.width / 2 - 292), (-baseServerIndex + num) * 50f + 62f, 150f, 25f), "Password:");
                        }
                    }
                    num++;
                }
                if (GUI.Button(new Rect((Screen.width - 110), (Screen.height - 60), 100f, 50f), "Create"))
                {
                    currentMenu = MenuGUIStates.create;
                }
                if (GUI.Button(new Rect(10f, (Screen.height - 60), 100f, 50f), "Refresh"))
                {
                    MasterServer.RequestHostList(versionName);
                    host = MasterServer.PollHostList();
                    RefreshPings();
                }
            }
            else
            {
                host = MasterServer.PollHostList();
                RefreshPings();
            }*/
        }
        else if (currentMenu == MenuGUIStates.create)
        {
            guistyle.normal.textColor = Color.Lerp(Color.black, Color.grey, 0.1f);
            GUI.Label(new Rect((Screen.width / 2 + 2 - 50), (Screen.height / 20 - 3), 100f, 25f), "Server Options", guistyle);
            guistyle.normal.textColor = Color.white;
            GUI.Label(new Rect((Screen.width / 2 - 50), (Screen.height / 20), 100f, 25f), "Server Options", guistyle);
            guistyle.fontSize = 42;
            guistyle.normal.textColor = Color.Lerp(Color.black, Color.grey, 0.1f);
            GUI.Label(new Rect((Screen.width / 2 + 2 - 50), Screen.height / 7f + 20f, 100f, 25f), "Server Password?", guistyle);
            guistyle.normal.textColor = Color.white;
            GUI.Label(new Rect((Screen.width / 2 - 50), Screen.height / 7f + 22f, 100f, 25f), "Server Password?", guistyle);
            guistyle.fontSize = 25;
            guistyle.normal.textColor = Color.Lerp(Color.black, Color.grey, 0.1f);
            GUI.Label(new Rect((Screen.width / 2 + 2 - 50), Screen.height / 7f + 100f, 100f, 25f), "(blank for public server)", guistyle);
            guistyle.normal.textColor = Color.white;
            GUI.Label(new Rect((Screen.width / 2 - 50), Screen.height / 7f + 103f, 100f, 25f), "(blank for public server)", guistyle);
            GUI.skin.textField.fontSize = 42;
            //GUI.skin.textField.alignment = 4;
            ServerPassword = GUI.TextField(new Rect((Screen.width / 2 - 200), (Screen.height / 7 + 60), 400f, 45f), ServerPassword, 25);
            GUI.skin.toggle.fontSize = 16;
            string text6 = "  Administrator Access Disabled";
            if (allowAdminAccess)
            {
                text6 = "  Administrator Access Enabled";
            }
            if (ServerPassword.Length > 0)
            {
                allowAdminAccess = GUI.Toggle(new Rect((Screen.width / 2 - 100), (Screen.height / 7 + 130), 300f, 30f), allowAdminAccess, text6);
            }
            else
            {
                allowAdminAccess = true;
            }
            guistyle.normal.textColor = Color.Lerp(Color.black, Color.grey, 0.1f);
            GUI.Label(new Rect((Screen.width / 2 + 2 - 175), Screen.height / 7f + 2f + 165f, 100f, 25f), "Server Port", guistyle);
            guistyle.normal.textColor = Color.white;
            GUI.Label(new Rect((Screen.width / 2 - 175), Screen.height / 7f + 165f, 100f, 25f), "Server Port", guistyle);
            port = GUI.TextField(new Rect((Screen.width / 2), (Screen.height / 7 + 160), 200f, 45f), string.Empty + port, 25);
            /*if (GUI.Button(new Rect((Screen.width / 2 - 150), (Screen.height / 7 + 240), 300f, 50f), "Create"))
            {
                try
                {
                    ListenPort = int.Parse(port);
                    currentMenu = MenuGUIStates.creating;
                }
                catch (Exception ex)
                {
                    port = string.Empty + 25001;
                }
            }*/
        }
        else if (currentMenu == MenuGUIStates.creating)
        {
            string text7 = "Creatin'...";
            guistyle.normal.textColor = Color.Lerp(Color.black, Color.grey, 0.1f);
            GUI.Label(new Rect((Screen.width / 2 + 2 - 50), (Screen.height / 3 - 3), 100f, 25f), text7, guistyle);
            guistyle.normal.textColor = Color.white;
            GUI.Label(new Rect((Screen.width / 2 - 50), (Screen.height / 3), 100f, 25f), text7, guistyle);
            string text8 = string.Empty;
            if (allowAdminAccess)
            {
                text8 += "1";
            }
            else
            {
                text8 += "0";
            }
            if (ServerPassword.Length > 0)
            {
                text8 += ServerPassword;
            }
            print(text8);
            /*bool flag2 = !Network.HavePublicAddress();
            Network.InitializeServer(10, ListenPort, flag2);
            MasterServer.RegisterHost(versionName, username + "'s Server", text8);*/
            currentMenu = MenuGUIStates.pauseMenu;
        }
        else if (currentMenu == MenuGUIStates.pauseMenu)
        {
            if (GUI.Button(new Rect((Screen.width / 2 - 100), 225f, 200f, 60f), "Resume"))
            {
                Cursor.visible = true;
                GetComponent<MouseLook>().enabled = true;
                enabled = false;
            }
            if (GUI.Button(new Rect((Screen.width / 2 - 100), 295f, 200f, 60f), "Options"))
            {
                currentMenu = MenuGUIStates.options;
            }
            /*if (GUI.Button(new Rect((Screen.width / 2 - 100), 435f, 200f, 60f), "Quit to Main Menu"))
            {
                Network.Disconnect();
                MasterServer.UnregisterHost();
                host = MasterServer.PollHostList();
                RefreshPings();
                Screen.lockCursor = false;
                Application.LoadLevel(0);
            }*/
        }
        else if (currentMenu == MenuGUIStates.options)
        {
            if (mouselook == null)
            {
                mouselook = GetComponent<MouseLook>();
            }
            if (GUI.Button(new Rect((Screen.width / 2 - 100), 225f, 200f, 60f), "Back"))
            {
                currentMenu = MenuGUIStates.pauseMenu;
            }
            if (/*Network.isServer && */GUI.Button(new Rect((Screen.width / 2 - 100), 300f, 200f, 60f), "Kick / Ban Users"))
            {
                currentMenu = MenuGUIStates.ban;
            }
            mouselook.inversion = GUI.Toggle(new Rect((Screen.width / 2 - 100), 400f, 200f, 30f), mouselook.inversion, "Look Inversion");
            GUI.skin.label.fontSize = 14;
            GUI.Label(new Rect((Screen.width / 2 - 100), 370f, 100f, 30f), "Sensitivity: " + mouselook.sensitivityX);
            mouselook.sensitivityX = ((int)GUI.HorizontalSlider(new Rect((Screen.width / 2 - 5), 380f, 100f, 20f), mouselook.sensitivityX, 1f, 20f));
            mouselook.sensitivityY = mouselook.sensitivityX;
            PlayerPrefs.SetFloat("sensitivity", mouselook.sensitivityX);
        }
        else if (currentMenu == MenuGUIStates.ban)
        {
            /*baseBanListIndex = GUI.VerticalScrollbar(new Rect((Screen.width - 150), 60f, 20f, Screen.height / 1.6f), baseBanListIndex, 1f, 0f, Network.connections.Length);
            for (int j = 0; j < Network.connections.Length; j++)
            {
                NetworkPlayer networkPlayer = Network.connections[j];
                if (j < Players.Count && Players[j] != null)
                {
                    GUI.skin.label.fontSize = 16;
                    GUI.skin.label.alignment = 1;
                    GUI.Label(new Rect((Screen.width / 2 - 400), 120f + 30f * (-baseBanListIndex + j), 250f, 40f), Players[j] + " - " + networkPlayer.ipAddress);
                    if (GUI.Button(new Rect((Screen.width / 2 - 125), 120f + 30f * (-baseBanListIndex + j), 250f, 28f), "Kick " + Players[j]))
                    {
                        BannedPlayers.Add(PlayerIPs[j]);
                        Players.RemoveAt(j);
                        PlayerIPs.RemoveAt(j);
                        Network.CloseConnection(networkPlayer, true);
                        j = Network.connections.Length;
                    }
                }
            }*/
            if (GUI.Button(new Rect((Screen.width / 2 - 100), (Screen.height - 100), 200f, 60f), "Back"))
            {
                currentMenu = MenuGUIStates.options;
            }
        }
    }

    private string versionName = "Kritz-CBD-00-05-06";

    public int ConnectPort = 25000;

    public int ListenPort = 25001;

    public string port = string.Empty;

    /*private string IPAddress = "104.131.73.47";

    private int MasterPort = 23466;

    private int FacilitatorPort = 50005;*/

    public Vector3 ballPos;

    public Vector3 ballInitialSpawn = new Vector3(-31f, 12f, -54f);

    public bool ballSpawned;

    //public HostData[] host;

    public Ping[] pingTimes;

    //private bool connecting;

    public GameObject target;

    private MouseLook mouselook;

    public string username = string.Empty;

    //private string password = string.Empty;

    private string ServerPassword = string.Empty;

    private string loginMessage = string.Empty;

    public bool gotUsernameFromWebsite;

    //private bool userAuthenticated;

    private float loginStartTime;

    /*private float loginTimeoutDuration = 30f;

    private string joiningServerName;*/

    private bool allowAdminAccess = true;

    /*private float baseServerIndex;

    private float baseBanListIndex;

    private bool passcode;

    private bool joiningPasswordProtected;

    private int passcodeProgress;

    private int passcodeMaxProgress = 6;

    private string loginURL = "http://kritz.net/unitylogin.php";

    private bool loggedIn;

    private bool canLogIn;*/

    public List<string> Players = new List<string>();

    public List<string> PlayerIPs = new List<string>();

    public List<string> BannedPlayers = new List<string>();

    public MenuGUIStates currentMenu;

    /*private float timerStartLooking;

    private float timerLookingDuration = 10f;*/

    public GUISkin guiSkin;

    public Font fontEras;

    public enum MenuGUIStates
    {
        username,
        searching,
        joining,
        create,
        creating,
        serverlist,
        pauseMenu,
        options,
        ban
    }
}
