using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] protected float WaitingTime = 1f;
    [SerializeField] protected bool GameEnded = false;

    // *** DICE
    [SerializeField] public Sprite[] diceSprites;
    [SerializeField] protected int NumberGot;
    [SerializeField] protected bool IsRolling, IsPlayerMoving;


    // **** player
    [SerializeField]
    public GameObject BluePlayerPrefab,
                        RedPlayerPrefab,
                        GreenPlayerPrefab,
                        YellowPlayerPrefab;
    [SerializeField] public int TurnIndex;


    // **** Path Point
    [SerializeField]
    protected PathPoint[] RedPathPoints,
                        GreenPathPoints,
                        YellowPathPoints,
                        BluePathPoints;

    protected string[] colors = { "green", "blue", "yellow", "red" };

}
