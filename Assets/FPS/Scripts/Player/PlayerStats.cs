using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FramedWok.PlayerController;
using Player.Weapons;


namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerPhysics))]
    public class PlayerStats : MonoBehaviour
    {

        [Header("Stats")]
        [SerializeField]
        public string playerName;
        [SerializeField]
        private int maxHealth;
        [SerializeField]
        public int currentHealth;
        [SerializeField]
        public int teamNumber;
        [SerializeField]
        private List<Spawn> teamSpawns = new List<Spawn>();
        [SerializeField]
        private float respawnTimer;

        private PlayerController controller;
        private PlayerWeapons weapons;

        [Header("UI")]
        [SerializeField]
        private Text healthText;

        public int kills;
        public int deaths;


        public bool dead;

        private void Start()
        {



            foreach (Spawn spawn in FindObjectsOfType<Spawn>()) 
            {
                if(teamNumber == spawn.teamSpawn)
                {
                    teamSpawns.Add(spawn);
                }
                
            }
            UpdateHealth();
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
            gameObject.SetActive(false);
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
            gameObject.SetActive(true);
            dead = false;
        }
    }
}

