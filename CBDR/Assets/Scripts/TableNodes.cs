using UnityEngine;

public class TableNodes : MonoBehaviour
{
    private void Awake()
    {
        if (table == null)
        {
            float num = 99999f;
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Table"))
            {
                if ((transform.position - gameObject.transform.position).magnitude < num)
                {
                    num = (transform.position - gameObject.transform.position).magnitude;
                    GameObject gameObject2 = gameObject;
                    table = gameObject2.GetComponent<Table>();
                }
            }
            table.myTableNode = this;
        }
    }

    public void SetOccupied(bool occ)
    {
        occupied = occ;
        occupiedStartTime = Time.time;
    }

    public Table table;

    public bool occupied;

    public float occupiedStartTime;
}
