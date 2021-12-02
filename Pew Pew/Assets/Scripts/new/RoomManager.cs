using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    //room info
    public static RoomManager Instance;
    private PhotonView PV; // used to send message from one client to other clients using RPC calls
    //public bool isGameLoaded;
    private int currentScene;

    // player info
    private Player[] photonPlayers; //array holding list of all players in room


    void Awake()
    {
        //setup singleton
        //create one instance and replace it with any new Instances
        if(Instance) //checks if another room exist
        {
            Destroy(gameObject); // destroys itself and return
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
        PV = GetComponent<PhotonView>();
    }


    public override void OnEnable()
    {
        // When ever scene is loaded OnSceneFinishedLoading is called
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading; //registering action event on loading a new scene
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading; // removing action event 
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

       
    }

    
// callback // when another person joins our room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player has joined the room");
        

    }


//loads multiplayer scene for all players
    void StartGame()
    {
        Debug.Log("Loading Level"); 
        PhotonNetwork.LoadLevel(1);
    }


// 
    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //called when multiplayer scene is loaded
        currentScene = scene.buildIndex;
        if(currentScene == 1) // we're in the game scene
        {
            
            //RPC_CreatePlayer();
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity, 0);

        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
       
    }

    [PunRPC]  //creates player network controller but not player character
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
    }


}
