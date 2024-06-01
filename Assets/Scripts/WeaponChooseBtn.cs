using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponChooseBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [Header("Do this for every Btn")]
    public int btnIndex = 0;
    public string btnName = "button";
    [TextArea(3, 5)]
    public string btnDescription = "";
    public Sprite btnSprite;

    [Header("Prefab")]
    public Image btnImage;
    public CanvasGroup textCG;
    public TMP_Text btnHeaderText;
    public TMP_Text btnDescriptionText;

    private void Start() {
        textCG.alpha = 0;
        btnImage.sprite = btnSprite;
        btnHeaderText.text = btnName;
        btnDescriptionText.text = btnDescription;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        textCG.alpha = 1;
    }

    public void OnPointerExit(PointerEventData eventData) {
        textCG.alpha = 0;
    }

    public void ChooseBtn() {
        AudioManager.Instance.PlaySFX("InstallWeapon");

        Debug.Log($"Christian hier musst du was mit dem index machen - {btnIndex}");
    }
}