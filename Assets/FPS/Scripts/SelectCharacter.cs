using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;
using FramedWok.PlayerController;
using UnityEngine.SceneManagement;

public class SelectCharacter : NetworkBehaviour
{
    [SerializeField] private GameObject[] characterList = default;

    public override void OnStartClient()
    {
        CmdSelect();

        SceneManager.LoadSceneAsync("InGameMenus", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("LevelTest", LoadSceneMode.Additive);
    }

    [Command(requiresAuthority = false)]
    public void CmdSelect(NetworkConnectionToClient sender = null)
    {
        GameObject characterInstance = Instantiate(characterList[Random.Range(0, characterList.Length)], transform.position, Quaternion.identity);

        NetworkServer.Spawn(characterInstance, sender);

        //characterInstance.GetComponent<PlayerController>().Setup();
    }
    
}
