using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform roomListContent;
    [SerializeField] private RoomListing roomListingPrefab;
    [SerializeField] private PlayerListingsMenu playerListingsMenu;

    private List<RoomListing> listings = new List<RoomListing>();

    public override void OnJoinedRoom()
    {
        roomListContent.DestroyChildren();
        //layerListingsMenu.GetCurrentRoomPlayers();
        listings.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(RoomInfo info in roomList)
        {
            //Removed from rooms list
            if (info.RemovedFromList == true)
            {
                int index = listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if(index != -1)
                {
                    Destroy(listings[index].gameObject);
                    listings.RemoveAt(index);
                }
            }
            //Added to rooms list
            else
            {
                int index = listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if(index == -1)
                {
                    RoomListing listing = Instantiate(roomListingPrefab, roomListContent);
                    if (listing != null)
                    {
                        listing.SetRoomInfo(info);
                        listings.Add(listing);
                    }
                }
                else
                {

                }
            }

        }
    }
}