using UnityEngine;

public class Instantiator : MonoBehaviour
{

    public GameObject auth, popup;

    CanvasGroup cg;

    private void Awake()
    {
        cg = GameObject.FindObjectOfType<CanvasGroup>();

        Instantiate(auth, Vector3.zero, Quaternion.identity);
        Instantiate(popup, Vector3.zero, Quaternion.identity);
    }

    private void OnEnable()
    {
        if (cg != null)
        {
            cg.alpha = 0;
            cg.LeanAlpha(1, 0.2f);
        }
    }
}