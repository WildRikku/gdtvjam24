using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public delegate void WeaponButtonClicked(int index);

public class WeaponChooseBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [Header("Do this for every Btn")]
    public int btnIndex = 0;
    public string btnName = "button";
    [TextArea(3, 5)]
    public string btnDescription = "";
    public Sprite btnSprite;

    [Header("Prefab")]
    public Image btnWeaponImage;
    public Image btnImage;
    public CanvasGroup textCG;
    public TMP_Text btnHeaderText;
    public TMP_Text btnDescriptionText;
    public Color activColor;
    public event WeaponButtonClicked WeaponButtonClicked;
    private WeaponChoosePanel panelParent;
    public GameController gameController;

    private void Start() {
        textCG.alpha = 0;
        btnWeaponImage.sprite = btnSprite;
        btnHeaderText.text = btnName;
        btnDescriptionText.text = btnDescription;
        panelParent = GetComponentInParent<WeaponChoosePanel>();
    }

    private void OnEnable() {
        ResetBtnColor();
        Invoke(nameof(TriggerActivBtn), 0.1f);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        textCG.alpha = 1;
    }

    public void OnPointerExit(PointerEventData eventData) {
        textCG.alpha = 0;
    }

    public void ChooseBtn() {
        AudioManager.Instance.PlaySFX("InstallWeapon");
        if (panelParent != null) {
            panelParent.RestBtnColor();
        }

        WeaponButtonClicked?.Invoke(btnIndex);
        SetActivBtn();
    }

    public void SetActivBtn() {
        btnImage.color = activColor;
    }

    public void TriggerActivBtn() {
        if (gameController != null) {
            int weaponIndex = gameController.teams[gameController.activeTeam].GetActivePlayer().weapon.index;
            if (weaponIndex == btnIndex)
                SetActivBtn();
        }
    }

    public void ResetBtnColor() {
        btnImage.color = Color.white;
    }
}