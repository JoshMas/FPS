using System.Collections.Generic;
using UnityEngine;

using Mirror;

[AddComponentMenu("")]
public class FPSNetManager : NetworkManager
{

    public static FPSNetManager Instance => singleton as FPSNetManager;

    //Team spawn points
    public BoxCollider redSpawn;
    public BoxCollider blueSpawn;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
    }
}
