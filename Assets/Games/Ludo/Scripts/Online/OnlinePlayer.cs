using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnlinePlayer : MonoBehaviour
{
    public float MoveSpeed;
    public string color;
    public Vector3 DefaultPosition, DefaultScale;
    public bool CanMove;
    public int AlreadyMoved = 0;
    public bool ReachedHome = false;


    private OnlinePathPoint prev;



    [SerializeField]
    private OnlinePathPoint[] PathPoints;

    PhotonView PV;

    private void Start()
    {
        PV = gameObject.GetComponent<PhotonView>();

        MoveSpeed = 0.2f;
        DefaultPosition = gameObject.transform.position;
        DefaultScale = gameObject.transform.localScale;
        switch (color)
        {
            case "red":
                PathPoints = OnlineGameManager.instance.RedPathPoints;
                break;

            case "green":
                PathPoints = OnlineGameManager.instance.GreenPathPoints;
                break;

            case "blue":
                PathPoints = OnlineGameManager.instance.BluePathPoints;
                break;

            case "yellow":
                PathPoints = OnlineGameManager.instance.YellowPathPoints;
                break;
        }
    }


    public void OnMouseDown()
    {
        if (OnlineGameManager.instance.MyColor != OnlineGameManager.instance.Rolled)
        {
            return;
        }
        if (OnlineGameManager.instance.Rolled == color)
        {
            if (!CanMove && !ReachedHome)
            {
                if (OnlineGameManager.instance.NumberGot == 6)
                {
                    PV.RPC("RPC_PlayerJustActivated", RpcTarget.All);
                    return;
                }
            }


            if (!CanMove
             || ReachedHome
             || OnlineGameManager.instance.IsPlayerMoving
             || OnlineGameManager.instance.NumberGot + AlreadyMoved > 57
             )
            {
                return;
            }

            PV.RPC("RPC_MoveNSteps", RpcTarget.All, OnlineGameManager.instance.NumberGot);
        }
    }

    [PunRPC]
    private void RPC_PlayerJustActivated()
    {
        Debug.Log("player just activated");
        OnlineGameManager.instance.NumberGot = 0;
        CanMove = true;
        prev = PathPoints[0];
        prev.AddPlayer(this);
        AlreadyMoved = 1;
        OnlineGameManager.instance.OnPlayerMove(true);
    }

    [PunRPC]
    private void RPC_MoveNSteps(int n)
    {
        StartCoroutine(MoveNSteps(n));
    }

    private IEnumerator MoveNSteps(int n)
    {
        Debug.Log("Moving: " + n.ToString());
        OnlineGameManager.instance.IsPlayerMoving = true;

        for (int i = AlreadyMoved; i < AlreadyMoved + n; i++)
        {
            yield return new WaitForSeconds(MoveSpeed);
            PathPoints[i].AddPlayer(this);

            if (prev) prev.RemovePlayer(this);

            prev = PathPoints[i];
        }

        AlreadyMoved += n;

        bool killedOrReachedHome = CheckHomeOrKill();

        OnlineGameManager.instance.OnPlayerMove(killedOrReachedHome);
        OnlineGameManager.instance.IsPlayerMoving = false;

    }

    bool CheckHomeOrKill()
    {
        bool killedOrReachedHome = false;
        if (AlreadyMoved == PathPoints.Length)
        {
            killedOrReachedHome = ReachedHome = true;
            if (PathPoints[PathPoints.Length - 1].Players.Count == 4)
            {
                // player won
                OnlineGameManager.instance.OnWin(color);
            }
        }
        else if (PathPoints[AlreadyMoved - 1].Players.Count > 1)
        {
            for (int i = 0; i < PathPoints[AlreadyMoved - 1].Players.Count; i++)
            {
                OnlinePlayer p = PathPoints[AlreadyMoved - 1].Players[i];
                if (p.color != color && !PathPoints[AlreadyMoved - 1].IsStoppage)
                {
                    killedOrReachedHome = true;
                    p.Kill();
                    break;
                }
            }
        }
        return killedOrReachedHome;
    }

    void Kill()
    {
        PathPoints[AlreadyMoved - 1].RemovePlayer(this);
        CanMove = false;
        transform.position = DefaultPosition;
        transform.localScale = DefaultScale;
        AlreadyMoved = 0;
        Debug.Log("Killed: " + color);
    }
}
