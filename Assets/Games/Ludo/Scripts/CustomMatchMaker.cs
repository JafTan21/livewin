using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;


public class CustomMatchMaker : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField RoomNameInput;
    [SerializeField] private Button JoinButton, CreateButton;


    private void Start()
    {
        JoinButton.onClick.AddListener(Join);
        CreateButton.onClick.AddListener(Create);
    }

    void Join()
    {
        string _name = RoomNameInput.text;
        PhotonNetwork.JoinRoom(_name);
    }

    void Create()
    {
        JoinButton.interactable = false;
        CreateButton.interactable = false;

        PopUp.instance.Loading("Creating new room");

        ExitGames.Client.Photon.Hashtable custom = new ExitGames.Client.Photon.Hashtable();
        custom["IsCustom"] = true;
        custom["Creator"] = AuthManager.instance.username;

        string roomName = AuthManager.instance.username + "_" + Random.Range(100, 10000);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = false;
        roomOptions.IsOpen = true;
        roomOptions.CustomRoomProperties = custom;


        Debug.Log("Room created: " + roomName);
        PhotonNetwork.CreateRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.NickName = AuthManager.instance.username;
        Debug.Log("joined: " + PhotonNetwork.NickName);
        Debug.Log("Room name: " + PhotonNetwork.CurrentRoom.Name);
        SceneManager.LoadScene("LobbyScene");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PopUp.instance.Error("Join failed: " + message);
    }
}
