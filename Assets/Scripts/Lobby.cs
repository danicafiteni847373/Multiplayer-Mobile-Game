using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class Lobby : MonoBehaviourPunCallbacks
{
    [Tooltip("Content Object")]
    public GameObject ScrollViewContent;

    [Tooltip("UI Row Prefab containing the room details ")]
    public GameObject RowRoom;

    [Tooltip("Player Name")]
    public GameObject InputPlayerName;

    [Tooltip("Room Name")]
    public GameObject InputRoomName;

    [Tooltip("Status Message")]
    public GameObject Status;

    [Tooltip("Button Create Room")]
    public GameObject BtnCreateRoom;

    [Tooltip("Panel Lobby")]
    public GameObject PanelLobby;

    [Tooltip("Panel Waiting for Players")]
    public GameObject PanelWaitingForPlayers;
        
    List<RoomInfo> availableRooms = new List<RoomInfo>();
    bool joiningRoom = false;

    UnityEngine.Events.UnityAction buttonCallback;

    private void Start()
    {

        //This makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {
            //Set the App version before connecting
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = "1.0";
            // Connect to the photon master-server. We use the settings saved in PhotonServerSettings (a .asset file in this project)
            PhotonNetwork.ConnectUsingSettings();
        }

        InputRoomName.GetComponent<TMP_InputField>().text = "Room1";
        InputPlayerName.GetComponent<TMP_InputField>().text = "Player 1";
    }






    public override void OnDisconnected(DisconnectCause cause)
    {
        print("OnFailedToConnectToPhoton. StatusCode: " + cause.ToString() + " ServerAddress: " + PhotonNetwork.ServerAddress);
    }

    public override void OnConnectedToMaster()
    {
        print("OnConnectedToMaster");
        //After we connected to Master server, join the Lobby
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print(roomList.Count);
        //After this callback, update the room list
        availableRooms = roomList;
        UpdateRoomList();
        if(roomList.Count>0) InputPlayerName.GetComponent<TMP_InputField>().text = "Player 2";
       
    }

    private void UpdateRoomList()
    {
        foreach(RoomInfo roomInfo in availableRooms)
        {
            GameObject rowRoom = GameObject.Instantiate(RowRoom) as GameObject;
            rowRoom.transform.parent = ScrollViewContent.transform;
            rowRoom.transform.localScale = Vector3.one;

            rowRoom.transform.Find("RoomName").GetComponent<TextMeshProUGUI>().text = roomInfo.Name;
            rowRoom.transform.Find("RoomPlayers").GetComponent<TextMeshProUGUI>().text = roomInfo.PlayerCount.ToString();
            buttonCallback = () => this.OnClickJoinRoom(roomInfo.Name);
            rowRoom.transform.Find("BtnJoin").GetComponent<Button>().onClick.AddListener(buttonCallback);
        }
    }

    public void OnClickJoinRoom(string roomName)
    {
        joiningRoom = true;
        //Set our Player name
        PhotonNetwork.NickName = InputPlayerName.GetComponent<TMP_InputField>().text;

        //Join the Room
        PhotonNetwork.JoinRoom(roomName);
    }


    private void OnGUI()
    {
        Status.GetComponent<TextMeshProUGUI>().text = "Status:" + PhotonNetwork.NetworkClientState.ToString();

        if (joiningRoom || !PhotonNetwork.IsConnected || PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
        {
            BtnCreateRoom.SetActive(false);
        }
        else
        {
            BtnCreateRoom.SetActive(true);
        }
    }

    public void CreateRoom()
    {
        joiningRoom = true;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = (byte)2; //Set any number

        PhotonNetwork.JoinOrCreateRoom(InputRoomName.GetComponent<TMP_InputField>().text, roomOptions, TypedLobby.Default);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("Cannot create room:" + message);
        joiningRoom = false;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("Cannot join room:" + message);
        joiningRoom = false;
    }
        
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Cannot join random room:"+message);
        joiningRoom = false;
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        //Set our player name
        PhotonNetwork.NickName = InputPlayerName.GetComponent<TMP_InputField>().text;        
    }

    private void LoadMainGame()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            //Load the Scene called GameLevel (Make sure it's added to build settings)
            PhotonNetwork.LoadLevel("MainGame");
        }
 
    }

    public override void OnJoinedRoom()
    {
        PanelLobby.SetActive(false);
        PanelWaitingForPlayers.SetActive(true);
        print("OnJoinedRoom");
        

    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {        
        base.OnPlayerEnteredRoom(newPlayer);
        LoadMainGame();
    }

    public void Refresh()
    {
        if (PhotonNetwork.IsConnected)
        {
            //Re-join Lobby to get the latest Room list
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }
        else
        {
            //We are not connected, estabilish a new connection
            PhotonNetwork.ConnectUsingSettings();
        }
    }



}
