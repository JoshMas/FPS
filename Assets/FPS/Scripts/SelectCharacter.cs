using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;
using FramedWok.PlayerController;

public class SelectCharacter : NetworkBehaviour
{
    [SerializeField] private GameObject[] characterList = default;

    public override void OnStartClient()
    {
        base.OnStartClient();
    }
}
