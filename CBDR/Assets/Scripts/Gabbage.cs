using UnityEngine;
public class Gabbage : MonoBehaviour {
    public GameObject[] slides;
    public AudioClip audioCut;
    float timer = 1;
    void Update() {
        timer = Mathf.Max(timer - Time.deltaTime, 0);
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name.Contains("BoxCut") && timer == 0) {
            if (gameObject.name.Contains("Slide")) {
                for (int i = 0; i < 3; i++) {
                    GameObject lettuce = Instantiate(Resources.Load("Prefabs/Food/Lettuce") as GameObject, transform.position, transform.rotation, GameObject.Find("Rigid-Elements").transform);
                    lettuce.GetComponent<Food>().foodTemp = gameObject.GetComponent<Food>().foodTemp / 3;
                    lettuce.GetComponent<Food>().cooked = gameObject.GetComponent<Food>().cooked;
                    lettuce.GetComponent<Flamable>().currentTemp = gameObject.GetComponent<Flamable>().currentTemp / 3;
                    lettuce.GetComponent<MeshRenderer>().material.color = gameObject.GetComponent<MeshRenderer>().material.color;
                    lettuce.name = "Lettuce";
                }
                Destroy(gameObject);
            } else {
                for (int i = 0; i < 2; i++) {
                    GameObject slideGabbage = Instantiate(Resources.Load("Prefabs/Food/SlideGabbage") as GameObject, transform.position, transform.rotation, GameObject.Find("Rigid-Elements").transform);
                    if (i == 1) {
                        slideGabbage.transform.Rotate(new Vector3(0, 180, 0));
                    }
                    slideGabbage.GetComponent<Food>().foodTemp = gameObject.GetComponent<Food>().foodTemp / 2;
                    slideGabbage.GetComponent<Food>().cooked = gameObject.GetComponent<Food>().cooked;
                    slideGabbage.GetComponent<Flamable>().currentTemp = gameObject.GetComponent<Flamable>().currentTemp / 2;
                    slideGabbage.GetComponent<MeshRenderer>().material.color = gameObject.GetComponent<MeshRenderer>().material.color;
                    slideGabbage.name = "SlideGabbage";
                }
                Destroy(gameObject);
            }
            AudioSource.PlayClipAtPoint(audioCut, transform.position);
        }
    }
}
