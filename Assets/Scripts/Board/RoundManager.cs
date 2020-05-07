using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    Board_Manager Board;
    WarBoard_Manager WarBoard;
    UI_Manager UI;

    public static RoundManager m_Instance;

    int Round;
    public int RoundCoin;
    public bool RoundReady;
    public float ReadyTime,NRT; // 대기시간 , Now Ready Time
    // Start is called before the first frame update
    void Start()
    {
        Board = Board_Manager.m_Instance;
        WarBoard = WarBoard_Manager.m_Instance;
        UI = UI_Manager.m_Instance;
        m_Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Board.Stop || !RoundReady) return;

        NRT += Time.deltaTime;
        UI.UpdateText(UI.RemainPrepareTime,$"남은 전투 대기시간: {(int)(ReadyTime - NRT)}초");

        if(NRT >= ReadyTime)
        {
            Board.Stop = false;
            RoundReady = false;
            UI.RemainPrepareTime.gameObject.SetActive(false);
            UI.Skip.SetActive(false);
        }
    }

    public void ReadyForRound()
    {
        Round++;
        RoundReady = true;
        NRT = 0;
        Board.Stop = true;

        Board.UpCoin(RoundCoin);
        UI.RemainPrepareTime.gameObject.SetActive(true);
        UI.Skip.SetActive(true);
    }

    public void SkipReady()
    {
        NRT = ReadyTime;
    }
}
