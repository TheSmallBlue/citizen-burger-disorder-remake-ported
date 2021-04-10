using System;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum wants
    {
        toExitScene,
        toEnter,
        toFindSeat,
        toGetToSeat,
        toGetMenu,
        toLeave,
        toEat,
        toIdle
    }

    public Transform playerToFollow;

    public Transform nodeToFollow;

    public Transform targetToFollow;

    private GraphScript graph;

    public List<NavigationScript> pathToFollow = new List<NavigationScript>();

    private List<Transform> avoidNodes = new List<Transform>();

    public bool isFollowingPlayer;

    public bool isFollowingTableNode;

    private float speed = 60f;

    private float maxSpeed = 120f;

    private float unsafeDistance = 7f;

    private Vector3 randomDistanceFromNodeBias;

    private float followWeight = 1f;

    private float avoidNodesWeight = 0.6f;

    //private float avoidNPCsWeight = 0.75f;

    public bool inside;

    public GameObject exitNode;

    public List<GameObject> exitListz = new List<GameObject>();

    public TableGraph tableGraph;

    public List<NPC> myNPCGroup = new List<NPC>();

    public NPC myNPCGroupLeader;

    public TableNodes targetSeat;

    public wants currentlyWants;

    public wants previouslyWanted;

    public float idleTimeMin = 5f;

    public float idleTimeRand;

    public float idleStartTime;

    public float waitUntilNextIdle = 5f;

    private float linecastCheckDelay = 5f;

    private float lastLinecastCheck;

    private int timesStuckPathfinding;

    public int desiredGroupSize = 2;

    private float menuWaitBaseDuration = 120f;

    private float menuWaitStartTime = 0;

    private float foodWaitBaseDuration = 400f;

    public float foodWaitStartTime;

    private int layerMask = -28929;

    private int pathfindLayerMask = -53505;

    /*private void OnPlayerConnected(NetworkPlayer player)
    {
        SyncPosition(transform.position, transform.rotation);
        if (GetComponent<Rigidbody>())
        {
            SyncRigidbody(
                GetComponent<Rigidbody>().velocity,
                GetComponent<Rigidbody>().angularVelocity
            );
        }
    }*/

    private void SyncPosition(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }

    private void SyncRigidbody(Vector3 vel, Vector3 aVel)
    {
        if (!GetComponent<Rigidbody>())
        {
            gameObject.AddComponent<Rigidbody>();
        }
        GetComponent<Rigidbody>().velocity = vel;
        GetComponent<Rigidbody>().angularVelocity = aVel;
    }

    private void SyncWants(int currentlyWantsID, int previouslyWantsID)
    {
        currentlyWants = (NPC.wants)currentlyWantsID;
    }

    /*private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        if (stream.isWriting)
        {
            Vector3 position = base.transform.position;
            Quaternion rotation = base.transform.rotation;
            stream.Serialize(ref position);
            stream.Serialize(ref rotation);
        }
        else
        {
            Vector3 zero = Vector3.zero;
            Quaternion rotation2 = Quaternion.Euler(0f, 0f, 0f);
            stream.Serialize(ref zero);
            stream.Serialize(ref rotation2);
            base.transform.position = Vector3.Lerp(base.transform.position, zero, 0.2f);
            base.transform.rotation = rotation2;
        }
    }*/

    private void Start()
    {
        graph = GameObject.Find("!NavigationGraph").GetComponent<GraphScript>();
        tableGraph = GameObject.Find("!TableNodes").GetComponent<TableGraph>();
        foreach (Transform item in GameObject.Find("!AvoidNodes").transform)
        {
            avoidNodes.Add(item);
        }
        randomDistanceFromNodeBias = UnityEngine.Random.insideUnitSphere * 5f;
        randomDistanceFromNodeBias.y = 0f;
        speed *= UnityEngine.Random.Range(0.9f, 1.5f);
        FindExit(true);
    }

    private void Update()
    {
        if (currentlyWants == wants.toEnter)
        {
            if (pathToFollow == null || pathToFollow.Count == 0)
            {
                CreatePath(graph.enterance.transform.position);
            }
            else
            {
                if ((pathToFollow[0].transform.position + randomDistanceFromNodeBias - transform.position).magnitude < 10f)
                {
                    if (Enterance.npcsWaiting < 6)
                    {
                        pathToFollow.RemoveAt(0);
                    }
                    else
                    {
                        setWants(wants.toExitScene);
                        pathToFollow = new List<NavigationScript>();
                        FindExit(false);
                        CreatePath(exitNode.transform.position);
                    }
                    if (UnityEngine.Random.value > 0.5f)
                    {
                        randomDistanceFromNodeBias = UnityEngine.Random.insideUnitSphere * 5f;
                        randomDistanceFromNodeBias.y = 0f;
                    }
                }
                RaycastHit raycastHit;
                if (pathToFollow.Count == 0)
                {
                    Enterance.npcsWaiting++;
                    setWants(wants.toFindSeat);
                }
                else if (Time.time > lastLinecastCheck + linecastCheckDelay && Physics.Linecast(transform.position, pathToFollow[0].transform.position, out raycastHit, pathfindLayerMask) && raycastHit.distance < 3f)
                {
                    if (timesStuckPathfinding > 4)
                    {
                        gameObject.layer = 16;
                    }
                    randomDistanceFromNodeBias = UnityEngine.Random.insideUnitSphere * 0.2f;
                    randomDistanceFromNodeBias.y = 0f;
                    CreatePath(graph.enterance.transform.position);
                    timesStuckPathfinding++;
                    lastLinecastCheck = Time.time;
                }
                else
                {
                    Vector3 vector = Vector3.zero;
                    Vector3 a = Vector3.zero;
                    vector += Seek(pathToFollow[0].transform.position + randomDistanceFromNodeBias) * followWeight;
                    a = vector;
                    a.y = 0f;
                    vector += SeparateAvoidNodes() * avoidNodesWeight;
                    vector.y = 0f;
                    if (vector != Vector3.zero)
                    {
                        transform.rotation = Quaternion.LookRotation(a + transform.forward);
                    }
                    GetComponent<Rigidbody>().AddForce(vector.normalized * maxSpeed * 10f * Time.deltaTime);
                }
            }
        }
        else if (currentlyWants == NPC.wants.toExitScene)
        {
            if (pathToFollow == null || pathToFollow.Count == 0)
            {
                CreatePath(exitNode.transform.position);
            }
            else
            {
                if ((pathToFollow[0].transform.position + randomDistanceFromNodeBias - transform.position).magnitude < 10f)
                {
                    pathToFollow.RemoveAt(0);
                    if (UnityEngine.Random.value > 0.5f)
                    {
                        randomDistanceFromNodeBias = UnityEngine.Random.insideUnitSphere * 5f;
                        randomDistanceFromNodeBias.y = 0f;
                    }
                }
                RaycastHit raycastHit2;
                if (pathToFollow.Count == 0)
                {
                    SpawnNPC.currentNPCs--;
                    /*Network.RemoveRPCs(base.networkView.viewID);
                    Network.Destroy(base.networkView.viewID);*/
                    Destroy(gameObject);
                }
                else if (Time.time > lastLinecastCheck + linecastCheckDelay && Physics.Linecast(transform.position, pathToFollow[0].transform.position, out raycastHit2, layerMask) && raycastHit2.distance < 3f)
                {
                    if (timesStuckPathfinding > 4)
                    {
                        gameObject.layer = 15;
                    }
                    randomDistanceFromNodeBias = UnityEngine.Random.insideUnitSphere * 0.2f;
                    randomDistanceFromNodeBias.y = 0f;
                    CreatePath(exitNode.transform.position);
                    timesStuckPathfinding++;
                    lastLinecastCheck = Time.time;
                }
                else
                {
                    Vector3 vector2 = Vector3.zero;
                    Vector3 a2 = Vector3.zero;
                    vector2 += Seek(pathToFollow[0].transform.position + randomDistanceFromNodeBias) * followWeight;
                    a2 = vector2;
                    a2.y = 0f;
                    vector2 += SeparateAvoidNodes() * avoidNodesWeight;
                    vector2.y = 0f;
                    if (vector2 != Vector3.zero)
                    {
                        transform.rotation = Quaternion.LookRotation(a2 + transform.forward);
                    }
                    GetComponent<Rigidbody>().AddForce(vector2.normalized * maxSpeed * 10f * Time.deltaTime);
                }
            }
        }
        else if (currentlyWants == wants.toIdle)
        {
            transform.rotation = Quaternion.LookRotation(transform.forward);
            Vector3 zero = Vector3.zero;
            GetComponent<Rigidbody>().AddForce(zero.normalized * maxSpeed * 10f * Time.deltaTime);
            if (Time.time > idleTimeMin + idleTimeRand + idleStartTime)
            {
                currentlyWants = previouslyWanted;
                if (currentlyWants == wants.toExitScene)
                {
                    FindExit(false);
                    CreatePath(exitNode.transform.position);
                }
            }
        }
        else if (currentlyWants == wants.toFindSeat)
        {
            List<NPC> list = new List<NPC>();
            int num = 0;
            int num2 = 0;
            list.Add(this);
            if (this.desiredGroupSize > 1)
            {
                GameObject[] array = GameObject.FindGameObjectsWithTag("NPC");
                for (int i = 0; i < array.Length; i++)
                {
                    GameObject gameObject = array[i];
                    if (gameObject != base.gameObject && (transform.position - gameObject.transform.position).magnitude < 40f)
                    {
                        NPC component = gameObject.GetComponent<NPC>();
                        if (num2 < desiredGroupSize - 1 && (component.currentlyWants == wants.toFindSeat || component.currentlyWants == wants.toIdle) && component.desiredGroupSize == desiredGroupSize)
                        {
                            list.Add(component);
                            num2++;
                        }
                    }
                }
            }
            if (list.Count > 1 && list.Count == desiredGroupSize)
            {
                myNPCGroup = list;
                if (tableGraph == null)
                {
                    tableGraph = GameObject.Find("!TableNodes").GetComponent<TableGraph>();
                }
                num = TableGraph.FindUnoccupiedTableForGroup(myNPCGroup.Count);
                print("Goal table: " + num);
                if (num >= 0)
                {
                    foreach (NPC current in list)
                    {
                        if (current != this)
                        {
                            current.myNPCGroupLeader = this;
                            current.setWants(wants.toGetToSeat);
                        }
                        foreach (TableNodes current2 in TableGraph.tables[num - 1].tableNodes)
                        {
                            if (!current2.occupied)
                            {
                                current.targetSeat = current2;
                                current2.SetOccupied(true);
                                current.setWants(wants.toGetToSeat);
                                current.pathToFollow = graph.FindPath(FindClosestNode(current.transform.position), FindClosestVisibleNode(current.targetSeat.transform));
                                print(string.Concat(new object[]
                                {
                                    "Found a seat for id[",
                                    list.IndexOf(current),
                                    "] @ ",
                                    num,
                                    " : ",
                                    TableGraph.tables[num - 1].tableNodes.IndexOf(current2)
                                }));
                                Enterance.npcsWaiting--;
                                break;
                            }
                        }
                        if (targetSeat == null)
                        {
                            print("Error: no free seat found?");
                        }
                    }
                }
                else
                {
                    if (/*Network.isServer && */desiredGroupSize == 4 && UnityEngine.Random.value > 0.8f)
                    {
                        setGroupSize(gameObject, 2);
                    }
                    myNPCGroup = new List<NPC>();
                    myNPCGroupLeader = null;
                    setWants(wants.toIdle);
                    idleStartTime = Time.time;
                    idleTimeRand = UnityEngine.Random.Range(20f, 40f);
                }
            }
            else if (desiredGroupSize == 1)
            {
                if (tableGraph == null)
                {
                    tableGraph = GameObject.Find("!TableNodes").GetComponent<TableGraph>();
                }
                num = TableGraph.FindUnoccupiedTableForGroup(myNPCGroup.Count);
                if (num >= 0)
                {
                    foreach (TableNodes current3 in TableGraph.tables[num - 1].tableNodes)
                    {
                        if (!current3.occupied)
                        {
                            targetSeat = current3;
                            current3.SetOccupied(true);
                            setWants(wants.toGetToSeat);
                            pathToFollow = graph.FindPath(FindClosestNode(transform.position), FindClosestVisibleNode(targetSeat.transform));
                            Enterance.npcsWaiting--;
                            break;
                        }
                    }
                }
                setWants(wants.toIdle);
                idleStartTime = Time.time;
                idleTimeRand = UnityEngine.Random.Range(20f, 40f);
            }
            else
            {
                myNPCGroup = new List<NPC>();
                myNPCGroupLeader = null;
                setWants(wants.toIdle);
                idleStartTime = Time.time;
                idleTimeRand = UnityEngine.Random.Range(10f, 30f);
            }
        }
        else if (currentlyWants == wants.toGetToSeat)
        {
            Vector3 vector3 = Vector3.zero;
            Vector3 a3 = Vector3.zero;
            if (pathToFollow.Count > 0 && (pathToFollow[0].transform.position - transform.position).magnitude < 3f)
            {
                pathToFollow.RemoveAt(0);
            }
            RaycastHit raycastHit3;
            if (Physics.Linecast(transform.position, targetSeat.transform.position, out raycastHit3, layerMask))
            {
                if (pathToFollow.Count > 0)
                {
                    vector3 += Seek(pathToFollow[0].transform.position) * followWeight;
                    a3 = vector3;
                    a3.y = 0f;
                    vector3 += SeparateAvoidNodes() * avoidNodesWeight;
                    vector3.y = 0f;
                    if (vector3 != Vector3.zero)
                    {
                        transform.rotation = Quaternion.LookRotation(a3 + transform.forward);
                    }
                    GetComponent<Rigidbody>().AddForce(vector3.normalized * maxSpeed * 8f * Time.deltaTime);
                }
                else
                {
                    float num3 = 1f;
                    if ((transform.position - targetSeat.transform.position).magnitude < 6f)
                    {
                        num3 = 0.2f;
                    }
                    if ((transform.position - targetSeat.transform.position).magnitude < 1f)
                    {
                        targetSeat.table.SetNPCAtTable(targetSeat.table.gameObject, gameObject);
                        GetComponent<Rigidbody>().velocity = Vector3.zero;
                        setWants(wants.toEat);
                        /*if (Network.peerType == NetworkPeerType.Server)
                        {
                            targetSeat.table.GenerateFoodOrder();
                        }*/
                        targetSeat.table.GenerateFoodOrder();
                        foodWaitStartTime = Time.time;
                    }
                    vector3 += Seek(targetSeat.transform.position) * followWeight;
                    a3 = vector3;
                    a3.y = 0f;
                    vector3 += SeparateAvoidNodes() * (avoidNodesWeight * num3);
                    vector3.y = 0f;
                    if (vector3 != Vector3.zero)
                    {
                        transform.rotation = Quaternion.LookRotation(a3 + transform.forward);
                    }
                    GetComponent<Rigidbody>().AddForce(vector3.normalized * maxSpeed * 10f * Time.deltaTime);
                }
            }
            else
            {
                if ((transform.position - targetSeat.transform.position).magnitude < 1f)
                {
                    targetSeat.table.SetNPCAtTable(targetSeat.table.gameObject, gameObject);
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    setWants(wants.toEat);
                    /*if (Network.peerType == NetworkPeerType.Server)
                    {
                        targetSeat.table.GenerateFoodOrder();
                    }*/
                    targetSeat.table.GenerateFoodOrder();
                    foodWaitStartTime = Time.time;
                }
                vector3 += Seek(targetSeat.transform.position) * followWeight * 1.5f;
                a3 = vector3;
                a3.y = 0f;
                vector3 += SeparateAvoidNodes() * avoidNodesWeight;
                vector3.y = 0f;
                if (vector3 != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(a3 + transform.forward);
                }
                GetComponent<Rigidbody>().AddForce(vector3.normalized * maxSpeed * 10f * Time.deltaTime);
            }
        }
        else if (currentlyWants == wants.toLeave)
        {
            if (myNPCGroupLeader == null)
            {
                int num4 = 0;
                foreach (NPC current4 in myNPCGroup)
                {
                    if (current4.currentlyWants == wants.toLeave)
                    {
                        num4++;
                    }
                }
                if (num4 == myNPCGroup.Count)
                {
                    print("Entire group wants to leave");
                    if (desiredGroupSize > 1)
                    {
                        foreach (NPC current5 in myNPCGroup)
                        {
                            current5.targetSeat.table.npcAtTable = null;
                            current5.targetSeat.table.ClearOrder();
                            current5.targetSeat.SetOccupied(false);
                            current5.FindExit(false);
                            current5.CreatePath(current5.exitNode.transform.position);
                            current5.setWants(wants.toExitScene);
                        }
                    }
                    else
                    {
                        targetSeat.table.npcAtTable = null;
                        targetSeat.table.ClearOrder();
                        targetSeat.SetOccupied(false);
                        FindExit(false);
                        CreatePath(exitNode.transform.position);
                        setWants(wants.toExitScene);
                    }
                }
            }
        }
        else if (currentlyWants == wants.toGetMenu)
        {
            if (Time.time > menuWaitStartTime + menuWaitBaseDuration)
            {
                currentlyWants = wants.toLeave;
            }
        }
        else if (currentlyWants == wants.toEat && Time.time > foodWaitStartTime + foodWaitBaseDuration)
        {
            currentlyWants = wants.toLeave;
        }
    }

    public void FindExit(bool excludeNearestExit)
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag("Despawner");
        List<GameObject> list = new List<GameObject>();
        float num = 99999f;
        int index = 0;
        for (int i = 0; i < array.Length; i++)
        {
            list.Add(array[i]);
            if ((transform.position - array[i].transform.position).magnitude < num)
            {
                num = (transform.position - array[i].transform.position).magnitude;
                index = i;
            }
        }
        if (excludeNearestExit && list.Count > 1)
        {
            list.RemoveAt(index);
        }
        exitNode = list[UnityEngine.Random.Range(0, list.Count)];
        exitListz = list;
    }

    private Vector3 Seek(Vector3 Target)
    {
        Vector3 zero = Vector3.zero;
        return (Target - transform.position).normalized * maxSpeed - GetComponent<Rigidbody>().velocity;
    }

    private Vector3 Arrive()
    {
        Vector3 vector = Vector3.zero;
        vector = playerToFollow.position + playerToFollow.forward * 5f - transform.position;
        float magnitude = vector.magnitude;
        float num = magnitude * 0.1f;
        return vector.normalized * (maxSpeed * num) * speed;
    }

    private Vector3 SeparateFlock()
    {
        Vector3 vector = Vector3.zero;
        GameObject[] array = GameObject.FindGameObjectsWithTag("NPC");
        for (int i = 0; i < array.Length; i++)
        {
            GameObject gameObject = array[i];
            if ((transform.position - gameObject.transform.position).magnitude < unsafeDistance)
            {
                vector -= (gameObject.transform.position - transform.position).normalized * maxSpeed;
            }
        }
        return vector;
    }

    private Vector3 SeparateAvoidNodes()
    {
        Vector3 vector = Vector3.zero;
        foreach (Transform current in avoidNodes)
        {
            if ((transform.position - current.transform.position).magnitude < unsafeDistance)
            {
                vector -= (current.transform.position - transform.position).normalized * maxSpeed;
            }
        }
        return vector;
    }

    private void CreatePath(Vector3 target)
    {
        int num = FindClosestVisibleNode(transform);
        int goal = FindClosestNode(target, num);
        if (num == -1)
        {
            print("Error: no path found");
            num = 0;
        }
        pathToFollow = graph.FindPath(num, goal);
    }

    private int FindClosestNode(Vector3 t)
    {
        float num = 9999f;
        int num2 = -1;
        foreach (NavigationScript current in graph.nodes)
        {
            if ((current.transform.position - t).magnitude < num)
            {
                num = (current.transform.position - t).magnitude;
                num2 = graph.nodes.IndexOf(current);
            }
        }
        if (num2 == -1)
        {
            print("Error: no node found");
        }
        return num2;
    }

    private int FindClosestNode(Vector3 t, int s)
    {
        float num = 9999f;
        int num2 = -1;
        foreach (NavigationScript current in graph.nodes)
        {
            if ((current.transform.position - t).magnitude < num && current.index != s)
            {
                num = (current.transform.position - t).magnitude;
                num2 = graph.nodes.IndexOf(current);
            }
        }
        if (num2 == -1)
        {
            print("Error: no node found");
        }
        return num2;
    }

    private int FindClosestVisibleNode(Transform t)
    {
        float num = 9999f;
        int num2 = -1;
        foreach (NavigationScript current in graph.nodes)
        {
            if (!Physics.Linecast(t.position, current.transform.position, layerMask) && (current.transform.position - t.position).magnitude < num)
            {
                if ((t.transform.position - new Vector3(24.6f, 4.5f, 31.7f)).magnitude < 2f)
                {
                    print("Dist: " + (current.transform.position - t.position).magnitude);
                }
                if ((current.transform.position - transform.position).magnitude > 0.5f)
                {
                    num = (current.transform.position - t.position).magnitude;
                    num2 = graph.nodes.IndexOf(current);
                }
            }
        }
        if (num2 == -1)
        {
            print("Error: no visible node found");
        }
        return num2;
    }

    public void setWants(int newWantID)
    {
        previouslyWanted = currentlyWants;
        currentlyWants = (wants)newWantID;
    }

    public void setWants(wants newWant)
    {
        previouslyWanted = currentlyWants;
        currentlyWants = newWant;
    }

    public void setGroupSize(GameObject npcID, int newDesiredGroupSize)
    {
        NPC component = npcID.GetComponent<NPC>();
        component.desiredGroupSize = newDesiredGroupSize;
    }

    public void FollowAPlayer(GameObject playerID)
    {
        if (playerID != null)
        {
            playerToFollow = gameObject.transform;
            isFollowingPlayer = true;
        }
        else
        {
            playerToFollow = null;
            isFollowingPlayer = false;
        }
    }

    public void FollowNoPlayer()
    {
        playerToFollow = null;
        isFollowingPlayer = false;
    }

    public void SetNPCTexture(GameObject npcID, string textureName)
    {
        npcID.GetComponent<MeshRenderer>().material = (Resources.Load("Skins/Materials/" + textureName) as Material);
    }
}
