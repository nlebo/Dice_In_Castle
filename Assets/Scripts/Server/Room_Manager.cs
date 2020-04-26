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

    public Text[] ID;
    public int[] Coin;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Instance = this;
        Coin = new int[2];
        ID[0].text = PhotonNetwork.LocalPlayer.NickName;

        if (PhotonNetwork.PlayerList.Length < 2)
            ID[1].gameObject.SetActive(false);
        else
            ID[1].text = PhotonNetwork.PlayerList[0].NickName;
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        ID[1].gameObject.SetActive(true);
        ID[1].text = newPlayer.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Ready()
    {

    }


    
}
