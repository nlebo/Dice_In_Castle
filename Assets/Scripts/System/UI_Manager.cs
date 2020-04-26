using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager m_Instance;
    public GameObject Help;
    public Text CoinText;
    public Text_Manager UpCoinText;
    public Text CreateText;
    // Start is called before the first frame update
    void Start()
    {
     m_Instance = this;
     Time.timeScale = 0;   
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
            Time.timeScale=1;
            Help.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            Help.SetActive(true);
        }
    }
}
