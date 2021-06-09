using System.Collections.Generic;
using UnityEngine;

using Mirror;
using FramedWok.PlayerController;

namespace Shooter.Networking
{
    [AddComponentMenu("")]
    public class FPSNetManager : NetworkManager
    {

        public static FPSNetManager Instance => singleton as FPSNetManager;

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            Transform startPos = GetStartPosition();
            GameObject player = startPos != null
                ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
                : Instantiate(playerPrefab);

            //Replace the zero with a value between 0 and 2, based on whatever method is used to select the character
            player.GetComponent<PlayerController>().CharacterSelect(0);

            NetworkServer.AddPlayerForConnection(conn, player);
        }
        public bool IsHost { get; private set; } = false;
        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);
        }
        public FPSPlayerNet GetPlayerForId(byte _playerId)
        {
            FPSPlayerNet player;
            players.TryGetValue(_playerId, out player);
            return player;
        }
        private Dictionary<byte, FPSPlayerNet> players = new Dictionary<byte, FPSPlayerNet>();
    }
}