using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WYUN;
using WYUN.Deserialization;
using Utf8Json;

public class NetworkManager : MonoBehaviour, WYUN.ILobbyCallback, WYUN.IRoomCallback
{
    [SerializeField]
    PlayerInfo myInfo, encounterInfo;
    [SerializeField]
    DiskCount disks;
    [SerializeField]
    OthelloGame board;
    RoomList rooms;
    int roomIndex;
    Random rnd;
    private void Awake()
    {
        rnd = new Random();
        Core.settings = new AppSettings("wyun_wyun_1:8929", 10008, rnd.Next(1500).ToString());
        Core.AddLobbyCallback(this);
        Core.AddRoomCallback(this);
    }
    public void JoinedLobby()
    {
        //
    }
    public void LeftLobby()
    {
        //
    }
    public void UpdatedLobbyMember(string list)
    {
        //
    }
    public void UpdatedRoomList(string list)
    {
        rooms = JsonSerializer.Deserialize<RoomList>(list);
        if (rooms.rooms.Count > 0)
        {
            roomIndex = 0;
            Core.JoinRoom(rooms.rooms[roomIndex].name);
        }
        else
        {
            Core.CreateAndJoinRoom(rnd.Next(15000).ToString(), 2);
        }
    }
    public void ServerError(string message)
    {
        if (message.Equals("Hitting room limit"))
        {
            if (++roomIndex < rooms.rooms.Count)
            {
                Core.JoinRoom(rooms.rooms[roomIndex].name);
            }
            else
            {
                Core.CreateAndJoinRoom(rnd.Next(15000).ToString(), 2);
            }
        }
        else if (message.Equals("Already exist room with this name"))
        {
            Core.CreateAndJoinRoom(rnd.Next(15000).ToString(), 2);
        }
    }
    public void MessageReceived(string message)
    {
        string[] m = message.Split(',');
        Debug.Log(m[1]);
        if (m[1] == "pass")
        {
            board.state = GameState.EncounterPassed;
        }
        else if (m[1] == "check")
        {
            board.WinLose();
        }
        else
        {
            board.clickedx = int.Parse(m[1]);
            board.clickedy = int.Parse(m[2]);
            board.state = GameState.EncounterClicked;
        }
    }
    public void JoinedRoom()
    {
        myInfo.setName(Core.settings.userName);
    }
    public void LeftRoom()
    {
        //
    }
    public void UpdatedRoomMember(string list)
    {
        MemberList memberList = JsonSerializer.Deserialize<MemberList>(list);
        if (memberList.members.Count == 2)
        {
            Debug.Log(memberList.members[0].name);
            Debug.Log(memberList.members[1].name);
            if (memberList.members[0].name.Equals(Core.settings.userName))
            {
                myInfo.ChangeTurn();
                encounterInfo.setName(memberList.members[1].name);
            }
            else
            {
                encounterInfo.ChangeTurn();
                encounterInfo.setName(memberList.members[0].name);
            }
            board.state = GameState.Uninit;
        }
    }
    public void UpdatedRoomOption(string options)
    {
        //
    }
    // Start is called before the first frame update
    void Start()
    {
        myInfo.setName(Core.settings.userName);
        Core.Connect();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDestroy()
    {
        Debug.Log("NetworkManager.Destroy呼ばれた");
        Core.Exit();
    }
}
