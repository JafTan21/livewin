using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LudoController : MonoBehaviour
{
    public int NumberOfPlayers;
    public static LudoController instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
