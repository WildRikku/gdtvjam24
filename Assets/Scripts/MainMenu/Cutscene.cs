using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Cutscene : MonoBehaviour {
    public PlayableDirector director;
    public CanvasGroup transitionImage;
    private bool _sceneFlag = false;


    private void Update() {
        if (director.state != PlayState.Playing && _sceneFlag == false) {
            GotoMenu();
            _sceneFlag = true;
        }

        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0)) && _sceneFlag == false) {
            GotoMenu();
            _sceneFlag = true;
        }
    }

    public void GotoMenu() {
        AudioManager.Instance.PlaySFX("CameraSwipe");
        transitionImage.DOFade(1f, 0.5f).OnComplete(()=> {
            
            SceneManager.LoadScene("MenuScene");
            });

        
    }
}
