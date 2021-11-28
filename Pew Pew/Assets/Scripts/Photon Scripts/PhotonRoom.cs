using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    //room info
    public static PhotonRoom room;
    private PhotonView PV; // used to send message from one client to other clients using RPC calls
    public bool isGameLoaded;
    public int currentScene;

    // player info
    private Player[] photonPlayers; //array holding list of all players in room
    public int playersInRoom;
    public int myNumberInRoom;
    public int playersInGame;

    //Delayed start
    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayers;
    private float timetoStart;

    void Awake()
    {
        //setup singleton
        //create one instance and replace it with any new Instances
        if(PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }else
        {
            if(PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }


    public override void OnEnable()
    {
        // When ever scene is loaded OnSceneFinishedLoading is called
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }



    // Start is called before the first frame update
    void Start()
    {
        //set private variables
        PV = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayers = 6;
        timetoStart = startingTime;
    }

    // Update is called once per frame
    void Update()
    {

        // for dealy start play
        if(MultiplayerSettings.multiplayerSettings.delaystart)
        {
            if(playersInRoom == 1)
            {
                RestartTimer();
            }
            if(!isGameLoaded)
            {
                if(readyToStart)
                {
                    atMaxPlayers -= Time.deltaTime;
                    lessThanMaxPlayers = atMaxPlayers;
                    timetoStart = atMaxPlayers;
                }
                else if(readyToCount)
                {
                    lessThanMaxPlayers -= Time.deltaTime;
                    timetoStart = lessThanMaxPlayers;
                }

                Debug.Log("Display time to start to the players " + timetoStart);
                if(timetoStart <= 0)
                {
                    StartGame();
                }
            
            }
        }
    }

    // callback // when user has joined the room
    public override void OnJoinedRoom()
    {
        //sets player data when we join the room
        base.OnJoinedRoom();
        Debug.Log("We are in room now");
        photonPlayers = PhotonNetwork.PlayerList; // gets the list of players in room
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();

        if(MultiplayerSettings.multiplayerSettings.delaystart)
        {
            Debug.Log("Player in room out of max players possible (" +playersInRoom + ":" + MultiplayerSettings.multiplayerSettings.maxPlayers + ")" );

            if(playersInRoom > 1)
            {
                readyToCount = true;
            }

            //checks if room is full
            if(playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers)
            {
                readyToStart = true;
                if(!PhotonNetwork.IsMasterClient)
                {
                    return;
                }
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }else
        {
            StartGame();
        }
    }


// callback // when another person joins our room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player has joined the room");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;

        if(MultiplayerSettings.multiplayerSettings.delaystart)
        {
            Debug.Log("players in room out of max players possiple (" + playersInRoom + ":" +MultiplayerSettings.multiplayerSettings.maxPlayers + ")");
            if(playersInRoom > 1)
            {
                readyToCount = true;
            }
            if(playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers)
            {
                readyToStart = true;

                if(!PhotonNetwork.IsMasterClient)
                {
                    return;
                }
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }

    }


// To load us in multiplayer scene
    void StartGame()
    {
        isGameLoaded = true;

        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        if(MultiplayerSettings.multiplayerSettings.delaystart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.multiplayerScene);
    }

    void RestartTimer()
    {
        lessThanMaxPlayers = startingTime;
        timetoStart = startingTime;
        atMaxPlayers = 6;
        readyToCount = false;
        readyToStart = false;
    }

// 
    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //called when multiplayer scene is loaded
        currentScene = scene.buildIndex;
        if(currentScene == MultiplayerSettings.multiplayerSettings.multiplayerScene)
        {
            isGameLoaded = true;
            if(MultiplayerSettings.multiplayerSettings.delaystart)
            {
                PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            }else
            {
                RPC_CreatePlayer();
            }
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playersInGame ++;
        if(playersInGame == PhotonNetwork.PlayerList.Length)
        {
            PV.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
    }


}
