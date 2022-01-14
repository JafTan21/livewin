using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnlineDice : MonoBehaviour
{
    public bool CanRoll;
    public float RollTime;
    public string color;
    public SpriteRenderer DiceResultRenderer;
    public List<OnlinePlayer> Players = new List<OnlinePlayer>();

    PhotonView PV;

    private void Start()
    {
        PV = gameObject.GetComponent<PhotonView>();
        RollTime = 1f;
        GameObject[] _objects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in _objects)
        {
            OnlinePlayer p = obj.GetComponent<OnlinePlayer>();
            if (p.color == color)
            {
                Players.Add(p);
            }
        }

        CanRoll = true;
    }

    public void OnMouseDown()
    {
        if (OnlineGameManager.instance.IsRolling
        || OnlineGameManager.instance.IsPlayerMoving
        || OnlineGameManager.instance.TurnOf != color
        || !CanRoll
        || OnlineGameManager.instance.MyColor != OnlineGameManager.instance.TurnOf
        )
        {
            Debug.Log(CanRoll.ToString() + " -- clicked but not rolled: " + color);
            return;
        }

        int n = Random.Range(1, 7);
        PV.RPC("RPC_RollDiceTo", RpcTarget.All, n);

        // StartCoroutine(RollDiceTo(n));
    }

    [PunRPC]
    void RPC_RollDiceTo(int n)
    {
        StartCoroutine(RollDiceTo(n));
    }

    public IEnumerator RollDiceTo(int n)
    {
        CanRoll = false;
        OnlineGameManager.instance.StopDiceRoller_Coroutine(); // stop auto rolling
        OnlineGameManager.instance.IsRolling = true;

        // animation start
        float timeLeft = RollTime;
        while (timeLeft > 0)
        {
            timeLeft -= 0.05f;
            DiceResultRenderer.sprite = OnlineGameManager.instance.DiceAnimationSprites[Random.Range(0, 6)];
            yield return new WaitForSeconds(0.01f);
        }
        // animation end
        DiceResultRenderer.sprite = OnlineGameManager.instance.DiceSprites[n - 1];


        OnlineGameManager.instance.IsRolling = false;
        OnlineGameManager.instance.NumberGot = n;
        OnlineGameManager.instance.Rolled = color;

        // call on dice roll
        OnlineGameManager.instance.OnDiceRoll(Players);
    }
}
