using System.Collections.Generic;
using UnityEngine;

using Mirror;
using FramedWok.PlayerController;

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
        //player.GetComponent<PlayerController>().CharacterSelect(0);

        NetworkServer.AddPlayerForConnection(conn, player);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
    }
}
