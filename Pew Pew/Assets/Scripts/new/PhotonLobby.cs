using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using TMPro;

public class PhotonLobby : MonoBehaviourPunCallbacks
{

    public static PhotonLobby Instance;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;



    void Awake()
    {
        Instance = this; // Creates the singleton, lives within the Main menu scene
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting to master");
        PhotonNetwork.ConnectUsingSettings(); // connects to photon server
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //callback // when Player is connect to master server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to Photon master server");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true; // to make all connected players (to host) to load a scene that's loaded on host system 
        //battleButton.SetActive(true);
    }

    //callback // when player has joined the lobby
    public override void OnJoinedLobby()
    {
        Debug.Log("Player has joined the lobby");
        MenuManager.Instance.OpenMenu("Title");
        PhotonNetwork.NickName = "Player"+Random.Range(0,1000).ToString("0000");
    }

    //to create a new room
    public void CreateRoom()
    {
        Debug.Log("Trying to create a new room");
        // int randomRoomName = Random.Range(0,10000); // generate a random number to concat with room name to avoid duplication
        // RoomOptions roomOps = new RoomOptions() 
        // {
        //     IsVisible = true,
        //     IsOpen = true,
        //     MaxPlayers = (byte)MultiplayerSettings.multiplayerSettings.maxPlayers
        // };
        if(string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("Loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("Loading");        
    }

    //callback// when player has joined the room
    public override void OnJoinedRoom()
    {
        Debug.Log("Room Joined");
        MenuManager.Instance.OpenMenu("Room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0 ; i<players.Length; i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(players[i]);
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    } 

    //callback// when host is changed
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }


    // callback // when Room creation is failed
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Tries to create a new room but failed, there must already be a room with the same name");
        errorText.text = "Room Creation failed: " + message;
        MenuManager.Instance.OpenMenu("Error");
        //CreateRoom();
    }

    public void startGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    // to leave the joined room
    public void LeaveRoom()
    {
        Debug.Log("Leaving Room");
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("Loading");
    }

    //callback// when successfully left the room
    public override void OnLeftRoom()
    {
        Debug.Log("Room Left");
        MenuManager.Instance.OpenMenu("Title");
    }    

    //callback// give us list of room info (like name nad max number of players etc)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }


        for (int i = 0; i < roomList.Count; i++)
        {
            if(roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().Setup(roomList[i]);
        }
    }

    //callback//
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
    }


// callback // when user failed to join a room
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //base.OnJoinRandomFailed(returnCode, message);

        Debug.Log("Tried to join a random game but fialed. There must be no open games available");
        CreateRoom();
    
    }

}
