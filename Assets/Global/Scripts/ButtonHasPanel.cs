using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHasPanel : MonoBehaviour
{
    float AnimationTime;

    private Button button;
    public Button close;
    public GameObject panel;

    private void Awake()
    {
        AnimationTime = 0.2f;
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OpenPanel);
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
        panel.transform.localScale = Vector3.zero;
        panel.LeanScale(Vector3.one, AnimationTime).setEaseInOutQuart();
    }

    void BacktoNormal()
    {
    }


}
