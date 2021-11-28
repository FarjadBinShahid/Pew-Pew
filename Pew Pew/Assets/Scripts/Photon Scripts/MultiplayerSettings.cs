using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSettings : MonoBehaviour
{
    public static MultiplayerSettings multiplayerSettings;
    public bool delaystart; // if false user join with continous loading game if true user join with delay
    public int maxPlayers; // max player in room
    public int menuScene; // Menu scene index
    public int multiplayerScene; // multiplayer scene index

    void Awake()
    {
        // creates one instance and delete duplicates after that
        if (MultiplayerSettings.multiplayerSettings == null)
        {
            MultiplayerSettings.multiplayerSettings = this;
        }else
        {
            if(MultiplayerSettings.multiplayerSettings != this)
            {
                Destroy(this.gameObject); 
            }
        }

        DontDestroyOnLoad(this.gameObject);
        
    }
}
