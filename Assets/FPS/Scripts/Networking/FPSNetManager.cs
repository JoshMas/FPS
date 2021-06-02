using System.Collections.Generic;
using UnityEngine;

using Mirror;
using FramedWok.PlayerController;

[AddComponentMenu("")]
public class FPSNetManager : NetworkManager
{
    [SerializeField]
    private List<GameObject> playerPrefabs;


    public static FPSNetManager Instance => singleton as FPSNetManager;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefabs[0], startPos.position, startPos.rotation)
            : Instantiate(playerPrefabs[0]);

        

        NetworkServer.AddPlayerForConnection(conn, player);
    }
}
