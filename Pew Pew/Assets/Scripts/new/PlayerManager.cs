using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    GameObject controller;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if(PV.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        Transform spawnPoint = SpawnManager.Instance.GetRandomSpawnPoint();
        Debug.Log("Instantiate Player");
        //instantiate player controller
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnPoint.position, spawnPoint.rotation, 0, new object[] {PV.ViewID});
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
    }
}
