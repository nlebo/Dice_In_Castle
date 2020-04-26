using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Test_Server_Manager : MonoBehaviourPunCallbacks
{
    public GameObject Player;
    public Transform[] SpawnPos;

    private void Start() {

        if(PhotonNetwork.IsMasterClient)
        SpawnPlayer();   
    }
    public void SpawnPlayer()
    {
        int PlayerNum = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        PhotonNetwork.Instantiate(Player.name,SpawnPos[PlayerNum].position,Quaternion.identity);
    }
}
