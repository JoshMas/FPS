using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Player
{
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
        private List<GameObject> teamSpawns = new List<GameObject>();
        [SerializeField]
        private float respawnTimer;

        [Header("UI")]
        [SerializeField]
        private Text healthText;

        public int kills;
        public int deaths;


        public bool dead;

        private void Start()
        {

            

            foreach (GameObject spawn in GameObject.FindGameObjectsWithTag(teamNumber + "Spawn"))
            {
                teamSpawns.Add(spawn);
            }
            
            
            
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
        public void UpdateHealth()
        {
            healthText.text = "+ " + currentHealth;
        }
        IEnumerator Death()
        {
            deaths++;
            gameObject.SetActive(false);
            
            yield return new WaitForSeconds(respawnTimer);

            Transform newSpawn = teamSpawns[Random.Range(0, teamSpawns.Count)].transform;
            gameObject.transform.position = new Vector3(newSpawn.position.x, newSpawn.rotation.y, newSpawn.position.z);

            gameObject.SetActive(true);

            
        }
    }
}

