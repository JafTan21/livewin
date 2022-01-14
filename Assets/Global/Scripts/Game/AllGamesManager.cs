using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AllGamesManager : MonoBehaviour
{
    float FadeTime;
    public CanvasGroup loading;
    public Button ludo, spinner;

    public void Awake()
    {
        FadeTime = 0.1f;

        ludo.onClick.AddListener(LoadLudoScene);
        spinner.onClick.AddListener(delegate { SceneManager.LoadScene("LudoEntryScene"); });
        // todo : add correct listener to spinner
    }

    public void LoadLudoScene()
    {
        loading.alpha = 1;
        loading.LeanAlpha(0, FadeTime).setOnComplete(LudoPage);
    }

    void LudoPage()
    {
        SceneManager.LoadScene("LudoEntryScene");

    }
}
