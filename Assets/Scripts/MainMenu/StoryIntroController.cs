using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;

public class StoryIntroController : MonoBehaviour {
    public PlayableDirector timeline_01;
    public PlayableDirector timeline_02;
    public PlayableDirector timeline_03;
    public CanvasGroup transitionCG;

    public int timelineIndex = 0;
    private bool isDirectorPlay = true;

    private void Update() {
        if (timeline_01.state != PlayState.Playing && timelineIndex == 0) {
            timelineIndex = 1;
            isDirectorPlay = false;
        }

        if (timeline_02.state != PlayState.Playing && timelineIndex == 2) {
            timelineIndex = 3;
            isDirectorPlay = false;
        }

        if (timeline_03.state != PlayState.Playing && timelineIndex == 4) {
            timelineIndex = 5;
            isDirectorPlay = false;
        }


        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && isDirectorPlay == false) {
            if (timelineIndex == 1) { timeline_02.Play(); timelineIndex = 2; AudioManager.Instance.PlaySFX("MouseClick"); }
            if (timelineIndex == 3) { timeline_03.Play(); timelineIndex = 4; AudioManager.Instance.PlaySFX("MouseClick"); }
            if (timelineIndex == 5) { GotoRoom("Level_01"); }
                isDirectorPlay = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            GotoRoom("MenuScene");
        }
    }

    public void GotoRoom(string room) {
        AudioManager.Instance.PlaySFX("CameraSwipe");
        transitionCG.DOFade(1f, 0.5f).OnComplete(() => {
            SceneManager.LoadScene(room);
        });
    }
}
