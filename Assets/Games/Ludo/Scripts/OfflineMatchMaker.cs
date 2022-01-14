using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class OfflineMatchMaker : MonoBehaviour
{
    public int NumberOfPlayers;
    public Button p2, p3, p4;
    public Button _continue;


    private void Awake()
    {


        p2.onClick.AddListener(delegate { SelectPlayer(2); });
        // p3.onClick.AddListener(delegate { SelectPlayer(3); });
        p4.onClick.AddListener(delegate { SelectPlayer(4); });

        SelectPlayer(4);

        _continue.onClick.AddListener(GoToGameScene);
    }

    void SelectPlayer(int n)
    {
        NumberOfPlayers = n;

        switch (n)
        {
            case 2:
                p3.GetComponent<CanvasGroup>().LeanAlpha(0.5f, 0.05f);
                p4.GetComponent<CanvasGroup>().LeanAlpha(0.5f, 0.05f);

                p2.GetComponent<CanvasGroup>().LeanAlpha(1, 0.05f);
                break;

            case 3:
                p2.GetComponent<CanvasGroup>().LeanAlpha(0.5f, 0.05f);
                p4.GetComponent<CanvasGroup>().LeanAlpha(0.5f, 0.05f);

                p3.GetComponent<CanvasGroup>().LeanAlpha(1, 0.05f);
                break;

            case 4:
                p2.GetComponent<CanvasGroup>().LeanAlpha(0.5f, 0.05f);
                p3.GetComponent<CanvasGroup>().LeanAlpha(0.5f, 0.05f);

                p4.GetComponent<CanvasGroup>().LeanAlpha(1, 0.05f);
                break;
        }

        LudoController.instance.NumberOfPlayers = NumberOfPlayers;
    }

    void GoToGameScene()
    {
        SceneManager.LoadScene("BoardScene");
    }
}
