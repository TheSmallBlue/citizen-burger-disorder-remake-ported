using System.Collections.Generic;
using UnityEngine;

public class TableGraph : MonoBehaviour
{
    private void Start()
    {
        int num = 0;
        foreach (object obj in base.transform)
        {
            Transform transform = (Transform)obj;
            TableNodes component = transform.GetComponent<TableNodes>();
            nodes.Add(component);
            if (num < component.table.tableNumber)
            {
                num = component.table.tableNumber;
            }
        }
        tables = new TableGroup[num];
        for (int i = 0; i < tables.Length; i++)
        {
            tables[i] = new TableGroup();
            tables[i].capacity = 0;
            tables[i].tableMats = new List<Table>();
            tables[i].tableNodes = new List<TableNodes>();
        }
        foreach (TableNodes tableNodes in nodes)
        {
            int num2 = tableNodes.table.tableNumber - 1;
            tables[num2].tableNodes.Add(tableNodes);
            tables[num2].tableMats.Add(tableNodes.table);
            tables[num2].capacity++;
        }
    }

    private int GetCapacityOfTable(int tableNumber)
    {
        return tables[tableNumber - 1].capacity;
    }

    public static int FindUnoccupiedTableForGroup(int groupSize)
    {
        if (tables != null)
        {
            for (int i = 0; i < tables.Length; i++)
            {
                print(string.Concat(new object[]
                {
                    "Table ",
                    i + 1,
                    " has a capacity of: ",
                    tables[i].capacity
                }));
                if (tables[i].capacity >= groupSize)
                {
                    print("The group of " + groupSize + " can fit here");
                    bool flag = false;
                    foreach (TableNodes tableNodes in tables[i].tableNodes)
                    {
                        if (tableNodes.occupied)
                        {
                            print("... but the table is already occupied.");
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        return i + 1;
                    }
                }
            }
        }
        return -1;
    }

    public List<TableNodes> nodes = new List<TableNodes>();

    public static TableGroup[] tables;

    public class TableGroup
    {
        public int capacity;

        public List<Table> tableMats;

        public List<TableNodes> tableNodes;
    }
}
