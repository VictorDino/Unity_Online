using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Game_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnPlayer1;

    [SerializeField]
    private GameObject spawnPlayer2;

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Player", spawnPlayer1.transform.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("Player2", spawnPlayer2.transform.position, Quaternion.identity);
        }
    }
}
