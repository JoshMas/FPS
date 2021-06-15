using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shooter.Networking;

namespace Shooter.Networking
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerLobby networkManager = null;
        [SerializeField] private GameObject landingPagePanel = null;
        // Start is called before the first frame update
        public void HostLobby()
        {
            networkManager.StartHost();

            landingPagePanel.SetActive(true);
        }
    }
}

