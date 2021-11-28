using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;
    public Transform[] spawnPoints; // locations for player spawn points
    // Start is called before the first frame update
    void OnEnable()
    {
        //making singleton
        if( GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }
}
