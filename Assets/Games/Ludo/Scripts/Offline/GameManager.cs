using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public float WaitingTime;
    public bool GameEnded;
    public GameObject ResultPanel;
    public GameObject[] Results;

    // *** DICE
    public Sprite[] DiceSprites, DiceAnimationSprites;
    public int NumberGot;
    public bool IsRolling, IsPlayerMoving;
    public string Rolled;
    public List<Dice> Dices = new List<Dice>();


    // **** player
    public List<string> Winners = new List<string>();
    int NumberOfPlayers;

    public GameObject BluePlayerPrefab,
                        RedPlayerPrefab,
                        GreenPlayerPrefab,
                        YellowPlayerPrefab;
    public int LastIndex;
    public string TurnOf;


    // **** Path Point

    public PathPoint[] RedPathPoints,
                        GreenPathPoints,
                        YellowPathPoints,
                        BluePathPoints;

    public string[] colors = { "green", "blue", "yellow", "red" };

    private Coroutine DiceRoller, PlayerMover;


    public static GameManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        Dice[] _Dices = (Dice[])GameObject.FindObjectsOfType(typeof(Dice));
        GameEnded = false;
        NumberOfPlayers = LudoController.instance
        ? LudoController.instance.NumberOfPlayers : 4;

        WaitingTime = 1f;

        if (NumberOfPlayers == 2)
        {
            RemoveFromBoard(colors[2]);
            RemoveFromBoard(colors[3]);

            colors = new string[2];
            colors[0] = "green";
            colors[1] = "blue";
        }
        else if (NumberOfPlayers == 3)
        {
            RemoveFromBoard(colors[3]);
            colors = new string[3];
            colors[0] = "green";
            colors[1] = "blue";
            colors[2] = "yellow";
        }

        foreach (Dice d in _Dices)
        {
            if (d.isActiveAndEnabled)
            {
                Dices.Add(d);
            }
        }

        LastIndex = 0;
        SetTurnOf(colors[0]);

    }

    void RemoveFromBoard(string color)
    {
        Destroy(GameObject.Find(color));

    }

    public void SetTurnOf(string _color)
    {

        if (GameEnded)
        {
            GiveResult();
            return;
        }

        TurnOf = _color;

        Debug.Log("Turn set to: " + _color);

        DiceRoller = StartCoroutine(DiceShouldBeRolled());
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
    IEnumerator PlayerShoundMove(Player player)
    {
        yield return new WaitForSeconds(WaitingTime);

        // auto move if possible
        Debug.Log("Player auto moving");
        player.OnMouseDown();

    }

    public void OnPlayerMove(bool ReachedHomeOrGotSix = false)
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

    public void StopPlayerMoving_Coroutine()
    {
        if (PlayerMover != null) StopCoroutine(PlayerMover);
    }


    // dice
    public IEnumerator DiceShouldBeRolled()
    {
        yield return new WaitForSeconds(WaitingTime);

        foreach (Dice dice in Dices)
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

    public void OnDiceRoll(List<Player> _players)
    {
        // check if has a moveable player
        bool _playerIsMoveable = false;
        Player player = null;
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
            PlayerMover = StartCoroutine(PlayerShoundMove(player));
        }
        else
        {
            SwitchToNextPlayer();
        }
    }



    // result
    public void OnWin(string color)
    {
        Debug.Log("Win: " + color);
        for (int i = 0; i < colors.Length - 1; i++)
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
}
