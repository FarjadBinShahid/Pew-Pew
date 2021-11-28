using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AvatarSetup : MonoBehaviour
{
    public int playerHealth;
    public int playerDamage;
    private PhotonView PV;
    public GameObject myCharacter;
    public int characterValue;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        //check if 
        if(PV.IsMine) 
        {
            PV.RPC(/*function to execute on receiving end*/ "RPC_AddCharacter",/* whom to send*/ RpcTarget.AllBuffered,/* parameter to send*/ PlayerInfo.PI.mySelectedCharacter); //sends a broadcast to users defined (RpcTarget.AllBuffered) with a variable and a function name that will be executed on the receiving end
        }
    }

    [PunRPC]
    void RPC_AddCharacter(int whichCharacter) // method that runs on receiving end of broadcast
    {
        characterValue = whichCharacter;
        myCharacter = Instantiate(PlayerInfo.PI.allCharacters[whichCharacter], transform.position, transform.rotation, transform);
    }
}
