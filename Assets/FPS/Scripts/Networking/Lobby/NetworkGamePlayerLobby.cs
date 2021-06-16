using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
using Shooter.Game;


namespace Shooter.Networking
{
    public class NetworkGamePlayerLobby : NetworkBehaviour
    {
        [SyncVar]
        public string DisplayName = "Awaiting Player...";

     
        private NetworkManagerLobby room;

        private NetworkManagerLobby Room
        {
            get
            {
                if (room != null)
                {
                    return room;
                }
                return room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }
        

        public override void OnStartClient()
        {
            DontDestroyOnLoad(gameObject);
            
            Room.GamePlayers.Add(this);
        }

        public override void OnStopClient()
        {
            Room.GamePlayers.Remove(this);
        }

        [Server]
        public void SetDisplayName(string displayName)
        {
            this.DisplayName = displayName;
        }

    }
}

