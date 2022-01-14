using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LudoVarientsController : MonoBehaviourPunCallbacks
{
    public Button multiplayer, computer, custom, offline, leave;
    public GameObject _multiplayerPanel, _computerPanel, _customPanel, _offlinePanel;



    private void Awake()
    {
        multiplayer.onClick.AddListener(Multiplayer);
        computer.onClick.AddListener(Computer);
        custom.onClick.AddListener(Custom);
        offline.onClick.AddListener(Offline);
        leave.onClick.AddListener(Leave);

        Reconnect();
    }


    void Reconnect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("reconnecting");
            PhotonNetwork.ConnectUsingSettings();
            PopUp.instance.ClearAll();
            PopUp.instance.Loading("Connecting to server.", false);
        }
    }

    public override void OnConnectedToMaster()
    {
        PopUp.instance.ClearAll();
        // PopUp.instance.Success("Connected");
        Debug.Log("Connected");
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PopUp.instance.Error("Disconnected from server: " + cause.ToString());
        Reconnect();
    }














    void Multiplayer()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom(); return;
        }
        if (PhotonNetwork.IsConnectedAndReady)
        {
            OpenPanel(_multiplayerPanel);
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Reconnect();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // no player online
        Debug.Log("creating room: " + message);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("player entered");

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            PhotonNetwork.LoadLevel(6);
        }
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("joined room");
    }

    void Leave()
    {
        if (PhotonNetwork.InRoom)
        {
            leave.interactable = false;
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            _multiplayerPanel.SetActive(false);
        }
    }

    public override void OnLeftRoom()
    {
        leave.interactable = true;
        _multiplayerPanel.SetActive(false);
    }









    void Computer()
    { OpenPanel(_computerPanel); }





















    void Custom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            OpenPanel(_customPanel);
        }
        else
        {
            Reconnect();
        }
    }
























    void Offline()
    { OpenPanel(_offlinePanel); }
























    void OpenPanel(GameObject panel)
    {
        Debug.Log("Opening: " + panel.name);
        panel.SetActive(true);
        panel.transform.localScale = Vector3.zero;
        panel.LeanScale(Vector3.one, 0.3f).setEaseInQuart();

    }
}
