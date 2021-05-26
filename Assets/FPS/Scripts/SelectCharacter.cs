using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;
using FramedWok.PlayerController;

public class SelectCharacter : NetworkBehaviour
{
    [SerializeField] private List<GameObject> characterList;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject playerPrefab = Instantiate(characterList[0]);
        
        //playerPrefab.GetComponent<PlayerController>().Setup();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
