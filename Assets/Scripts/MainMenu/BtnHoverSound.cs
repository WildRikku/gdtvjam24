using UnityEngine;
using UnityEngine.EventSystems;

public class BtnHoverSound : MonoBehaviour, IPointerEnterHandler
{

    private string[] hoverSounds = new string[3] { "MouseHover1", "MouseHover2", "MouseHover3" };


    public void OnPointerEnter(PointerEventData eventData)
    {
        int ran = Random.Range(0, 3);

        AudioManager.Instance.PlaySFX(hoverSounds[ran]);
    }
}
