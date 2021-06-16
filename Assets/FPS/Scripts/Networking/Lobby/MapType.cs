using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Shooter.Game
{
    public class MapType : MonoBehaviour
    {
        [SerializeField] private int modeType;
        private int maxPlayers;
        private string scene;

        public void SelectScene(int currentMode)
        {
            switch (currentMode)
            {
                case 0:
                    maxPlayers = 2;
                    scene = "1v1";
                    break;
                case 1:
                    maxPlayers = 6;
                    scene = "3v3";
                    break;

            }
        }
        

    }
}

