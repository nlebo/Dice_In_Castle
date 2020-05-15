using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardSystem : MonoBehaviour
{
    public static CardSystem m_Instance;
    string[] Event_Name;
    string[] Event_Info;
    string[,] selection;


    string[] PrisonerEvent_Name;
    string[] PrisonerEvent_Info;
    string[,] PrisonerEvent_Selection; 

    bool Prisoner;
    bool Missing_Prisoner;
    bool Running_Prisoner;

    [SerializeField]
    GameObject EventHandle;
    [SerializeField]
    Text EventName_Text;
    [SerializeField]
    Text EventInfo_Text;
    [SerializeField]
    Text[] EventSelection_Text;

    [SerializeField]
    GameObject EventResultHandle;
    [SerializeField]
    Text EventResultName_Text;
    [SerializeField]
    Text EventResultInfo_Text;
    

    int EventNum;
    float _ResultCount = 0;
    float ResultCount
    {
        get{
            return _ResultCount;
        }
        set{
            _ResultCount = value;

            if(_ResultCount <= 0 && ResultAction != null) ResultAction();
        }
    }
    string EventCategory;

    Board_Manager Board;
    WarBoard_Manager WarBoard;
    RoundManager Round;
    UnityAction ResultAction;
    // Start is called before the first frame update
    void Start()
    {
        ResultCount = 0;
        ResultAction += ResultHandler_Disable;
        Round = RoundManager.m_Instance;
        Board = Board_Manager.m_Instance;
        WarBoard = WarBoard_Manager.m_Instance;
        m_Instance = this;
        Event_Name = new string[] {
            "홍수",
            "변절자"
            };
        
        PrisonerEvent_Name = new string[] {
            "탈출",
            "탈출-2 (1)",
            "탈출-2 (2)",
            "탈출-2 (3)",
            "약탈",
            "자유"};



        Event_Info = new string[] {
            "성주변에 홍수가 일어났습니다.",
            "적 유닛 3명이 멀리서 하얀 깃발을 흔들며 오고있습니다."};

        PrisonerEvent_Info = new string[]
        {
            "포로들이 탈출하였습니다.",
            "포로들을 잡아들였습니다.",
            "포로를 잡는중에 포로를 사살하였습니다.",
            "포로를 놓쳤습니다.",
            "자유를 가진 포로들은 약탈후 성을 빠져나갔습니다.",
            "포로들은 시민이 되었습니다."};


        selection = new string[,] {
            {"얼음 주사위를 보낸다.","벽을 높게 올려 홍수를 막는다.(-5금화)","대피 명령을 내린다."},
            {"성에 근접하면 죽인다.","항복을 받아들이고 부대로 합류시킨다.","항복을 받아들이고 포로로 들인다."}};

        PrisonerEvent_Selection = new string[,] {
            {"3골드의 현상금을 건다.","내버려 둔다.","병력을 풀어 포로를 찾는다."},
            {"사형을 집행한다.","강제 노역행에 처한다.","포로 수용소에 가둔다."},
            {"전리품을 챙긴다","",""},
            {"한숨..","",""},
            {"약탈당한 주민들을 위로합니다(-3금화)","",""},
            {"직업을 가지게합니다.","병사로 훈련을 시킵니다.",""}};

    }

    private void Update() {
        if(ResultCount <= 0) return;

        ResultCount -= Time.deltaTime;
    }
    public void Start_Event()
    {
        if(Prisoner && Random.Range(0,3) != 0)
        {
            Up_Event(Random.Range(0,1),"Prisoner");
        }
        else if(Missing_Prisoner && Random.Range(0,3) != 0)
        {
            Up_Event(Random.Range(1,4),"Prisoner");
        }
        else if(Running_Prisoner && Random.Range(0,3) != 0)
        {
            Up_Event(Random.Range(4,6),"Prisoner");
        }
        else
        {
            Up_Event(Random.Range(0,Event_Name.Length),"Normal");
        }
        
    }
    void Up_Event(int Num,string Category)
    {
        string[] Name;
        string[] Info;
        string[,] Selection;
        switch(Category)
        {
            case "Normal":
            Name = Event_Name;
            Info = Event_Info;
            Selection = selection;
            break;
            case "Prisoner":
            Name = PrisonerEvent_Name;
            Info = PrisonerEvent_Info;
            Selection = PrisonerEvent_Selection;
            break;
            default:
            Name = null;
            Info = null;
            Selection = null;
            break;
        }
        
        if(Name.Length <= Num) return;

        EventHandle.SetActive(true);
        EventName_Text.text = Name[Num];
        EventInfo_Text.text = Info[Num];

        for(int i =0; i<EventSelection_Text.Length;i++)
            EventSelection_Text[i].text = Selection[Num,i];

        EventNum = Num;
        EventCategory = Category;
        
    }

    public void Select(int Num)
    {
        string Result = "";
        string Result_Info = "";

        string check = "";
        if(EventCategory == "Normal")
        {
            check = selection[EventNum,Num];
        }
        else if(EventCategory == "Prisoner")
        {
            check = PrisonerEvent_Selection[EventNum,Num];
        }

        if(check == "") return;

        switch (EventCategory)
        {
            case "Normal":
                switch (EventNum)
                {
                    case 0: //----------------------------------------------------------------------------------홍수
                        switch (Num)
                        {
                            case 0:
                                if(!Board.DeleteDice("Blue"))
                                {
                                    Result = "얼음 주사위를 보낸다고하였으나 보내지않아 주민들이 피해를입었습니다.";
                                    Round.RoundCoin -= 5;
                                    Round.RoundCount = 3;
                                    Round.RoundCountAction += Failed_AvoidFlood;
                                    Result_Info = "3라운드동안 종료시 얻는 금화 -5";
                                }
                                else
                                {
                                    Result = "얼음 주사위가 홍수를 성공적으로 막아 주민들의 지지를 받습니다.";
                                    Round.RoundCoin += 5;
                                    Round.RoundCount = 3;
                                    Round.RoundCountAction += Succeed_AvoidFlood;
                                    Result_Info = "3라운드동안 종료시 얻는 금화 +5";

                                }
                                break;
                            case 1:
                            if(Board.Coin < 5){
                                Result = "부채가 생겼으나 홍수는 성공적으로 막았습니다.";
                                Board.UpCoin(-5);
                                Result_Info = "금화 - 5";
                                }
                                else
                                {
                                    Result = "홍수를 성공적으로 막았습니다.";
                                    Board.UpCoin(-5);
                                    Result_Info = "금화 - 5";
                                }
                                break;
                            case 2:
                                Result = "성공적인 대피명령으로 홍수피해는 적었습니다.";
                                Board.UpCoin(-1);
                                WarBoard.UpMaxUnit(-1);
                                Result_Info = "최대인구수 -1 ,금화 -1";
                                break;
                        }
                    break;
                    case 1: //----------------------------------------------------------------------------------변절자
                        switch (Num)
                        {
                            case 0:
                                Result = "적 유닛이 성에접근하자 아군은 적을 죽였고 전리품으로 금화를 얻었습니다.";
                                int r = Random.Range(2,11);
                                Board.UpCoin(r);
                                Result_Info = $"금화 + {r}";
                                break;
                            case 1:
                                Result = "적 유닛을 부대에 합류시켰습니다.";
                                for(int i =0; i<3;i++)
                                Board.CreateDice(Round.Round / 10);

                                Result_Info = "랜덤 주사위3개획득(레벨은 적주사위의 레벨 유지)";
                                break;
                            case 2:
                                Result = "적 유닛을 포로수용소에 가두었습니다.";
                                Prisoner = true;
                                Result_Info = "아무일도 없었습니다.";
                                break;
                        }
                    break;
                }
            break;
            case "Prisoner":
                switch (EventNum)
                {
                    case 0: //----------------------------------------------------------------------------------탈출
                        switch (Num)
                        {
                            case 0:
                                Result = "포로들 에게 3골드의 현상금을 걸었습니다.";
                                Board.UpCoin(-3);
                                Prisoner = false;
                                Missing_Prisoner = true;
                                Result_Info = "금화 -3";
                                break;
                            case 1:
                                Result = "포로들은 자유를 얻었습니다.";
                                Running_Prisoner = true;
                                Prisoner = false;
                                Result_Info = "좋은일이길 기원합니다.";
                                break;
                            case 2:
                                Result = "병력에게 포로들을 찾아오라 명하였습니다.";
                                WarBoard.UpMaxUnit(-1);
                                Result_Info = "최대인구수 -1";
                                Prisoner = false;
                                Missing_Prisoner = true;
                                break;
                        }
                        break;
                    case 1: //----------------------------------------------------------------------------------포획
                        switch (Num)
                        {
                            case 0:
                                Result = "포로들을 사형집행하였고 전리품을 얻었습니다.";
                                int r = Random.Range(2, 11);
                                Board.UpCoin(r);
                                Missing_Prisoner = false;
                                Prisoner = false;
                                Result_Info = $"금화 + {r}";
                                break;
                            case 1:
                                Result = "강제 노역을 진행 시킵니다.";
                                Round.RoundCoin += 4;
                                Missing_Prisoner = false;
                                Prisoner = true;
                                Result_Info = "종료시 얻는 금화 +4";
                                break;
                            case 2:
                                Result = "포로들을 다시 포로수용소에 가두었습니다.";
                                Missing_Prisoner = false;
                                Prisoner = true;
                                Result_Info = "한숨 돌린것일겁니다.";
                                break;
                        }
                    break;
                    case 2: //----------------------------------------------------------------------------------포획중 사망
                        switch (Num)
                        {
                            case 0:
                                Result = "포로들의 전리품을 받았습니다.";
                                int r = Random.Range(2, 11);
                                Board.UpCoin(r);
                                Missing_Prisoner = false;
                                Prisoner = false;
                                Result_Info = $"금화 + {r}";
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                        }
                    break;
                    case 3: //----------------------------------------------------------------------------------도주
                        switch (Num)
                        {
                            case 0:
                                Result = "잘된 일이길 바랍니다.";
                                Running_Prisoner = true;
                                Missing_Prisoner = false;
                                Result_Info = "좋은 일일 겁니다.";
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                        }
                    break;
                    case 4: //----------------------------------------------------------------------------------약탈
                        switch (Num)
                        {
                            case 0:
                                Result = "주민들은 당신의 위로에 감동을 받았습니다.";
                                Board.UpCoin(-3);
                                WarBoard.UpMaxUnit(1);
                                Missing_Prisoner = false;
                                Prisoner = false;
                                Result_Info = "금화 - 3 , 최대 인구수 +1";
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                        }
                    break;
                    case 5: //----------------------------------------------------------------------------------자유
                        switch (Num)
                        {
                            case 0:
                                Result = "포로들은 자유롭게 일을하는것에 만족해합니다.";
                                Round.RoundCoin += 6;
                                Missing_Prisoner = false;
                                Prisoner = false;
                                Result_Info = "종료시 얻는 금화 + 6";
                                break;
                            case 1:
                                Result = "좋은 병사가 되었습니다.";
                                WarBoard.UpMaxUnit(2);
                                Missing_Prisoner = false;
                                Prisoner = false;
                                Result_Info = "최대 인구수 + 2";
                                break;
                            case 2:
                                break;
                        }
                    break;
                }
            break;
            default:
            
            break;
        }

        EventHandle.SetActive(false);
        EventResultHandle.SetActive(true);
        EventResultName_Text.text = Result;
        EventResultInfo_Text.text = Result_Info;
        ResultCount = 5;
    }

    public void ResultHandler_Disable()
    {
        EventResultHandle.SetActive(false);
        Round.ReadyForRound();
    }

    public void Failed_AvoidFlood()
    {
        Round.RoundCoin += 5;
    }
    public void Succeed_AvoidFlood()
    {
        Round.RoundCoin -= 5;
    }

}
