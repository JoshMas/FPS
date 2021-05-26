using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shooter.Player;
using FramedWok.PlayerController;
using TMPro;

namespace Shooter.Abilities
{
    public class Support : MonoBehaviour
    {
        private PlayerController controller;
        private PlayerStats player;
        private int team;

        #region grenade
        public GameObject smokePrefab;
        public Transform smokeSpawn;
        public float smokeSpeed = 30;
        public float lifeTime = 3;
        #endregion
        #region Cooldowns
        public float altFireCD = 10f;
        
        private float nextAltFireTime = 0;
        private float nextPowerTime = 0;
        public float powerCD = 30f;

        [SerializeField] private TMP_Text powerCDText;
        [SerializeField] private TMP_Text altCDText;
        #endregion

        private GameObject[] friends;



        private void Start()
        {
            smokePrefab = Resources.Load<GameObject>("WeaponPrefabs/Smoke");
            smokeSpawn = GetWeaponTransform();


            team = gameObject.GetComponent<PlayerStats>().teamNumber;
            if (team == 0)
            {
                friends = GameObject.FindGameObjectsWithTag("Team 0");
            }
            if (team == 1)
            {
                friends = GameObject.FindGameObjectsWithTag("Team 1");
            }
        }
        private void Update()
        {
            
            if (Time.time > nextAltFireTime)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    AltFire();
                    nextAltFireTime = Time.time + altFireCD;
                }
            }

            if (Time.time > nextPowerTime)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Power();
                    nextPowerTime = Time.time + powerCD;
                }
            }

            //Passive?


        }

        private Transform GetWeaponTransform()
        {
            Transform[] transforms = GetComponentsInChildren<Transform>();
            foreach (Transform t in transforms)
            {
                if (t.CompareTag("WeaponTransform"))
                    return t;
            }
            return transform;
        }

        private void AltFire()
        {
            GameObject smoke = Instantiate(smokePrefab);

            Physics.IgnoreCollision(smoke.GetComponent<Collider>(),
                smokeSpawn.parent.GetComponent<Collider>());

            smoke.transform.position = smokeSpawn.position;

            Vector3 rotation = smoke.transform.rotation.eulerAngles;

            smoke.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);

            smoke.GetComponent<Rigidbody>().AddForce(smokeSpawn.forward * smokeSpeed, ForceMode.Impulse);

            StartCoroutine(DestroyMissileAfterTime(smoke, 12));
        }

        private void Power()
        {
            //Heal all team members to full
            foreach(GameObject friend in friends)
            {
                friend.GetComponent<PlayerStats>().currentHealth = friend.GetComponent<PlayerStats>().maxHealth;
                
            }
        }

        private IEnumerator DestroyMissileAfterTime(GameObject smoke, float delay)
        {
            yield return new WaitForSeconds(delay);

            Destroy(smoke);
        }
    }
}