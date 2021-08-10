using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

//public class PlayerData
//{
//    internal static void Register()
//    {
//        NetworkManager.isRegisterType = PhotonPeer.RegisterType(typeof(PlayerData), 0x89, Serialize, Deserialize);
//        Debug.Log("RegisterType = " + NetworkManager.isRegisterType);
//    }

//    public int actorNumber { get; set; }    // PhotonView id
//    public string sword { get; set; }   // Sword name
//    public bool inArena { get; set; }   // In Trigger arena

//    public PlayerData()
//    {
//        actorNumber = 0;
//        sword = "";
//        inArena = false;
//    }

//    public void SetProperty(PlayerData _player)
//    {
//        this.actorNumber = _player.actorNumber;
//        this.sword = _player.sword;
//        this.inArena = _player.inArena;
//    }

//    public static object Deserialize(byte[] data)
//    {
//        PlayerData result = new PlayerData();
//        using (MemoryStream m = new MemoryStream(data))
//        {
//            using (BinaryReader reader = new BinaryReader(m))
//            {
//                result.actorNumber = reader.ReadInt32();
//                result.sword = reader.ReadString();
//                result.inArena = reader.ReadBoolean();
//            }
//        }
//        return result;
//    }

//    public static byte[] Serialize(object customType)
//    {
//        var c = (PlayerData)customType;
//        using (MemoryStream m = new MemoryStream())
//        {
//            using (BinaryWriter writer = new BinaryWriter(m))
//            {
//                writer.Write(c.actorNumber);
//                writer.Write(c.sword);
//                writer.Write(c.inArena);
//            }
//            return m.ToArray();
//        }
//    }

//}

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance = null;
    public static PhotonView pView
    {
        get
        {
            if (instance == null)
                return null;
            var p_view = instance.GetComponent<PhotonView>();
            if (p_view == null)
                p_view = instance.gameObject.AddComponent<PhotonView>();
            p_view.ViewID = 555;
            return p_view;
        }
    }
    public static int actor
    {
        get
        {
            try
            {
                if (!PhotonNetwork.IsConnected)
                    return 0;
                int _actor = PhotonNetwork.LocalPlayer.ActorNumber;
                return _actor;
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message + "\n" + ex.ToString());
                return 0;
            }
        }
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

    public bool send = false;
    private void Update()
    {
        if (send)
        {
            send = false;
            Debug.Log("Send player data now");
            byte[] bb1 = { 0x22, 0x33 };
            byte[] bb2 = { 0x12, 0x43 };
            pView.RPC("RPC_SyncArray", RpcTarget.All, bb1, bb2);
        }
    }

    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    Debug.Log("Failed to connect to the server: " + cause.ToString(), this);
    //}

    //public override void OnJoinedLobby()
    //{
    //    Debug.Log("Joined into the lobby");
    //}

    //public override void OnCreatedRoom()
    //{
    //    Debug.Log("Created room successfully", this);
    //}

    //public override void OnPlayerEnteredRoom(Player newPlayer)
    //{
    //    Debug.Log("Player entered into the room", this);
    //    byte[] bb1 = {0x22, 0x33 };
    //    byte[] bb2 = { 0x12, 0x43 };
    //    pView.RPC("RPC_SyncArray", RpcTarget.All, bb1, bb2);
    //}

    //public override void OnPlayerLeftRoom(Player otherPlayer)
    //{
    //    Debug.Log("Player left the room", this);
    //}

    //public override void OnJoinedRoom()
    //{
    //    Debug.Log($"Joined into the room, actor number = {actor}", this);
    //}

    [PunRPC]
    public void RPC_SyncArray(byte[] bb1, byte[] bb2)
    {
        Debug.Log(bb1[0] + ":" + bb2[1]);
    }

}