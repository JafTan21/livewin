using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClosePanel : MonoBehaviour
{
    public float CloseTime = 0.1f;
    public GameObject panel;

    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(Close);
    }

    private void Close()
    {
        if (panel != null) LeanTween.scale(panel, Vector3.zero, CloseTime).setOnComplete(onComplete);
    }

    void onComplete()
    {
        panel.SetActive(false);
    }
}
