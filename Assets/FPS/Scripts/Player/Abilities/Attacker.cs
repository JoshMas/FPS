using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shooter.Player;
using FramedWok.PlayerController;
using TMPro;

namespace Shooter.Abilities
{
    public class Attacker : MonoBehaviour
    {
        private PlayerStats player;
        private PlayerController controller;
        private int team;
        

        #region missile
        public GameObject missilePrefab;
        public Transform missileSpawn;
        public float missileSpeed = 30;
        public float lifeTime = 3;
        public Transform target;
        #endregion
        #region Cooldowns
        public float altFireCD = 5f;
        private float nextAltFireTime = 0;
        private float nextPowerTime = 0;
        public float powerCD = 20f;

        [SerializeField] private TMP_Text powerCDText;
        [SerializeField] private TMP_Text altCDText;
        #endregion

        public bool kill = false;
        private void Start()
        {
            missilePrefab = Resources.Load<GameObject>("WeaponPrefabs/Missile");
            missileSpawn = GetWeaponTransform();


            team = gameObject.GetComponent<PlayerStats>().teamNumber;
            if(team == 0)
            {
                target = GameObject.FindGameObjectWithTag("Team 1").transform;
            }
            if (team == 1)
            {
                target = GameObject.FindGameObjectWithTag("Team 0").transform;
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
                    //player.currentHealth -= 50 * Time.deltaTime;
                    //movement -= 25 * Time.deltaTime;
                    nextPowerTime = Time.time + powerCD;
                }
            }


            if (kill == true)
            {
                altFireCD -= 3f;
                kill = false;
            }
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
            GameObject missile = Instantiate(missilePrefab);

            Physics.IgnoreCollision(missile.GetComponent<Collider>(),
                missileSpawn.parent.GetComponent<Collider>());

            missile.transform.position = missileSpawn.position;

            Vector3 rotation = missile.transform.rotation.eulerAngles;

            missile.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);

            missile.GetComponent<Rigidbody>().AddForce(missileSpawn.forward * missileSpeed, ForceMode.Impulse);

            StartCoroutine(DestroyMissileAfterTime(missile, lifeTime));
        }

        private void Power()
        {
            player.currentHealth += 50;
            controller.walkSpeed += 25;
            new WaitForSeconds(10);
            player.currentHealth -= 50;
            controller.walkSpeed -= 25;
            // movement += 25;

        }

        private IEnumerator DestroyMissileAfterTime(GameObject missile, float delay)
        {
            yield return new WaitForSeconds(delay);

            Destroy(missile);
        }
    }
}
