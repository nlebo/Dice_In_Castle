using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room_Manager : MonoBehaviourPunCallbacks
{
    public static Room_Manager m_Instance;
    public Select_Dice Select_Page;
    public GameObject ready;

    public Castle_Manager[] Castle;

    public Text[] ID;
    public int[] Coin;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Instance = this;
        Coin = new int[2];
        // ID[0].text = PhotonNetwork.LocalPlayer.NickName;

        // if (PhotonNetwork.PlayerList.Length < 2)
        //     ID[1].gameObject.SetActive(false);
        // else
        //     ID[1].text = PhotonNetwork.PlayerList[0].NickName;
    }

    // public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    // {
    //     ID[1].gameObject.SetActive(true);
    //     ID[1].text = newPlayer.NickName;
    // }
    public void HitCastle(int Damage, int HitCastle)
    {
        // int hit = HitCastle == PhotonNetwork.LocalPlayer.ActorNumber-1 ? 0 : 1;
        int hit = HitCastle;
        Castle[hit].HP -= Damage;
        Castle[hit].HP_Bar.fillAmount = 1 - (float)Castle[hit].HP/Castle[hit].MaxHP;
    }
    public void DeathCastle()
    {
        PhotonNetwork.LeaveRoom(true);
        PhotonNetwork.LoadLevel("LogIn");
    }
    // [PunRPC]
    // void RPCHitCastle(int Damage, int HitCastle)
    // {
    //     int hit = HitCastle == PhotonNetwork.LocalPlayer.ActorNumber-1 ? 0 : 1;
    //     Castle[hit].HP -= Damage;
    //     Castle[hit].HP_Bar.fillAmount = 1 - (float)Castle[hit].HP/Castle[hit].MaxHP;
    // }

    // [PunRPC]
    // void RPCDeathCastle()
    // {
    //     PhotonNetwork.LeaveRoom(true);
    //     PhotonNetwork.LoadLevel("LogIn");
    // }


    
}
