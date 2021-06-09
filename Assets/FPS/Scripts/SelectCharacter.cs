using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;
using FramedWok.PlayerController;
using UnityEngine.SceneManagement;

public class SelectCharacter : NetworkBehaviour
{
    [SerializeField] private GameObject[] characterList = default;

    private bool hasSelected = false;

    public override void OnStartClient()
    {
        if (isLocalPlayer)
        {
            SceneManager.LoadSceneAsync("LevelTest", LoadSceneMode.Additive);
        }
        CmdSelect();
    }

    [Command(requiresAuthority = true)]
    public void CmdSelect(NetworkConnectionToClient sender = null)
    {
        GameObject characterInstance = Instantiate(characterList[Random.Range(0, characterList.Length)], transform);
        
        NetworkServer.Spawn(characterInstance, sender);

        RpcSetCamera(characterInstance);
    }

    [ClientRpc]
    private void RpcSetCamera(GameObject _characterInstance)
    {
        if (isLocalPlayer)
        {
            _characterInstance.GetComponent<PlayerController>().SetCamera();
        }
    }
}
