using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter.Menus
{
    public class UI : MonoBehaviour
    {
        public static bool isPaused;
        public GameObject pausePanel;
        public GameObject optionsPanel;
        
        [SerializeField]
        public GameObject gameplayUI;


        // Start is called before the first frame update
        void Start()
        {

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isPaused = false;
            gameplayUI.SetActive(true);

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseMenuActive();
            }
        }
        public void PauseMenuActive()
        {
            isPaused = !isPaused;
            Debug.Log(isPaused);
            if (isPaused)
            {
                gameplayUI.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pausePanel.SetActive(true);
            }
            else
            {
                gameplayUI.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pausePanel.SetActive(false);
            }
        }

      


    }

}
