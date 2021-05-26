using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Shooter.Player;

namespace Shooter.Abilities
{

    public class Defender : MonoBehaviour
    {
        private PlayerStats player;

 

        #region grenade
        public GameObject grenadePrefab;
        public Transform grenadeSpawn;
        public float grenadeSpeed = 30;
        public float lifeTime = 3;

        #endregion
        #region Cooldowns
        public float altFireCD = 5f;
        private float nextAltFireTime = 0;
        private float nextPowerTime = 0;
        public float powerCD = 20f;

        [SerializeField] private TMP_Text powerCDText;
        [SerializeField] private TMP_Text altCDText;
        #endregion
        #region Power
        public float pushRadius = 50;
        public float pushForce = 1000;
        private GameObject[] enemyObj;
        public Rigidbody[] rbs;
        #endregion

        private void Start()
        {
            grenadePrefab = Resources.Load<GameObject>("WeaponPrefabs/GravGrenade");
            grenadeSpawn = GetWeaponTransform();


            if (player.teamNumber == 0)
            {
                enemyObj = GameObject.FindGameObjectsWithTag("Team 1");
                rbs = new Rigidbody[enemyObj.Length];
            }

            if (player.teamNumber == 1)
            {
                enemyObj = GameObject.FindGameObjectsWithTag("Team 0");
                rbs = new Rigidbody[enemyObj.Length];
            }


            for (int i = 0; i < enemyObj.Length; i++)
            {
                GameObject player = enemyObj[i];
                rbs[i] = player.GetComponent<Rigidbody>();
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

            //These lines are temporarily disabled
            //powerCDText.text = powerCD.ToString();
            //altCDText.text = altFireCD.ToString();


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
            GameObject grenade = Instantiate(grenadePrefab);

            Physics.IgnoreCollision(grenade.GetComponent<Collider>(),
                grenadeSpawn.parent.GetComponent<Collider>());

            grenade.transform.position = grenadeSpawn.position;

            Vector3 rotation = grenade.transform.rotation.eulerAngles;

            grenade.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);

            grenade.GetComponent<Rigidbody>().AddForce(grenadeSpawn.forward * grenadeSpeed, ForceMode.Impulse);

            StartCoroutine(DestroyMissileAfterTime(grenade, lifeTime));
        }

        private void Power()
        {
            foreach (Rigidbody rb in rbs)
            {
                if (Vector2.Distance(gameObject.transform.position, rb.transform.position) < pushRadius)
                {
                    Debug.Log(rb);
                    // calculate direction from target to me
                    Vector3 forceDirection = gameObject.transform.position + rb.transform.position;

                    // apply force on target towards me
                    rb.GetComponent<Rigidbody>().AddForce(forceDirection * pushForce/* * Time.deltaTime*/);
                }





            }
        }

        private IEnumerator DestroyMissileAfterTime(GameObject grenade, float delay)
        {
            yield return new WaitForSeconds(delay);

            Destroy(grenade);
        }
    }
}