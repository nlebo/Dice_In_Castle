using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Board_Manager : MonoBehaviour
{
    public static Board_Manager m_Instance;
    public static UnityAction Initialize;
    public Transform[] SpawnPos;
    public Dice_Manager[] Dices;
    public int[] Grids;
    public int Count = 0;
    int Coin;

    [SerializeField]
    private int CreateCoin;

    UI_Manager UI;
    float nTime;
    int Click_Count;

    public bool Stop;

    // Start is called before the first frame update
    void Start()
    {
        Grids = new int[SpawnPos.Length];
        Grids.Initialize();
        m_Instance = this;
        UI = UI_Manager.m_Instance;
        Initialize += Init;
        Stop =true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Stop) return;
        nTime += Time.deltaTime;
        if(nTime>= 1){ 
            UpCoin(1,0);
            nTime =0;
        }
    }

    public void CreateDice()
    {
        if(Count >= SpawnPos.Length || Coin < CreateCoin) return;
        
        int where = Random.Range(0,Grids.Length);
        while(Grids[where] != 0)
        {
            where = where + 1 >= Grids.Length ? 0 : where + 1;
        }

        int what = Random.Range(0,Dices.Length);
        Instantiate(Dices[what].gameObject,SpawnPos[where]).GetComponent<Dice_Manager>().where = where;
        Grids[where] = 1;

        Coin -=CreateCoin;
        Click_Count ++;
        CreateCoin += Click_Count / 2;

        UI.UpdateText(UI.CreateText,"소환(" + CreateCoin.ToString() + ")");
        UI.UpdateText(UI.CoinText,Coin.ToString());

        Count++;

        return;
    }
    public void CreateDice(int Wh,int Lv)
    {
        DeleteDice(Wh);

        int what = Random.Range(0,Dices.Length);
        Dice_Manager dice = Instantiate(Dices[what].gameObject,SpawnPos[Wh]).GetComponent<Dice_Manager>();
        dice.where = Wh;
        dice.Level = Lv;
        dice.GetComponent<SpriteRenderer>().sprite = dice.scales[Lv];
        
        Grids[Wh] = 1;

        Count++;

        return;
    }
    

    public bool DeleteDice(int where)
    {
        if(Grids[where] == 0) return false;

        Grids[where] = 0;
        Count--;
        Destroy(SpawnPos[where].GetChild(0).gameObject);

        return true;
    }

    public void UpCoin(int up){
        Coin += up;
        UI.UpdateText(UI.CoinText,Coin.ToString());

        Text_Manager Tex = Instantiate(UI_Manager.m_Instance.UpCoinText.gameObject,UI_Manager.m_Instance.CoinText.transform).GetComponent<Text_Manager>();

        Tex.TEXT("+" + up.ToString());
        Tex.Up_Text(0.5f,0.5f);
    }

    public void UpCoin(int up,int flag){
        Coin += up;
        UI.UpdateText(UI.CoinText,Coin.ToString());

    }
    public void Init()
    {
        Coin = 0;
        CreateCoin = 4;
         UI.UpdateText(UI.CreateText,"소환(" + CreateCoin.ToString() + ")");
        UpCoin(0,0);
        nTime = 0;
        Click_Count = 0;

        for(int i =0;i<Grids.Length;i++)
            DeleteDice(i);
        
    }
}
