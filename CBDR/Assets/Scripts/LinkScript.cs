using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class LinkScript : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler {
    public string url;
    public void OnPointerClick (PointerEventData eventDate) {
        Application.OpenURL(url);
    }
    public void OnPointerDown(PointerEventData eventData) {
        gameObject.GetComponent<Text>().color = new Color(0, 0.25f, 1);
    }
    public void OnPointerEnter(PointerEventData eventData) {
        gameObject.GetComponent<Text>().color = new Color(0, 0.75f, 1);
    }
    public void OnPointerExit(PointerEventData eventData) {
        gameObject.GetComponent<Text>().color = new Color(0, 0.5f, 1);
    }
    public void OnPointerUp(PointerEventData eventData) {
        gameObject.GetComponent<Text>().color = new Color(0, 0.5f, 1);
    }
}
