using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnlineGameManager : MonoBehaviourPunCallbacks
{
    // UI
    public Button LeaveButton;




    public float WaitingTime;
    public bool GameEnded;
    public GameObject ResultPanel;
    public GameObject[] Results;

    // *** DICE
    public Sprite[] DiceSprites, DiceAnimationSprites;
    public int NumberGot;
    public bool IsRolling, IsPlayerMoving;
    public string Rolled;
    public List<OnlineDice> Dices = new List<OnlineDice>();


    // **** player
    public List<string> Winners = new List<string>();
    int NumberOfPlayers;
    public string MyColor;

    public GameObject BluePlayerPrefab,
                        RedPlayerPrefab,
                        GreenPlayerPrefab,
                        YellowPlayerPrefab;
    public int LastIndex;
    public string TurnOf;


    // **** Path Point

    public OnlinePathPoint[] RedPathPoints,
                        GreenPathPoints,
                        YellowPathPoints,
                        BluePathPoints;

    [SerializeField]
    private List<string> colors = new List<string>();
    // public string[] colors = new string[2];//= { "green", "blue" };//, "yellow", "red"

    private Coroutine DiceRoller, PlayerMover;

    PhotonView PV;
    public static OnlineGameManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        GameEnded = false;
        NumberOfPlayers = 2; // todo implement 4 players

        WaitingTime = 1;
        PV = gameObject.GetComponent<PhotonView>();

        LastIndex = 0;

        colors.Add("green");
        colors.Add("blue");
        // colors[0] = "green"; colors[1] = "blue";

        if (PhotonNetwork.IsMasterClient)
        {
            MyColor = colors[0];
            TurnOf = MyColor;
            SetTurnOf(MyColor);
        }
        else
        {
            MyColor = colors[1];
        }

        LeaveButton.onClick.AddListener(LeaveRoom);

        PhotonNetwork.NickName = MyColor;
    }

    void RemoveFromBoard(string color)
    {
        Destroy(GameObject.Find(color));

    }


    // ******* player switcher
    private void SetTurnOf(string _color)
    {
        Debug.Log("Setting turn: " + _color);
        PV.RPC("RPC_SetTurnOf", RpcTarget.All, _color);
    }

    [PunRPC]
    public void RPC_SetTurnOf(string _color)
    {
        TurnOf = _color;

        if (GameEnded)
        {
            StopDiceRoller_Coroutine();
            StopPlayerMoving_Coroutine();
            Debug.Log("Game ended");
            GiveResult();
            return;
        }

        Debug.Log("showing current player: " + MyColor);
        foreach (OnlineDice dice in Dices)
        {
            if (TurnOf == dice.color)
            {
                dice.CanRoll = true;
            }
            else
            {
                dice.CanRoll = false;
            }

            DiceRoller = StartCoroutine(DiceShouldBeRolled());

        }
    }




    public void SwitchToNextPlayer(bool _switch = true)
    {
        if (_switch)
        {
            LastIndex++;
            if (LastIndex >= NumberOfPlayers) LastIndex = 0;
        }
        SetTurnOf(colors[LastIndex]);
    }


    // player
    IEnumerator PlayerShoundMove(OnlinePlayer player)
    {
        yield return new WaitForSeconds(WaitingTime);

        // auto move if possible
        Debug.Log("OnlinePlayer auto moving");
        player.OnMouseDown();

    }

    public void OnPlayerMove(bool ReachedHomeOrGotSix = false)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (ReachedHomeOrGotSix || NumberGot == 6)
            {
                SwitchToNextPlayer(false);
            }
            else
            {
                SwitchToNextPlayer();
            }
        }
    }

    public void StopPlayerMoving_Coroutine()
    {
        if (PlayerMover != null) StopCoroutine(PlayerMover);
    }


    // dice
    public IEnumerator DiceShouldBeRolled()
    {
        yield return new WaitForSeconds(WaitingTime);

        foreach (OnlineDice dice in Dices)
        {
            if (dice && dice.color == TurnOf)
            {
                Debug.Log("dice auto rolled");

                dice.CanRoll = true;
                dice.OnMouseDown();
                break;
            }
        }
    }

    public void StopDiceRoller_Coroutine()
    {
        if (DiceRoller != null)
            StopCoroutine(DiceRoller);
    }

    // all all instance
    public void OnDiceRoll(List<OnlinePlayer> _players)
    {
        // check if has a moveable player
        bool _playerIsMoveable = false;
        OnlinePlayer player = null;
        for (int i = 0; i < _players.Count; i++)
        {
            player = _players[i];

            if (player.CanMove)
            {
                if ((player.AlreadyMoved + NumberGot) <= 57)
                {
                    _playerIsMoveable = true;
                    break;
                }
            }
            else
            {
                if (!player.ReachedHome && NumberGot == 6)
                {
                    _playerIsMoveable = true;
                    break;
                }
            }
        }

        // move player if possible
        if (_playerIsMoveable)
        {
            Debug.Log("Has moveable player: " + player.color);
            PlayerMover = StartCoroutine(PlayerShoundMove(player));
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            SwitchToNextPlayer();
        }
    }



    // result
    public void OnWin(string color)
    {
        Debug.Log("Win: " + color);
        for (int i = 0; i < colors.Count - 1; i++)
        {
            if (colors[i] == color)
            {
                string tmp = colors[i];
                colors[i] = colors[i + 1];
                colors[i + 1] = tmp;
            }
        }
        Winners.Add(color);

        if (Winners.Count == NumberOfPlayers - 1)
        {
            // game ended
            GameEnded = true;
            Winners.Add(colors[0]);
        }
    }

    void GiveResult()
    {
        Debug.Log("game ended");
        StopAllCoroutines();

        ResultPanel.SetActive(true);
        for (int i = 0; i < Winners.Count; i++)
        {
            Results[i].SetActive(true);
            Results[i].GetComponent<Place>().Name.text = Winners[i];
        }
    }










    // rooms ui
    void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LudoEntryScene");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (!otherPlayer.IsInactive)
        {
            if (otherPlayer.NickName == colors[0])
            {
                OnWin(colors[1]);
            }
            else
            {
                OnWin(colors[0]);
            }

            SetTurnOf(TurnOf);
            Debug.Log(otherPlayer.NickName + " left");
        }
    }
}
