using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class MenuPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private bool isMouseOver = false;
    public MenuController menuController;
    public Sprite mouseOverSprite;
    private Sprite panelSprite;
    public Image panelBKImage;
    private Vector3 panelSize;
    public TMP_Text text;

    private void Start()
    {
        panelSprite = panelBKImage.sprite;
        panelSize = gameObject.transform.localScale;
        text.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isMouseOver == false)
        {
            panelBKImage.sprite = mouseOverSprite;
            gameObject.transform.DOScale(panelSize * 1.02f, 0.2f);
            text.color = Color.green;
        }
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isMouseOver == true)
        {
            panelBKImage.sprite = panelSprite;
            gameObject.transform.DOScale(panelSize, 0.2f);
            text.color = Color.white;
        }
        isMouseOver = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Panel clicked");
    }
}
