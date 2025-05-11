using UnityEngine;
using Photon.Pun;

public class Sync : MonoBehaviour
{
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
}