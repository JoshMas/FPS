using UnityEngine;
using Mirror;
using Shooter.UI;
using Shooter.Player;
using UnityEngine.SceneManagement;
using System.Collections;
using Shooter.Networking;

namespace Shooter.Networking
{
    [RequireComponent(typeof(PlayerStats))]
    public class FPSPlayerNet : NetworkBehaviour
    {
        [SyncVar]
        public byte playerId;
        [SyncVar]
        public string username = "";

        public bool ready;
        private Lobby lobby;
        private bool hasJoinedLobby = false;

        void Start()
        {
            // If we are the localplayer setup the movement script
            if (isLocalPlayer)
            {
                PlayerStats playerMotor = gameObject.GetComponent<PlayerStats>();
                playerMotor.Setup();
            }
           
        }
        private void Update()
        {
            if(FPSNetManager.Instance.IsHost)
            {
                if (lobby == null && !hasJoinedLobby)
                    lobby = FindObjectOfType<Lobby>();

                if (lobby != null && !hasJoinedLobby)
                {
                    hasJoinedLobby = true;
                    lobby.OnPlayerConnected(this);
                }
            }
           
                
        }
        public void SetUsername(string _name)
        {
            if (isLocalPlayer)
            {
                // Only localplayers can call commands as localplayers are the only ones who have the authority to talk to the server
                CmdSetUsername(_name);
            }
        }

        public void AssignPlayerToSlot(bool _left, int _slotId, byte _playerId)
        {
            if (isLocalPlayer)
            {
                CmdAssignPlayerToLobbySlot(_left, _slotId, _playerId);
            }
        }

        #region Commands
        [Command]
        public void CmdSetUsername(string _name) => username = _name;
        [Command]
        public void CmdAssignPlayerToLobbySlot(bool _left, int _slotId, byte _playerId) => RpcAssignPlayerToLobbySlot(_left, _slotId, _playerId);
        #endregion
        #region RPCs
        [ClientRpc]
        public void RpcAssignPlayerToLobbySlot(bool _left, int _slotId, byte _playerId)
        {
            if (FPSNetManager.Instance.IsHost)
                return;

            StartCoroutine(AssignPlayerToLobbySlotDelayed(FPSNetManager.Instance.GetPlayerForId(_playerId), _left, _slotId));
        }
        #endregion
        #region Couroutines
        private IEnumerator AssignPlayerToLobbySlotDelayed(FPSPlayerNet _player, bool _left, int _slotId)
        {
            Lobby lobby = FindObjectOfType<Lobby>();
            while(lobby == null)
            {
                yield return null;
                lobby = FindObjectOfType<Lobby>();
            }
            lobby.AssignPlayerToSlot(_player, _left, _slotId);
        }
        #endregion
        public override void OnStartClient()
        {
            FPSNetManager.Instance.OnServerAddPlayer(connectionToServer);
        }
        public override void OnStartLocalPlayer()
        {
            SceneManager.LoadSceneAsync("InGameMenus", LoadSceneMode.Additive);
        }
        public override void OnStopClient()
        {
            //Remove the playerId from the server
            FPSNetManager.Instance.OnServerDisconnect(connectionToServer);
        }
    }

}
