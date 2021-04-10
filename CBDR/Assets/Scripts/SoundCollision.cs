using UnityEngine;
public class SoundCollision : MonoBehaviour {
    public GameObject prefab;
    public AudioClip clip;
    void OnCollisionEnter(Collision collision) {
        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude >= 1) {
            GameObject instantiateObject = Instantiate(prefab, transform.position, Quaternion.identity);
            instantiateObject.GetComponent<AudioSource>().clip = clip;
            instantiateObject.GetComponent<AudioSource>().volume = Mathf.Clamp((gameObject.GetComponent<Rigidbody>().velocity.magnitude / 2) - 0.5f, 0, 1);
            instantiateObject.GetComponent<NewAudioSourceScript>().pitch = Random.Range(0.9f, 1.2f) * Time.timeScale;
            instantiateObject.GetComponent<AudioSource>().Play();
        }
    }
}
