using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Shooter.Network
{
    public class PlayerNameCreate : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_InputField nameInputField = null;
        [SerializeField] private Button continueButton = null;

        public static string DisplayName { get; private set; }

        private const string PlayerNameKey = "PlayerName";


        private void Start()
       

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}