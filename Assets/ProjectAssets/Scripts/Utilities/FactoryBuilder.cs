using UnityEngine;
using Photon.Pun;
using System.IO;

public static class FactoryBuilder
{
    public static GameObject BuilderPlayer(string playerPrefabName, string nickname, Transform spawnTransform)
    {
        GameObject player = null;
        player = PhotonNetwork.Instantiate(Path.Combine("Prefabs/Characters", playerPrefabName), spawnTransform.position, spawnTransform.rotation, 0);

        if (player != null)
        {
            PlayerSetup _PlayerSetup = player.GetComponent<PlayerSetup>();
            player.GetComponent<PhotonView>().Controller.NickName = nickname;

            return player;
        }
        else
        {
            Debug.LogError("Player prefab not found: " + playerPrefabName);
            return null;
        }
    }
}