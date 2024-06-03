using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Playables;

public class StoryEndingController : MonoBehaviour {
    public PlayableDirector timeline_01;


    public int timelineIndex = 0;
    private bool isDirectorPlay = true;

    private void Update() {
        if (timeline_01.state != PlayState.Playing && timelineIndex == 0) {
            timelineIndex = 1;
            isDirectorPlay = false;
        }

  
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && isDirectorPlay == false) {
            if (timelineIndex == 1) { GotoRoom("MenuScene"); }
                isDirectorPlay = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            GotoRoom("MenuScene");
        }
    }

    public void GotoRoom(string room) {
        AudioManager.Instance.PlaySFX("CameraSwipe");
        //transitionImage.DOFade(1f, 0.5f).OnComplete(() => {

        SceneManager.LoadScene(room);



    }
}
