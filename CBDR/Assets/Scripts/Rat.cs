using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    private GraphScript graph;

    public List<NavigationScript> pathToFollow = new List<NavigationScript>();

    private static List<Transform> avoidNodes = new List<Transform>();

    private float maxSpeed = 500f;

    public GameObject exitNode;

    private Vector3 targetLocation = Vector3.zero;

    public PickupObject stolenFood;

    private Transform targetFood;

    private int pathfindLayerMask = -32513;

    private PickupObject myPickupObject;

    private bool wasBeingHeld;

    private float timeStuck;

    private float stuckDuration = 2f;

    private float stuckCheckTime;

    private float stuckCheckWait = 2f;

    private bool defeated;

    private GameObject foodInSight;

    public string testString;

    void Start()
    {
        SetSpeeds(Random.Range(300f, maxSpeed), Random.Range(300f, maxSpeed));
        graph = GameObject.Find("!RatGraph").GetComponent<GraphScript>();
        myPickupObject = GetComponent<PickupObject>();
        FindExit();
    }

    void Update()
    {
        if (!GetComponent<Rigidbody>())
        {
            enabled = false;
        }
        if (myPickupObject.beingHeld)
        {
            wasBeingHeld = true;
            targetLocation = Vector3.zero;
        }
        else if (wasBeingHeld)
        {
            wasBeingHeld = false;
            GiveUp(gameObject);
            CreatePath(exitNode.transform.position);
        }
        foodInSight = CanSeeFood();
        if ((transform.position - targetLocation).magnitude < 3f && !foodInSight)
        {
            targetLocation = Vector3.zero;
        }
        if (pathToFollow == null || pathToFollow.Count == 0)
        {
            if (targetLocation != Vector3.zero)
            {
                CreatePath(targetLocation);
            }
            else
            {
                CreatePath(exitNode.transform.position);
            }
        }
        if (pathToFollow.Count == 0) {
            CreatePath(targetLocation);
        }
        if ((exitNode.transform.position - transform.position).magnitude < 15f)
        {
            if (stolenFood != null)
            {
                stolenFood.DestroyObject();
                SetStolenFood(gameObject, stolenFood.gameObject, false);
            }
            FloorTrigger.currentRats--;
            Destroy(gameObject);
        }
        if (stolenFood == null && targetFood == null && foodInSight && !defeated)
        {
            targetFood = foodInSight.transform;
        }
        if (targetFood && stolenFood == null && !defeated)
        {
            Vector3 zero = Vector3.zero;
            Vector3 a = Vector3.zero;
            zero += Seek(targetFood.transform.position);
            zero += Avoid();
            a = zero;
            a.y = 0f;
            if (zero != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(a + transform.forward);
            }
            GetComponent<Rigidbody>().AddForce(zero.normalized * maxSpeed * 10f * Time.deltaTime);
            if (defeated || !targetFood || !((transform.position - targetFood.position).magnitude < 4f))
            {
                return;
            }
            Food component = targetFood.GetComponent<Food>();
            if (component.beingHeldByRat && Random.value > 0.5f)
            {
                GiveUp(component.beingHeldByRat.gameObject);
            }
            if (targetFood && component)
            {
                component.beingHeldByRat = this;
                SetStolenFood(gameObject, targetFood.gameObject, true);
                if (FloorTrigger.foodDropPosition.Contains(targetFood.gameObject))
                {
                    FloorTrigger.foodDropPosition.Remove(targetFood.gameObject);
                }
                targetLocation = Vector3.zero;
                CreatePath(exitNode.transform.position);
                targetFood = null;
            }
        }
        else if (pathToFollow != null && pathToFollow.Count > 0)
        {
            Vector3 position = transform.position;
            float x = position.x;
            Vector3 position2 = transform.position;
            Vector2 a2 = new Vector2(x, position2.z);
            Vector3 position3 = pathToFollow[0].transform.position;
            float x2 = position3.x;
            Vector3 position4 = pathToFollow[0].transform.position;
            Vector2 b = new Vector2(x2, position4.z);
            float magnitude = (a2 - b).magnitude;
            if (magnitude < 4f)
            {
                pathToFollow.RemoveAt(0);
            }
            Vector3 zero2 = Vector3.zero;
            Vector3 a3 = Vector3.zero;
            if (pathToFollow.Count > 0)
            {
                zero2 += Seek(pathToFollow[0].transform.position);
                zero2 += Avoid();
                a3 = zero2;
                a3.y = 0f;
            }
            else
            {
                zero2 += Seek(targetLocation);
                zero2 += Avoid();
                a3 = zero2;
                a3.y = 0f;
            }
            if (zero2 != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(a3 + transform.forward);
            }
            GetComponent<Rigidbody>().AddForce(zero2.normalized * maxSpeed * 10f * Time.deltaTime);
        }
        if (stolenFood && stolenFood.transform.parent != transform) {
            SetStolenFood(gameObject, stolenFood.gameObject, false);
            Debug.Log("Test!");
        }
    }

    void FixedUpdate()
    {
        if (/*(bool)*/stolenFood)
        {
            //GiveUp(gameObject);
            stolenFood.transform.position = transform.position + transform.forward;
        }
    }

    private void SetStolenFood(GameObject ratID, GameObject foodID, bool ratIsHolding)
    {
        Rat component = ratID.GetComponent<Rat>();
        PickupObject component2 = foodID.GetComponent<PickupObject>();
        if (ratIsHolding)
        {
            component.stolenFood = component2;
            //component.stolenFood.GetComponent<Rigidbody>().useGravity = false;
            component.stolenFood.GetComponent<Rigidbody>().isKinematic = true;
            component.stolenFood.transform.parent = transform;
            component.stolenFood.GetComponent<Collider>().enabled = false;
            component.stolenFood.GetComponent<Food>().beingHeldByRat = component;
        }
        else
        {
            //component.stolenFood.GetComponent<Rigidbody>().useGravity = true;
            component.stolenFood.GetComponent<Rigidbody>().isKinematic = false;
            component.stolenFood.transform.parent = null;
            component.stolenFood.GetComponent<Collider>().enabled = true;
            component.stolenFood.GetComponent<Food>().beingHeldByRat = null;
            component.stolenFood = null;
        }
    }

    public void GiveUp(GameObject ratToGiveUp)
    {
        Rat component = ratToGiveUp.GetComponent<Rat>();
        if (/*(bool)*/component.stolenFood)
        {
            print("Stolen food!");
            SetStolenFood(ratToGiveUp, component.stolenFood.gameObject, false);
        }
        component.targetFood = null;
        component.targetLocation = Vector3.zero;
        component.defeated = true;
    }

    private void SetSpeeds(float baseSpeed, float maxSpeed)
    {
        this.maxSpeed = maxSpeed;
    }

    private void LateUpdate()
    {
        if (stuckCheckTime == 0f || timeStuck > 0f)
        {
            if (timeStuck == 0f && !Physics.Raycast(transform.position + transform.up * 0.5f, transform.forward, 1f, pathfindLayerMask))
            {
                timeStuck = Time.time;
                stuckCheckTime = timeStuck;
            }
            if (timeStuck > 0f && Physics.Raycast(transform.position + transform.up * 0.5f, transform.forward, 1f, pathfindLayerMask))
            {
                timeStuck = 0f;
                stuckCheckTime = Time.time;
            }
        }
        if (stuckCheckTime > 0f && Time.time > stuckCheckTime + stuckCheckWait)
        {
            stuckCheckTime = 0f;
        }
        if (timeStuck > 0f && Time.time > timeStuck + stuckDuration)
        {
            if (targetLocation != Vector3.zero)
            {
                CreatePath(targetLocation);
            }
            else
            {
                CreatePath(exitNode.transform.position);
            }
            timeStuck = 0f;
            stuckCheckTime = Time.time;
        }
    }

    void SyncFoodInSight(GameObject foodID)
    {
        GameObject gameObject = foodInSight = foodID;
    }

    GameObject CanSeeFood()
    {
        Collider[] array = Physics.OverlapSphere(transform.position, 10f);
        Collider[] array2 = array;
        foreach (Collider collider in array2)
        {
            if (!collider.GetComponent<Food>() || collider.name.Contains("rat"))
            {
                continue;
            }
            colliderTest = collider;
            Food component = collider.GetComponent<Food>();
            RaycastHit hitInfo;
            if (component.foodBeenOnFloor && !component.inFood && component.GetBurgerStack() == null && component.type != Food.FoodType.bun && /*Physics.Linecast(transform.position - (Vector3.down * 0.3f), collider.transform.position, out hitInfo, pathfindLayerMask)*/Physics.SphereCast(transform.position + transform.forward, 1f, transform.forward, out hitInfo) && hitInfo.collider == collider)
            {
                Vector3 position = transform.position;
                float y = position.y;
                Vector3 position2 = collider.transform.position;
                if (Mathf.Abs(y - position2.y) < 10f)
                {
                    return collider.gameObject;
                }
                testFloat = Mathf.Abs(y - position2.y);
            }
        }
        return null;
    }

    Collider colliderTest;
    float testFloat;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 10f);
        Gizmos.DrawWireSphere(transform.position, testFloat);
        Gizmos.DrawWireSphere(transform.position + transform.forward, 1);
        if (colliderTest) {
            Gizmos.DrawLine(transform.position - (Vector3.down * 0.3f), colliderTest.transform.position);
        }
    }

    Vector3 Avoid()
    {
        Vector3 vector = Vector3.zero;
        if (avoidNodes == null || avoidNodes.Count == 0)
        {
            foreach (Transform item in GameObject.Find("!AvoidNodes").transform)
            {
                avoidNodes.Add(item);
            }
        }
        foreach (Transform avoidNode in avoidNodes)
        {
            if ((transform.position - avoidNode.position).magnitude < 3f)
            {
                vector += (transform.position - avoidNode.position).normalized * maxSpeed * 0.5f * ((3f - (transform.position - avoidNode.position).magnitude) / 3f);
            }
        }
        return vector;
    }

    private Vector3 Seek(Vector3 Target)
    {
        Vector3 result = Vector3.zero;
        if (/*(bool)*/GetComponent<Rigidbody>())
        {
            result = (Target - transform.position).normalized * maxSpeed - GetComponent<Rigidbody>().velocity;
        }
        return result;
    }

    public void SetTargetFood(Vector3 targetPos)
    {
        targetLocation = targetPos;
    }

    public void CreatePath(Vector3 targetPos)
    {
        int num = FindClosestVisibleNode(transform);
        int goal = FindClosestNode(targetPos);
        if (num == -1)
        {
            print("Can't find closest");
            transform.LookAt(graph.nodes[FindClosestNode(transform.position)].transform.position);
            num = FindClosestVisibleNode(transform);
        }
        pathToFollow = graph.FindPath(num, goal);
        testString = num + " - " + goal;
    }

    private int FindClosestNode(Vector3 pos)
    {
        float num = 9999f;
        int num2 = -1;
        foreach (NavigationScript node in graph.nodes)
        {
            if ((node.transform.position - pos).magnitude < num)
            {
                num = (node.transform.position - pos).magnitude;
                num2 = graph.nodes[graph.nodes.IndexOf(node)].index;
            }
        }
        if (num2 == -1)
        {
            print("Error: no node found");
        }
        return num2;
    }

    public void FindExit(bool excludeNearestExit = false)
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
        exitNode = list[Random.Range(0, list.Count)];
    }

    public int FindClosestVisibleNode(Transform t)
    {
        float num = 9999f;
        int num2 = -1;
        foreach (NavigationScript node in graph.nodes)
        {
            if (!Physics.Linecast(t.position, node.transform.position, pathfindLayerMask) && (node.transform.position - t.position).magnitude < num && (node.transform.position - transform.position).magnitude > 0.5f)
            {
                num = (node.transform.position - t.position).magnitude;
                num2 = graph.nodes[graph.nodes.IndexOf(node)].index;
            }
        }
        if (num2 == -1)
        {
            print("Error: no visible node found");
        }
        return num2;
    }
}
