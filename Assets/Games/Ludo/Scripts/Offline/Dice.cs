using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public bool CanRoll = true;
    public float RollTime;
    public string color;
    public SpriteRenderer DiceResultRenderer;
    public List<Player> Players = new List<Player>();

    private void Start()
    {
        RollTime = 0;
        GameObject[] _objects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in _objects)
        {
            Player p = obj.GetComponent<Player>();
            if (p.color == color)
            {
                Players.Add(p);
            }
        }

        CanRoll = true;
    }

    public void OnMouseDown()
    {
        if (GameManager.instance.IsRolling
        || GameManager.instance.IsPlayerMoving
        || GameManager.instance.TurnOf != color
        || !CanRoll
        )
        {
            Debug.Log("clicked but not rolled: " + color);
            return;
        }

        int n = Random.Range(1, 7);
        StartCoroutine(RollDiceTo(n));
    }


    public IEnumerator RollDiceTo(int n)
    {
        CanRoll = false;
        GameManager.instance.StopDiceRoller_Coroutine(); // stop auto rolling
        GameManager.instance.IsRolling = true;

        // animation start
        float timeLeft = RollTime;
        while (timeLeft > 0)
        {
            timeLeft -= 0.05f;
            yield return new WaitForSeconds(0.01f);
            DiceResultRenderer.sprite = GameManager.instance.DiceAnimationSprites[Random.Range(0, 6)];
        }
        // animation end
        DiceResultRenderer.sprite = GameManager.instance.DiceSprites[n - 1];


        GameManager.instance.IsRolling = false;
        GameManager.instance.NumberGot = n;
        GameManager.instance.Rolled = color;

        // call on dice roll
        GameManager.instance.OnDiceRoll(Players);
    }
}
