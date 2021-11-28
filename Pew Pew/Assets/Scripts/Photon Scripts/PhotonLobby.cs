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

//to create a new room
    void CreateRoom()
    {
        Debug.Log("Trying to create a new room");
        int randomRoomName = Random.Range(0,10000); // generate a random number to concat with room name to avoid duplication
        RoomOptions roomOps = new RoomOptions() 
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)MultiplayerSettings.multiplayerSettings.maxPlayers
        };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

// callback // when user failed to join a room
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //base.OnJoinRandomFailed(returnCode, message);

        Debug.Log("Tried to join a random game but fialed. There must be no open games available");
        CreateRoom();
    
    }

// callback // when Room creation is failed
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Tries to create a new room but failed, there must already be a room with the same name");
        CreateRoom();
    }

//callback // when Player is connect to master server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to Photon master server");
        PhotonNetwork.AutomaticallySyncScene = true; // to make all connected players (to host) to load a scene that's loaded on host system 
        battleButton.SetActive(true);
    }



// when cancel button is clicked
    public void OnCancelButtonClicked()
    {
        Debug.Log("Cancel button was clicked");
        cancelButton.SetActive(false);
        battleButton.SetActive(true);
        PhotonNetwork.LeaveRoom(); 
    }
//when battle button is clicked
    public void OnBattleButtonCLicked()
    {
        Debug.Log("Battle Button was clicked");
        battleButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }
}
