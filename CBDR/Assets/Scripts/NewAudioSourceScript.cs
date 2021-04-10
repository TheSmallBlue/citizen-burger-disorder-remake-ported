using UnityEngine;
public class NewAudioSourceScript : MonoBehaviour {
    public float pitch = 1;
    bool stopBool;

    GameObject obj;
    Vector3 positionGrill;
    Vector3 sizePosition;
    bool isGrill;
    void Update() {
        gameObject.GetComponent<AudioSource>().pitch = pitch * Time.timeScale;
        if (isGrill) {
            transform.position = new Vector3(
                Mathf.Clamp(obj.transform.position.x, (-sizePosition.x / 2) + positionGrill.x, (sizePosition.x / 2) + positionGrill.x), 
                positionGrill.y, 
                Mathf.Clamp(obj.transform.position.z, (-sizePosition.z / 2) + positionGrill.z, (sizePosition.z / 2) + positionGrill.z));
        }
        if (stopBool) {
            gameObject.GetComponent<AudioSource>().volume -= Time.deltaTime;
            if (gameObject.GetComponent<AudioSource>().volume <= 0) {
                Destroy(gameObject);
            }
        } else {
            if (!gameObject.GetComponent<AudioSource>().isPlaying) {
                Destroy(gameObject);
            }
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.gameObject == obj && isGrill) {
            stopBool = true;
        }
    }
    public void OnGrill(GameObject obj, Vector3 positionGrill, Vector3 size) {
        this.obj = obj;
        this.positionGrill = positionGrill;
        sizePosition = size;
        isGrill = true;
    }
}
