using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Select_Dice : MonoBehaviour
{
    Board_Manager Board;
    public List<int> Choose;
    public List<Dice_Manager> Dices;
    public Toggle[] T;
    public GameObject Select_Panel;
    // Start is called before the first frame update
    void Start()
    {
        Board = Board_Manager.m_Instance;
        Choose.Add(0);
        Choose.Add(1);
        Choose.Add(2);
        Choose.Add(3);
        Choose.Add(4);

        Board_Manager.Initialize += Init;
    }

    public void InAndOut(int i)
    {
        if(!Select_Panel.activeInHierarchy) return;

        if(Choose.Contains(i)){ Choose.Remove(i); T[i].isOn = false;}
        else if(Choose.Count < 5){ Choose.Add(i); T[i].isOn = true;}
        else{T[i].isOn = false;}
    }

    public void Play()
    {
        if(Choose.Count < 5) return;
        
        for(int i =0; i<5;i++)
        {
            Board.Dices[i] = Dices[Choose[i]];
            Select_Panel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Init()
    {
        Choose.Clear();
        Choose.Add(0);
        Choose.Add(1);
        Choose.Add(2);
        Choose.Add(3);
        Choose.Add(4);

        T[0].isOn = true;
        T[1].isOn = true;
        T[2].isOn = true;
        T[3].isOn = true;
        T[4].isOn = true;
        T[5].isOn = false;

        Time.timeScale = 0;
        Select_Panel.SetActive(true);
    }
}
