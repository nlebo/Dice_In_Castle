using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Manager : MonoBehaviourPunCallbacks
{
    public static Room_Manager m_Instance;
    public Select_Dice Select_Page;
    public GameObject ready;

    public int[] Coin;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Instance = this;
        Debug.Log(photonView.ViewID);
        CreateSelect();
        Coin = new int[2];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Ready()
    {

    }

    public void CreateSelect()
    {
        if(!PhotonNetwork.IsMasterClient) return;

        Select_Page = PhotonNetwork.Instantiate(Select_Page.gameObject.name,Vector3.zero,Quaternion.identity).GetComponent<Select_Dice>();
    }

    
}
