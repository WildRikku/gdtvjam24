using TMPro;
using DG.Tweening;
using UnityEngine;

public class DamagePopUp : MonoBehaviour {
    public CanvasGroup cG;
    public TMP_Text damageTxt;

    private void OnEnable() {
        cG.alpha = 0;
        cG.DOFade(1, 0.2f);

        gameObject.transform.DOBlendableMoveBy(new(0, 1, 0), 2f);
        cG.DOFade(0, 2f).SetDelay(0.2f).OnComplete(() => { Destroy(gameObject); });
    }
}