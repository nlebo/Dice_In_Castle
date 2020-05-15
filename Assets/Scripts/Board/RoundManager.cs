using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    Board_Manager Board;
    WarBoard_Manager WarBoard;
    UI_Manager UI;

    public static RoundManager m_Instance;
    public static UnityAction R_Finish;

    public UnityAction RoundCountAction;
    
    int _RoundCount = 0;
    public int RoundCount
    {
        get{
            return _RoundCount;
        }
        set{
            _RoundCount = value;

            if(_RoundCount == 0)
            {
                if(RoundCountAction != null) RoundCountAction();
            }
        }
    }

    public int Round;
    public int RoundCoin;
    public bool RoundReady;
    public float ReadyTime,NRT; // 대기시간 , Now Ready Time

    int EventRound;
    // Start is called before the first frame update
    void Start()
    {
        EventRound = Random.Range(3,8);
        Board = Board_Manager.m_Instance;
        WarBoard = WarBoard_Manager.m_Instance;
        UI = UI_Manager.m_Instance;
        m_Instance = this;
        RoundCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!Board.Stop && !RoundReady){
            CheckRoundEnd(); 
            return;
        }

        if(!RoundReady) return;

        NRT += Time.deltaTime;
        UI.UpdateText(UI.RemainPrepareTime,$"남은 전투 대기시간: {(int)(ReadyTime - NRT)}초");

        if(NRT >= ReadyTime)
        {
            Board.Stop = false;
            RoundReady = false;
            UI.RemainPrepareTime.gameObject.SetActive(false);
            UI.Skip.SetActive(false);
            Enemy_Spawn.m_Instance.RoundStart();
            WarBoard.RoundStart();
        }
    }

    public void ReadyForRound()
    {
        if(RoundCount > 0) RoundCount--;
        for(int i = 0; i < WarBoard.SpawnPoints[0].childCount;i++)
        {
            Destroy(WarBoard.SpawnPoints[0].GetChild(i).gameObject);
        }
        Round++;
        UI.UpdateText(UI.RoundText,$"Round {Round}");
        RoundReady = true;
        NRT = 0;
        Board.Stop = true;

        Board.UpCoin(RoundCoin);
        UI.RemainPrepareTime.gameObject.SetActive(true);
        UI.Skip.SetActive(true);
        WarBoard.RoundEnd();

        if(R_Finish != null) R_Finish();
    }

    public void SkipReady()
    {
        NRT = ReadyTime;
    }

    public void CheckRoundEnd()
    {
        if(WarBoard.Spawn_List.Count - WarBoard.PCount <= 0 && WarBoard.SpawnPoints[1].childCount <= 0)
        {
            
            if (Round == EventRound)
            {
                EventRound = Round + Random.Range(3,8);
                Board.Stop = true;
                CardSystem.m_Instance.Start_Event();
            }
            else
            {
                ReadyForRound();
            }
        }
    }
}
