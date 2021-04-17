using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class LinkScript : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler {
    public int index;
    string[] url = {
        "https://twitter.com/atAmpersatKritz",
        "https://www.youtube.com/c/NickKritzBlackburn",
        "https://www.youtube.com/channel/UC9Rap9CYqBLdNFgl94SkYqw",
        "https://www.youtube.com/channel/UCzMHTCV2VnRvhAC8vvC35Qg",
        "https://discord.gg/fTaznBKjjp"
    };
    public void OnPointerClick (PointerEventData eventDate) {
        Application.OpenURL(url[index]);
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
