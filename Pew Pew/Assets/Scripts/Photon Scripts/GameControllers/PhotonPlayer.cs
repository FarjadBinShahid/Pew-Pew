using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView PV;
    private GameObject myAvatar;

    // Start is called before the first frame updatee
    void Start()
    {
        
        PV = GetComponent<PhotonView>();
        int spawnPicker = Random.Range(0, GameSetup.GS.spawnPoints.Length); // randomly selecting from spawn points
        //to make sure to only Instantiate avatar for my player only
        if(PV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate
            (
                Path.Combine("PhotonPrefabs","PlayerAvatar"),
                GameSetup.GS.spawnPoints[spawnPicker].position, 
                GameSetup.GS.spawnPoints[spawnPicker].rotation, 
                0
            );
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
