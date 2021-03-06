using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FramedWok.PlayerController;
using Mirror;
using Shooter.Player.Weapons;
using Shooter.Menus;



namespace Shooter.Player
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerPhysics))]
    public class PlayerStats : NetworkBehaviour
    {

        [Header("Stats")]
        [SerializeField]
        public string playerName;
        [SerializeField]
        public int kills;
        private int deaths;
        public bool dead;
        
        [Header("Spawning")]
        [SerializeField]
        public int teamNumber;
        [SerializeField]
        private List<Spawn> teamSpawns = new List<Spawn>();
        [SerializeField]
        private float respawnTimer;

        private bool isSetup = false;
        
        [Header("Health")]
        //[SerializeField]
        public int maxHealth = 100;
        [SyncVar]
        public int currentHealth = 100;
        
        [Header("UI")]
        private GameObject healthTextObj;
        private Text healthText;
        

        [Header("Utility")]
        private PlayerController controller;
        private PlayerWeapons weapons;
       
     
        public void Setup()
        {
            isSetup = true;
        }
        void Start()
        {

            StartCoroutine(GetHUD());
           

            foreach (Spawn spawn in FindObjectsOfType<Spawn>()) 
            {
                if(teamNumber == spawn.teamSpawn)
                {
                    teamSpawns.Add(spawn);
                }
                
            }
            
            controller = GetComponent<PlayerController>();
            weapons = GetComponent<PlayerWeapons>();
            

        }


        private void OnEnable()
        {
            
            currentHealth = maxHealth;
            
        }

        // Update is called once per frame
        void Update()
        {
            if(!isSetup)
            return;

            

            if(currentHealth <= 0)
            {
                StartCoroutine(Death());
            }
        }

        /// <summary>
        /// Whenever the health is called upon, update the UI;
        /// </summary>
        public void UpdateHealth()
        {
            healthText.text = "+ " + currentHealth;
        }
        IEnumerator Death()
        {
            //Player is dead
            dead = true;
            
            deaths++;
            //gameObject.SetActive(false);
            controller.enabled = false;
            weapons.enabled = false;
            yield return new WaitForSeconds(respawnTimer);


            Respawn();
            
        }
        private void Respawn()
        {
            // Move player to designated spawn
            Transform newSpawn = teamSpawns [Random.Range(0, teamSpawns.Count)].transform;
            gameObject.transform.position = new Vector3(newSpawn.position.x, newSpawn.rotation.y, newSpawn.position.z);
            controller.enabled = true;
            weapons.enabled = true;
            //gameObject.SetActive(true);
            
            dead = false;
        }
        public void LoseHealth(int _damage)
        {
            currentHealth -= _damage;
        }

        IEnumerator GetHUD()
        {
            while (healthTextObj == null)
            {
                healthTextObj = GameObject.FindGameObjectWithTag("Health Text");
                

                
                yield return null;
               
            }
            healthText = healthTextObj.GetComponent<Text>();
            Debug.Log(healthTextObj);
            UpdateHealth();
            
            
           
        }

        
    }
}

