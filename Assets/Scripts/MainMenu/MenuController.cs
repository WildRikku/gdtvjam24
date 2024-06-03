using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
    public CanvasGroup mainMenuCG;
    public CanvasGroup creditsCG;
    public CanvasGroup openLevelCG;
    public CanvasGroup levelTransitonCG;

    public Slider musicSlider, sfxSlider;

    private float musicVolume;
    private float sfxVolume;

    public string strLevel1;
    public string strLevel2;
    public string strLevel3;
    public string storyIntro;

    private void Awake() {
        mainMenuCG.alpha = 1;
        mainMenuCG.blocksRaycasts = true;
        creditsCG.alpha = 0;
        creditsCG.blocksRaycasts = false;
        openLevelCG.alpha = 0;
        openLevelCG.blocksRaycasts = false;
        levelTransitonCG.alpha = 0;
        levelTransitonCG.blocksRaycasts = false;
    }

    private void Start() {
        if (musicSlider != null) {
            musicVolume = AudioManager.Instance.musicVolume;
            sfxVolume = AudioManager.Instance.sfxVolume;

            AudioManager.Instance.SetMusicVolume(musicVolume);
            AudioManager.Instance.SetSFXVolume(sfxVolume);

            musicSlider.value = musicVolume;
            sfxSlider.value = sfxVolume;
        }

        AudioManager.Instance.PlayMusic("MainMenuSound");
    }

    public void MusicVolume() {
        AudioManager.Instance.SetMusicVolume(musicSlider.value);
        AudioManager.Instance.musicVolume = musicSlider.value;
    }

    public void SFXVolume() {
        AudioManager.Instance.SetSFXVolume(sfxSlider.value);
        AudioManager.Instance.sfxVolume = sfxSlider.value;
    }

    public void ClickOnSlider() {
        AudioManager.Instance.PlaySFX("MouseSpecialHover");
    }

    public void EndTrackSlider() {
        AudioManager.Instance.PlaySFX("MouseClick");
    }

    public void OpenMenu() {
        AudioManager.Instance.PlaySFX("MouseGoBack2");

        mainMenuCG.DOFade(1, 0.5f);
        mainMenuCG.blocksRaycasts = true;
        creditsCG.DOFade(0, 0.5f);
        creditsCG.blocksRaycasts = false;
        openLevelCG.DOFade(0, 0.5f);
        openLevelCG.blocksRaycasts = false;
    }

    public void OpenCredits() {
        AudioManager.Instance.PlaySFX("MouseClick");

        mainMenuCG.DOFade(0, 0.5f);
        mainMenuCG.blocksRaycasts = false;
        creditsCG.DOFade(1, 0.5f);
        creditsCG.blocksRaycasts = true;
        openLevelCG.DOFade(0, 0.5f);
        openLevelCG.blocksRaycasts = false;
    }

    public void OpenLevel() {
        AudioManager.Instance.PlaySFX("MouseGoBack3");
        AudioManager.Instance.SetPvpmode();

        mainMenuCG.DOFade(0, 0.5f);
        mainMenuCG.blocksRaycasts = false;
        creditsCG.DOFade(0, 0.5f);
        creditsCG.blocksRaycasts = false;
        openLevelCG.DOFade(1, 0.5f);
        openLevelCG.blocksRaycasts = true;
    }

    public void StartStoryMode()  {
        mainMenuCG.DOFade(0, 0.1f);
        mainMenuCG.blocksRaycasts = false;
        creditsCG.DOFade(0, 0.1f);
        creditsCG.blocksRaycasts = false;
        openLevelCG.DOFade(0, 0.1f);
        openLevelCG.blocksRaycasts = false;
        levelTransitonCG.blocksRaycasts = true;

        AudioManager.Instance.PlaySFX("MouseGoBack1");
        AudioManager.Instance.SetStorymode();
        levelTransitonCG.DOFade(1, 0.25f).OnComplete(() => {
            SceneManager.LoadScene(storyIntro);
        });
    }

    public void GameQuit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void StartLevel(int level = 0) {
        mainMenuCG.DOFade(0, 0.1f);
        mainMenuCG.blocksRaycasts = false;
        creditsCG.DOFade(0, 0.1f);
        creditsCG.blocksRaycasts = false;
        openLevelCG.DOFade(0, 0.1f);
        openLevelCG.blocksRaycasts = false;
        levelTransitonCG.blocksRaycasts = true;

        AudioManager.Instance.PlaySFX("MouseGoBack1");
        levelTransitonCG.DOFade(1, 0.25f).OnComplete(() => {
            switch (level) {
                case 0:
                    SceneManager.LoadScene(strLevel1);
                    break;

                case 1:
                    SceneManager.LoadScene(strLevel2);
                    break;

                case 2:
                    SceneManager.LoadScene(strLevel3);
                    break;
            }
        });
    }
}