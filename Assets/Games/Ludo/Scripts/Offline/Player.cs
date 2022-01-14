using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float MoveSpeed;
    public string color;
    public Vector3 DefaultPosition, DefaultScale;
    public bool CanMove;
    public int AlreadyMoved = 0;
    public bool ReachedHome = false;


    private PathPoint prev;



    [SerializeField]
    private PathPoint[] PathPoints;

    private void Start()
    {
        MoveSpeed = 0;
        DefaultPosition = gameObject.transform.position;
        DefaultScale = gameObject.transform.localScale;
        switch (color)
        {
            case "red":
                PathPoints = GameManager.instance.RedPathPoints;
                break;

            case "green":
                PathPoints = GameManager.instance.GreenPathPoints;
                break;

            case "blue":
                PathPoints = GameManager.instance.BluePathPoints;
                break;

            case "yellow":
                PathPoints = GameManager.instance.YellowPathPoints;
                break;
        }
    }


    public void OnMouseDown()
    {
        Debug.Log("on mouse down");
        if (GameManager.instance.Rolled == color)
        {
            if (!CanMove && !ReachedHome && GameManager.instance.NumberGot == 6)
            {
                GameManager.instance.NumberGot = 0;
                CanMove = true;
                prev = PathPoints[0];
                prev.AddPlayer(this);
                AlreadyMoved = 1;
                GameManager.instance.StopPlayerMoving_Coroutine();
                GameManager.instance.OnPlayerMove(true);
                return;
            }


            if (!CanMove
             || ReachedHome
             || GameManager.instance.IsPlayerMoving
             || GameManager.instance.NumberGot + AlreadyMoved > 57
             || GameManager.instance.TurnOf != color
             )
            {
                return;
            }

            GameManager.instance.StopPlayerMoving_Coroutine();
            StartCoroutine(MoveNSteps(GameManager.instance.NumberGot));
        }
    }

    private IEnumerator MoveNSteps(int n)
    {
        Debug.Log("Moving: " + n.ToString());
        GameManager.instance.IsPlayerMoving = true;

        for (int i = AlreadyMoved; i < AlreadyMoved + n; i++)
        {
            yield return new WaitForSeconds(MoveSpeed);
            PathPoints[i].AddPlayer(this);

            if (prev) prev.RemovePlayer(this);

            prev = PathPoints[i];
        }

        AlreadyMoved += n;

        bool killedOrReachedHome = CheckHomeOrKill();

        GameManager.instance.OnPlayerMove(killedOrReachedHome);
        GameManager.instance.IsPlayerMoving = false;

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
                GameManager.instance.OnWin(color);
            }
        }
        else if (PathPoints[AlreadyMoved - 1].Players.Count > 1)
        {
            for (int i = 0; i < PathPoints[AlreadyMoved - 1].Players.Count; i++)
            {
                Player p = PathPoints[AlreadyMoved - 1].Players[i];
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
