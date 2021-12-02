using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using TMPro;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public RoomInfo info;

    public void Setup(RoomInfo _info)
    {
        info = _info;
        text.text = _info.Name;
    }

    public void OnCLick()
    {
        PhotonLobby.Instance.JoinRoom(info);
    }
}
