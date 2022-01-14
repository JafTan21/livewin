using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject host, other;
    [SerializeField] private TextMeshProUGUI roomname;
    [SerializeField] private Button LeaveButton, StartButton;

    PhotonView PV;

    private void Start()
    {
        PV = gameObject.GetComponent<PhotonView>();
        LeaveButton.onClick.AddListener(LeaveRoom);
        StartButton.interactable = false;
        StartButton.onClick.AddListener(StartGame);

        PhotonNetwork.AutomaticallySyncScene = true;

        roomname.text = "Room: \n" + PhotonNetwork.CurrentRoom.Name;
        if (PhotonNetwork.IsMasterClient)
        {
            host.transform.Find("name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.NickName;
        }
        else
        {
            other.transform.Find("name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.NickName;
        }
    }

    void StartGame()
    {
        PhotonNetwork.LoadLevel(6);
    }

    void LeaveRoom()
    {
        Debug.Log("Leaving room");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LudoEntryScene");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            StartButton.interactable = true;
        }
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        string _name = host.transform.Find("name").GetComponent<TextMeshProUGUI>().text;
        host.transform.Find("name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.NickName;
        other.transform.Find("name").GetComponent<TextMeshProUGUI>().text = _name;

    }
}
