using UnityEngine;

public class PanelAnimator : MonoBehaviour
{
    public static PanelAnimator instance;
    public float AnimationTime;

    private void Awake()
    {
        AnimationTime = 0.2f;

        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    // {
    //     get
    //     {
    //         if (instance == null)
    //         {
    //             instance = new PanelAnimator();
    //         }
    //         return instance;
    //     }
    //     private set { }
    // }

    // public void OpenPanel(GameObject panel)
    // {
    //     panel.SetActive(true);
    //     panel.transform.localScale = Vector3.zero;
    //     LeanTween.scale(panel, new Vector3(1.2f, 1.2f, 1), AnimationTime).setOnComplete(BacktoNormal);
    // }

    // void BacktoNormal()
    // {
    //     LeanTween.scale(panel, Vector3.one, AnimationTime);
    // }

}