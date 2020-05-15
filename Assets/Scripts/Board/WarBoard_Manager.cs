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

    public PreparedUnit[] PBoard;
    public int PCount;

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
        PCount = 0;
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
                CreateUnit(Spawn_List[0].What,Spawn_List[0].camp,Spawn_List[0].Lv,PCount);
                Spawn_List.RemoveAt(0);
                PCount++;
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
        // UI.SpawnTile_Add((Spawn_List.Count - 1) * 2,Units[What].GetComponent<SpriteRenderer>().sprite);


        return true;
    }

    public bool AddSpawnList(int What,int camp,int Lv,GameObject Orgi,int where)
    {
        if(MaxUnit[camp] <= UnitCount[camp]) return false;
        UnitCount[camp]++;

        if (camp == 0)
            UI.UpdateText(UI.MaxNowUnit, $"({UnitCount[0]}/{MaxUnit[0]})");

        PBoard[Spawn_List.Count].OriginalDice = Orgi;
        PBoard[Spawn_List.Count].Where = where;
        PBoard[Spawn_List.Count].Num = Spawn_List.Count;
        PBoard[Spawn_List.Count].sprite.sprite = Units[What].GetComponent<SpriteRenderer>().sprite;
        PBoard[Spawn_List.Count].gameObject.SetActive(true);
        Spawn_List.Add(new Spawning_Info(What,camp,Lv));
        PCount = 0;        


        return true;
    }

    public bool CreateUnit(int What,int PE,int Lv,int Num)
    {
        //if(MaxUnit[PE] <= UnitCount[PE]) return false;
        // U = PhotonNetwork.Instantiate(Units[What].gameObject.name,SpawnPoints[PE].position,Quaternion.identity).GetComponent<Unit_Manager>();
        // U.PlayerNum = PhotonNetwork.LocalPlayer.ActorNumber;
        U = Instantiate(Units[What].gameObject,SpawnPoints[PE]).GetComponent<Unit_Manager>();
        U.LV = Lv;
        U.PBoard_Num = Num;
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

    public void RoundStart()
    {
        for (int i = 0; i < UnitCount[0]; i++)
        {
            UI.SpawnTile_Add((i) * 2, PBoard[i].sprite.sprite);
            //PBoard[i].gameObject.SetActive(false);
        }
        PBoard_Manager._Disable();
        
    }

    public void RoundEnd()
    {
        PCount=0;
        
        PBoard_Manager._Enable();
        SortPBoard();
        MakeSpawnList();
    }

    public void DropUnit(int Num)
    {
        Spawn_List.RemoveAt(Num);
        // for(int i=Num + 1;i<UnitCount[0];i++)
        // {
        //     PBoard[i-1].OriginalDice = PBoard[i].OriginalDice;
        //     PBoard[i-1].Where = PBoard[i].Where;
        //     PBoard[i-1].sprite.sprite = PBoard[i].sprite.sprite;
        //     PBoard[i-1].gameObject.SetActive(true);
        //     PBoard[i].OriginalDice = null;
        //     PBoard[i].gameObject.SetActive(false);

        // }
        PBoard[Num].OriginalDice = null;
        SortPBoard();
        UnitCount[0]--;
        UI.UpdateText(UI.MaxNowUnit, $"({UnitCount[0]}/{MaxUnit[0]})");
    }

    public void DeathUnit(int Num)
    {
        Board_Manager.m_Instance.DeleteDice(PBoard[Num].Where);
        PBoard[Num].OriginalDice = null;
        PBoard[Num].gameObject.SetActive(false);
    }
    public void SortPBoard()
    {
        int count = 0;
        for(int i =0; i < PBoard.Length ;i++)
        {
            if (PBoard[i].OriginalDice==null)
            {
                int C_Count = 0;
                for (int j = i+1; j < PBoard.Length; j++)
                {
                    if (PBoard[j].OriginalDice!=null)
                    {
                        PBoard[i + C_Count].OriginalDice = PBoard[j].OriginalDice;
                        PBoard[i + C_Count].Where = PBoard[j].Where;
                        PBoard[i + C_Count].sprite.sprite = PBoard[j].sprite.sprite;
                        PBoard[j].gameObject.SetActive(false);
                        PBoard[j].OriginalDice = null;
                        PBoard[i + C_Count].gameObject.SetActive(true);
                        C_Count++;
                        count++;
                    }

                    //if(count >= Spawn_List.Count) break;
                }
            }
            else
                count++;
        }
    }
    public void MakeSpawnList()
    {
        for(int i=0;i<PBoard.Length;i++)
        {
            if (PBoard[i].OriginalDice!=null)
            {
                Dice_Manager dice = PBoard[i].OriginalDice.GetComponent<Dice_Manager>();
                Spawn_List.Add(new Spawning_Info(dice.kind_id,0,dice.Level));
            }
            else break;
        }
    }

    public void UpMaxUnit(int N)
    {
        MaxUnit[0] += N;
        UI.UpdateText(UI.MaxNowUnit, $"({UnitCount[0]}/{MaxUnit[0]})");
    }
    
}
