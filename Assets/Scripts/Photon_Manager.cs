using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Photon_Manager : MonoBehaviourPunCallbacks
{
    public static Photon_Manager _PHOTON_MANAGER;
    private void Awake()
    {
        //Generamos singelton
        if (_PHOTON_MANAGER != null && _PHOTON_MANAGER != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _PHOTON_MANAGER = this;
            DontDestroyOnLoad(this.gameObject);

            //Realizo conexion
            PhotonConnect();
        }
    }
    public void PhotonConnect()
    {
        //Sincronizo la carga de la sala para todos los jugadores
        PhotonNetwork.AutomaticallySyncScene = true;
        //Conexion al servidor con la configuracion establecida
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Conexion realizada correctamente");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("He implosionado porque: " + cause);
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Accediendo al Lobby");
    }
    public void CreateRoom(string nameRoom)
    {
        PhotonNetwork.CreateRoom(nameRoom, new RoomOptions { MaxPlayers = 2 });
    }
    public void JoinRoom(string nameRoom)
    {
        PhotonNetwork.JoinRoom(nameRoom);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Me he unido a la sala: " + PhotonNetwork.CurrentRoom.Name + " con " + PhotonNetwork.CurrentRoom.PlayerCount + " jugadores conectados en ella.");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("No me he podido conectar a la sala dado el error: " + returnCode + "que significa" + message);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Ingame");
        }
    }
}