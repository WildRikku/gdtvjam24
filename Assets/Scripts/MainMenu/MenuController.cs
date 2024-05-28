using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class MenuController : MonoBehaviour
{
    public CanvasGroup mainMenuCG;
    public CanvasGroup creditsCG;
    public CanvasGroup openLevelCG;

    public Slider musicSlider, sfxSlider;

    private float musicVolume;
    private float sfxVolume;

    private void Awake()
    {
        mainMenuCG.alpha = 1;
        mainMenuCG.blocksRaycasts = true;
        creditsCG.alpha = 0;
        creditsCG.blocksRaycasts = false;
        openLevelCG.alpha = 0;
        openLevelCG.blocksRaycasts = false;
    }

    private void Start()
    {
        if (musicSlider != null)
        {
            musicVolume = AudioManager.Instance.musicVolume;
            sfxVolume = AudioManager.Instance.sfxVolume;

            AudioManager.Instance.SetMusicVolume(musicVolume);
            AudioManager.Instance.SetSFXVolume(sfxVolume);

            musicSlider.value = musicVolume;
            sfxSlider.value = sfxVolume;
        }

        AudioManager.Instance.PlayMusic("MainMenuSound");
    }

    public void MusicVolume()
    {
        AudioManager.Instance.SetMusicVolume(musicSlider.value);
        AudioManager.Instance.musicVolume = musicSlider.value;
    }
    public void SFXVolume()
    {
        AudioManager.Instance.SetSFXVolume(sfxSlider.value);
        AudioManager.Instance.sfxVolume = sfxSlider.value;
    }

    public void OpenMenu()
    {
        mainMenuCG.DOFade(1, 0.5f);
        mainMenuCG.blocksRaycasts = true;
        creditsCG.DOFade(0, 0.5f);
        creditsCG.blocksRaycasts = false;
        openLevelCG.DOFade(0, 0.5f);
        openLevelCG.blocksRaycasts = false;


    }


    public void OpenCredits()
    {
        mainMenuCG.DOFade(0, 0.5f);
        mainMenuCG.blocksRaycasts = false;
        creditsCG.DOFade(1, 0.5f);
        creditsCG.blocksRaycasts = true;
        openLevelCG.DOFade(0, 0.5f);
        openLevelCG.blocksRaycasts = false;


    }

    public void OpenLevel()
    {
        mainMenuCG.DOFade(0, 0.5f);
        mainMenuCG.blocksRaycasts = false;
        creditsCG.DOFade(0, 0.5f);
        creditsCG.blocksRaycasts = false;
        openLevelCG.DOFade(1, 0.5f);
        openLevelCG.blocksRaycasts = true;


    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
