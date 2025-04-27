using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class PlayerListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform playerListContent;
    [SerializeField] private PlayerListing playerListingPrefab;

    private List<PlayerListing> listings = new List<PlayerListing>();

    public override void OnJoinedRoom()
    {
        GetCurrentRoomPlayers();
    }
    public override void OnLeftRoom()
    {
        for (int i = 0; i < listings.Count; i++)
        {
            Destroy(listings[i].gameObject);
        }
        listings.Clear();
    }

    public void GetCurrentRoomPlayers()
    {
        foreach (KeyValuePair<int,Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
    }

    private void AddPlayerListing(Player player)
    {
        int index = listings.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            listings[index].SetPlayerInfo(player);
        }
        else
        {
            PlayerListing listing = Instantiate(playerListingPrefab, playerListContent);
            if (listing != null)
            {
                listing.SetPlayerInfo(player);
                listings.Add(listing);
            }
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = listings.FindIndex(x => x.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(listings[index].gameObject);
            listings.RemoveAt(index);
        }
    }
}
