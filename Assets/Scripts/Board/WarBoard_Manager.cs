using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WarBoard_Manager : MonoBehaviourPun
{
    public static WarBoard_Manager m_Instance;
    public Unit_Manager[] Units;
    public Unit_Manager[] Miner;
    public Transform[] SpawnPoints;
    public int[] MaxUnit,UnitCount;
    Camera_Manager Cam;
    Unit_Manager U;

    public Transform[] OreStop;
    public Transform OreT;
    UI_Manager UI;

    float flowtime;
    // Start is called before the first frame update
    void Start()
    {
        m_Instance = this;
        Cam = Camera_Manager.m_Instance;
        flowtime = 0;
        UnitCount = new int[2];
        UI = UI_Manager.m_Instance;
        UI.MaxNowUnit.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.IsMasterClient && !Board_Manager.m_Instance.Stop)
            flowtime += Time.deltaTime;
    }

    public bool CreateUnit(int What,int PE,int Lv)
    {
        if(MaxUnit[PE] <= UnitCount[PE]) return false;
        U = PhotonNetwork.Instantiate(Units[What].gameObject.name,SpawnPoints[PE].position,Quaternion.identity).GetComponent<Unit_Manager>();
        U.PlayerNum = PhotonNetwork.LocalPlayer.ActorNumber;
        U.LV = Lv;
        UnitCount[PE]++;

        if (PE == 0)
            UI.UpdateText(UI.MaxNowUnit, $"({UnitCount[0]}/{MaxUnit[0]})");

            return true;
    }
    public void CreateMiner(int What, int PE, int Lv)
    {
        if (MaxUnit[PE] <= UnitCount[PE]) return;

        U = PhotonNetwork.Instantiate(Miner[What].gameObject.name, SpawnPoints[PE].position, Quaternion.identity).GetComponent<Unit_Manager>();
        U.PlayerNum = PhotonNetwork.LocalPlayer.ActorNumber;
        U.LV = Lv;
        UnitCount[PE]++;

        if (PE == 0)
            UI.UpdateText(UI.MaxNowUnit, $"({UnitCount[0]}/{MaxUnit[0]})");
    }
}
