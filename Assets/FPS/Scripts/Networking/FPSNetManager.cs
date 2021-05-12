using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class FPSNetManager : NetworkManager
{

    public static FPSNetManager Instance => singleton as FPSNetManager;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
    }
}
