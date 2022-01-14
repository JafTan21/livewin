using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlinePathPoint : MonoBehaviour
{
    public bool IsStoppage;
    public List<OnlinePlayer> Players = new List<OnlinePlayer>();

    public void AddPlayer(OnlinePlayer player)
    {
        Debug.Log("adding: " + player.color);
        Players.Add(player);
        PositionPlayers();
    }

    public void RemovePlayer(OnlinePlayer player)
    {
        if (Players.Contains(player))
        {

            Debug.Log("removing: " + player.color);

            Players.Remove(player);
            PositionPlayers();
        }
    }

    private void PositionPlayers()
    {
        bool isLeft = true;
        int orderInLayer = Players.Count;
        for (int i = 0; i < Players.Count; i++)
        {
            isLeft = i % 2 == 0;
            Vector3 offset = new Vector3(
                    (isLeft ? 0.07f * (i + 1) / 2 : -0.07f * i / 2),
                    0, 0);
            Players[i].transform.position = transform.position + offset;
            Players[i].GetComponent<SpriteRenderer>().sortingOrder = orderInLayer--;
        }
    }

}
