using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BtnHoverSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private string[] hoverSounds = new string[3] { "MouseHover1", "MouseHover2", "MouseHover3" };
    public TMP_Text text;
    public Color color;


    public void OnPointerEnter(PointerEventData eventData) {
        int ran = Random.Range(0, 3);

        AudioManager.Instance.PlaySFX(hoverSounds[ran]);

        if (text != null) {
            text.color = color;
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (text != null) {
            text.color = Color.white;
        }

    }
}