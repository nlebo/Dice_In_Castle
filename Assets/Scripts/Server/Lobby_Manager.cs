using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Lobby_Manager : MonoBehaviourPunCallbacks
{

    private readonly string gameVersion = "1";

    public Text connectionInfo;
    public InputField NickName;

    public Button JoinButton;

    bool Login;
    // Start is called before the first frame update
    void Start()
    {
        Login = false;
        connectionInfo.text = "Disconnected";
    }


    public override void OnConnectedToMaster()
    {
        JoinButton.interactable = true;
        connectionInfo.text = "Online : Connected to Master Server";
        Login = true;
        JoinButton.transform.GetChild(0).GetComponent<Text>().text = "JOIN";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        JoinButton.interactable = false;
        connectionInfo.text = $"Offline : connect Disabled {cause.ToString()} - Try Reconnecting";

        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {

        if (!Login)
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.NickName = NickName.text == "" ? "Test" : NickName.text;
            PhotonNetwork.ConnectUsingSettings();

            JoinButton.interactable = false;
            connectionInfo.text = "Connection To Master Server";
            return;
        }

        if(PhotonNetwork.IsConnected)
        {
            connectionInfo.text = "Connecting to Random Room...";
            PhotonNetwork.JoinRandomRoom();

        }
        else
        {
            connectionInfo.text = "Offline : connect Disabled - Try Reconnecting";

            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfo.text = "There Is no Empty room, Hosting new Room";
        PhotonNetwork.CreateRoom(null,new RoomOptions{MaxPlayers = 2});
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2) PhotonNetwork.LoadLevel("Main");
    }

    public override void OnJoinedRoom()
    {
        connectionInfo.text = $"{PhotonNetwork.NickName} : Connected With room";
        
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2) PhotonNetwork.LoadLevel("Main");
    }


}
