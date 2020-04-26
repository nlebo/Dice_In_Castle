using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Select_Dice : MonoBehaviourPun
{
    Board_Manager Board;
    public List<int> Choose;
    public List<Dice_Manager> Dices;
    public Toggle[] T;
    public GameObject Select_Panel;
    public Button Ready_Button;
    

    public int ready;
    // Start is called before the first frame update
    void Start()
    {

        Select_Panel = GameObject.Find("Select_Panel");
        Ready_Button = Select_Panel.transform.GetChild(Select_Panel.transform.childCount - 1).GetComponent<Button>();
        Ready_Button.onClick.AddListener(Ready);
        for(int i =0; i<T.Length;i++)
        {
            T[i] = Select_Panel.transform.GetChild(i).GetChild(1).GetComponent<Toggle>();
        }

        Board = Board_Manager.m_Instance;
        Choose.Add(0);
        Choose.Add(1);
        Choose.Add(2);
        Choose.Add(3);
        Choose.Add(4);

        ready = 0;
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
    public void Ready()
    {
        if(Choose.Count < 5) return;
        
        for(int i =0; i<5;i++)
        {
            Board.Dices[i] = Dices[Choose[i]];
            
            //Time.timeScale = 1;
        }

        Ready_Button.interactable = false;
        GetComponent<PhotonView>().RPC("RPCCompleteReady",RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void RPCCompleteReady()
    {
        Debug.Log("Call RPC");
        ready++;

        if(ready>=2){
         Time.timeScale = 1;
         Select_Panel.SetActive(false);
         Board.Stop = false;
        }
    }
}
