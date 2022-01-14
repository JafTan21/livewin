using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public float FadeTime;
    public string TargetScene = null;

    private void Start()
    {
        FadeTime = .1f;
        gameObject.GetComponent<Button>().onClick.AddListener(GoToScene);
    }

    private void GoToScene()
    {
        if (TargetScene == null) return;


        CanvasGroup cg = GameObject.FindObjectOfType<CanvasGroup>();

        if (cg != null)
        {
            cg.alpha = 1;
            cg.LeanAlpha(0, FadeTime).setOnComplete(LoadTarget);
        }
        else
        {
            LoadTarget();
        }

    }

    void LoadTarget()
    {
        SceneManager.LoadScene(TargetScene);
    }
}
