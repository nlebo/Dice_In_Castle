using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WarBoard_Manager : MonoBehaviourPun
{
    public static WarBoard_Manager m_Instance;
    public Unit_Manager[] Units;
    public Transform[] SpawnPoints;
    Camera_Manager Cam;
    Unit_Manager U;
    // Start is called before the first frame update
    void Start()
    {
        m_Instance = this;
        Cam = Camera_Manager.m_Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateUnit(int What,int PE,int Lv)
    {
        U = PhotonNetwork.Instantiate(Units[What].gameObject.name,SpawnPoints[PE].position,Quaternion.identity).GetComponent<Unit_Manager>();
        U.PlayerNum = PhotonNetwork.LocalPlayer.ActorNumber;
        U.LV = Lv;
    }

}
