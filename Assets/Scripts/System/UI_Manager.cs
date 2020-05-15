using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager m_Instance;
    public GameObject Help;
    public GameObject Skip;
    public Text CoinText;
    public Text_Manager UpCoinText;
    public Text CreateText;
    public Text MaxNowUnit;

    public Text RemainPrepareTime;
    public Text RoundText;
    public GameObject DiceInfo;    
    public Image[] SpawnTiles;
    // Start is called before the first frame update
    void Start()
    {
     m_Instance = this;
     //Time.timeScale = 0;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText(Text T,string text)
    {
        T.text = text;
    }

    public void ClickHelp()
    {
        if(Help.activeInHierarchy)
        {
            Help.SetActive(false);
        }
        else
        {
            Help.SetActive(true);
        }
    }


    public void SpawnTile_Add(int Num,Sprite sprite)
    {
        SpawnTiles[Num].sprite = sprite;
        SpawnTiles[Num+1].sprite = sprite;

        SpawnTiles[Num].gameObject .SetActive(true);
        SpawnTiles[Num + 1].gameObject .SetActive(true);

        SpawnTiles[Num + 1].fillAmount = 1;
    }
    public void SpawnTile_Fill(int Num,float value)
    {
        SpawnTiles[Num + 1].fillAmount = value;
    }

    public void ReSort()
    {
        SpawnTiles[0].gameObject.SetActive(false);
        SpawnTiles[1].gameObject.SetActive(false);
        for(int i =2; i<SpawnTiles.Length;i++)
        {
            if(i % 2 == 0 && !SpawnTiles[i].gameObject.activeInHierarchy) break;
            
            SpawnTiles[i].gameObject.SetActive(false);
            SpawnTiles[i-2].sprite = SpawnTiles[i].sprite;
            SpawnTiles[i-2].gameObject.SetActive(true);
        }

        SpawnTiles[0].fillAmount =  0;
    }

}
