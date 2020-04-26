using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Player_Controller : MonoBehaviourPun
{
    private void Update() {
        if(!photonView.IsMine) return;
        
        transform.Translate(transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * 3);


        if(Input.GetKeyDown(KeyCode.Space))
        Shout();

    }

    public void Shout()
    {
        if(!PhotonNetwork.IsMasterClient) return;

        photonView.RPC("RPCShout",RpcTarget.All,PhotonNetwork.LocalPlayer.ActorNumber);
    }

    [PunRPC]
    public void RPCShout(int i){
        Debug.Log(i);
    }
}
