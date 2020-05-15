using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DicesInfo : MonoBehaviour
{
    public List<string> Dices_Info;
    public List<string> DicesName;
    public List<Sprite> DiceSprite;

    public static DicesInfo m_Instance;
    // Start is called before the first frame update
    void Awake()
    {
        Dices_Info = new List<string>();
        DicesName = new List<string>();

        AddDicesInfo("화이트","공속이 빠르다 \n 체력 : 10 / 데미지 : 1 / 공속 : 0.5");
        AddDicesInfo("검정이","공격시 랜덤추가데미지(1 ~ 5) \n 체력 : 15 / 데미지 : 1 / 공속 : 1.5");
        AddDicesInfo("빨강이","공격시 범위데미지 \n 체력 : 20 / 데미지 : 1 / 공속 : 1");
        AddDicesInfo("화살주사위","원거리 공격 \n 체력 : 5 / 데미지 : 3 / 공속 : 2");
        AddDicesInfo("파랑이","지정된 범위 느려짐 효과 \n 이속 및 공속 저하(20% + (4% * 레벨)");
        AddDicesInfo("노랑이","라운드 종료시 보너스 금화 흭득 \n 4 + ( 4 * 레벨) 금화 흭득");

        m_Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddDicesInfo(string name,string info)
    {
        Dices_Info.Add(info);
        DicesName.Add(name);
    }
}
