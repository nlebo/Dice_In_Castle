using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Castle_Manager : MonoBehaviourPunCallbacks
{
    public int HP,MaxHP;
    public Image HP_Bar;
    public Room_Manager Room;
    public int Camp;
    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;

        Board_Manager.Initialize += Init;
    }

    public void Hit(int Damage)
    {
        if(!PhotonNetwork.IsMasterClient) return; 

        Room.GetComponent<PhotonView>().RPC("RPCHitCastle",RpcTarget.All,Damage,gameObject.name == "PlayerC" ? 0 : 1);

        if(HP<= 0) Room.GetComponent<PhotonView>().RPC("RPCDeathCastle",RpcTarget.All);
    }

    public void Init()
    {
        HP = MaxHP;
        HP_Bar.fillAmount = 1 - (float)HP/MaxHP;
    }
}
