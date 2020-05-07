using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WarBoard_Manager : MonoBehaviourPun
{
    public struct Spawning_Info
    {
        public int What;
        public int camp;
        public int Lv;

        public Spawning_Info(int _what,int _camp,int _Lv)
        {
            What = _what;
            camp = _camp;
            Lv = _Lv;
        }
    }
    public static WarBoard_Manager m_Instance;
    public Unit_Manager[] Units;
    public Unit_Manager[] Miner;
    public Transform[] SpawnPoints;
    public int[] MaxUnit,UnitCount;
    public float SpawnTime,NT;
    public List<Spawning_Info> Spawn_List;
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
        Spawn_List = new List<Spawning_Info>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Board_Manager.m_Instance.Stop) return;

        if(PhotonNetwork.IsMasterClient)
            flowtime += Time.deltaTime;

        if(Spawn_List.Count > 0 && !RoundManager.m_Instance.RoundReady)
        {
            NT += Time.deltaTime;

            UI.SpawnTile_Fill(0,NT/SpawnTime);

            if(NT >= SpawnTime)
            {
                CreateUnit(Spawn_List[0].What,Spawn_List[0].camp,Spawn_List[0].Lv);
                Spawn_List.RemoveAt(0);
                UI.ReSort();
                NT = 0;
            }
        }
        
    }

    public bool AddSpawnList(int What,int camp,int Lv)
    {
        if(MaxUnit[camp] <= UnitCount[camp]) return false;
        UnitCount[camp]++;

        if (camp == 0)
            UI.UpdateText(UI.MaxNowUnit, $"({UnitCount[0]}/{MaxUnit[0]})");

        Spawn_List.Add(new Spawning_Info(What,camp,Lv));
        UI.SpawnTile_Add((Spawn_List.Count - 1) * 2,Units[What].GetComponent<SpriteRenderer>().sprite);
        return true;
    }

    public bool CreateUnit(int What,int PE,int Lv)
    {
        //if(MaxUnit[PE] <= UnitCount[PE]) return false;
        // U = PhotonNetwork.Instantiate(Units[What].gameObject.name,SpawnPoints[PE].position,Quaternion.identity).GetComponent<Unit_Manager>();
        // U.PlayerNum = PhotonNetwork.LocalPlayer.ActorNumber;
        U = Instantiate(Units[What].gameObject,SpawnPoints[PE]).GetComponent<Unit_Manager>();
        U.LV = Lv;
        //UnitCount[PE]++;

        // if (PE == 0)
        //     UI.UpdateText(UI.MaxNowUnit, $"({UnitCount[0]}/{MaxUnit[0]})");

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
