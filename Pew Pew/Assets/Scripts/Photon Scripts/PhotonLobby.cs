using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PhotonLobby : MonoBehaviourPunCallbacks
{

    public static PhotonLobby lobby;
    public GameObject battleButton;
    public GameObject cancelButton;

    void Awake()
    {
        lobby = this; // Creates the singleton, lives within the Main menu scene

    
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // connects to photon server
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //base.OnJoinRandomFailed(returnCode, message);

        Debug.Log("Tried to join a random game but fialed. There must be no open games available");

        CreateRoom();
    
    }

    void CreateRoom()
    {
        Debug.Log("Trying to create a new room");
        int randomRoomName = Random.Range(0,10000);
        RoomOptions roomOps = new RoomOptions() 
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 10
        };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Tries to create a new room but failed, there must already be a room with the same name");
        CreateRoom();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to Photon master server");
        battleButton.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("We are in room now");
    }

    public void OnCancelButtonClicked()
    {
        Debug.Log("Cancel button was clicked");
        cancelButton.SetActive(false);
        battleButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
    public void OnBattleButtonCLicked()
    {
        Debug.Log("Battle Button was clicked");
        battleButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }
}
