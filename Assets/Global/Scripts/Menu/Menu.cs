using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Button DepositButton;
    public TextMeshProUGUI Balance;

    private void Awake()
    {
        DepositButton.onClick.AddListener(Deposit);
    }

    private void Update()
    {
        Balance.text = AuthManager.instance.Balance.ToString();
    }

    void Deposit()
    {
        SceneManager.LoadScene("DepositScene");
    }

}
