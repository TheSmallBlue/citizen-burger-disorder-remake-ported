using UnityEngine;

public class Box : MonoBehaviour
{
    private void Awake()
    {
        boxOpenedPrefab = (Resources.Load("Prefabs/Misc/BoxOpened") as GameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (/*Network.isServer && */collision.gameObject.name.Equals("hand"))
        {
            OpenBox();
        }
    }

    private Vector3 randomSpawnPos()
    {
        return transform.position + Random.insideUnitSphere * 1f;
    }

    //[RPC]
    public void SyncContents(/*NetworkViewID boxID, */int contentsID)
    {
        Box component = /*NetworkView.Find(boxID).*/transform.GetComponent<Box>();
        switch (contentsID)
        {
            case 0:
                component.contents = BoxContents.PattMcRat;
                break;
            case 1:
                component.contents = BoxContents.SeedyCedric;
                break;
            case 2:
                component.contents = BoxContents.GreenGrace;
                break;
            default:
                component.contents = BoxContents.PattMcRat;
                break;
        }
        print(string.Concat(new object[]
        {
            "Setting box contents to ",
            contentsID,
            ", ",
            component.contents
        }));
    }

    private void OpenBox()
    {
        /*Network.*/Instantiate(boxOpenedPrefab, transform.position, transform.rotation/*, 1*/, GameObject.Find("Rigid-Elements").transform);
        switch (contents)
        {
            case BoxContents.PattMcRat:
                for (int i = 0; i < Random.Range(4, 7); i++)
                {
                    /*Network.*/Instantiate(pattyPre, randomSpawnPos(), transform.rotation/*, 1*/, GameObject.Find("Rigid-Elements").transform);
                }
                for (int j = 0; j < Random.Range(4, 7); j++)
                {
                    /*Network.*/Instantiate(baconPre, randomSpawnPos(), transform.rotation/*, 1*/, GameObject.Find("Rigid-Elements").transform);
                }
                break;
            case BoxContents.SeedyCedric:
                for (int k = 0; k < Random.Range(4, 7); k++)
                {
                    /*Network.*/Instantiate(bunTopPre, randomSpawnPos(), transform.rotation/*, 1*/, GameObject.Find("Rigid-Elements").transform);
                    /*Network.*/Instantiate(bunBotPre, randomSpawnPos(), transform.rotation/*, 1*/, GameObject.Find("Rigid-Elements").transform);
                }
                break;
            case BoxContents.GreenGrace:
                if (GameObject.Find("NewRestaurant")) {
                    for (int l = 0; l < Random.Range(1, 3); l++)
                    {
                        Instantiate(gabbagePre, randomSpawnPos(), transform.rotation, GameObject.Find("Rigid-Elements").transform);
                    }
                } else {
                    for (int l = 0; l < Random.Range(2, 5); l++)
                    {
                        /*Network.*/Instantiate(lettucePre, randomSpawnPos(), transform.rotation/*, 1*/, GameObject.Find("Rigid-Elements").transform);
                    }
                }
                for (int m = 0; m < Random.Range(4, 7); m++)
                {
                    /*Network.*/Instantiate(cheesePre, randomSpawnPos(), transform.rotation/*, 1*/, GameObject.Find("Rigid-Elements").transform);
                }
                for (int n = 0; n < Random.Range(2, 5); n++)
                {
                    /*Network.*/Instantiate(TomatoPre, randomSpawnPos(), transform.rotation/*, 1*/, GameObject.Find("Rigid-Elements").transform);
                }
                break;
        }
        GetComponent<PickupObject>().DestroyObject();
    }

    private GameObject boxOpenedPrefab;

    public BoxContents contents;

    public GameObject pattyPre;

    public GameObject baconPre;

    public GameObject bunBotPre;

    public GameObject bunTopPre;

    public GameObject cheesePre;

    public GameObject lettucePre;

    public GameObject TomatoPre;

    public GameObject gabbagePre;

    public enum BoxContents
    {
        PattMcRat,
        SeedyCedric,
        GreenGrace
    }
}
