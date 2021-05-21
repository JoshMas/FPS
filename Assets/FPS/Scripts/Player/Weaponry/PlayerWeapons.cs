using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scoring;


namespace Player.Weapons
{
    public class PlayerWeapons : MonoBehaviour
    {
        [SerializeField] public float rateOfPrimaryFire;

        private float rechamberTime;
        [SerializeField] private GameObject secondaryFire;
        [SerializeField] private float secondaryCooldown;

        [SerializeField] private float bloomInc;
        [SerializeField] private float bloomMin;
        [SerializeField] private float bloomMax;
        [SerializeField] private float bloomReduct;

        [SerializeField] private float range;
        [SerializeField] private int damage;

        [SerializeField] private Image crosshair;


        GameManager GM;
        PlayerStats thisPlayer;



        private float bloom;

        private Transform camTransform;


        // Start is called before the first frame update
        void Start()
        {
            thisPlayer = gameObject.GetComponent<PlayerStats>();
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        private void OnEnable()
        {
            camTransform = Camera.main.transform;
            rechamberTime = 0;
            bloom = bloomMin;

        }

        // Update is called once per frame
        void Update()
        {
            if (rechamberTime > 0)
            {
                rechamberTime -= Time.deltaTime;

            }
            else if (rechamberTime <= 0)
            {

                if (Input.GetMouseButton(0))
                {
                    PrimaryFire();

                }
            }


            if (bloom >= bloomMax)
            {
                bloom = bloomMax;
            }

            if (bloom > bloomMin)
            {
                bloom -= bloomReduct * 60 * Time.deltaTime;
            }


        }

        public void PrimaryFire()
        {
            Vector3 currentBloom = new Vector3(Random.Range(-bloom, bloom), Random.Range(-bloom, bloom), 0);

            int layerMask = 1 << 8;
            layerMask = ~layerMask;

            if (bloom < bloomMax)
            {
                bloom += bloomInc * 60 * Time.deltaTime;
                Debug.Log((bloom));
            }

            RaycastHit hit;
            if (Physics.Raycast(camTransform.position, (camTransform.TransformDirection(Vector3.forward) + gameObject.transform.TransformDirection(Vector3.forward) + currentBloom), out hit, range))
            {
                PlayerStats enemyStats = hit.collider.GetComponent<PlayerStats>();

                if (enemyStats != null)
                {
                    enemyStats.currentHealth -= damage;
                    enemyStats.UpdateHealth();
                }

                if(enemyStats.currentHealth <= 0)
                {
                    thisPlayer.kills++;
                    GM.IncreaseScore(thisPlayer.teamNumber);
                }

            }
            Debug.DrawLine(camTransform.position, (camTransform.TransformDirection(Vector3.forward) + gameObject.transform.TransformDirection(Vector3.forward) + currentBloom) * 20f, Color.red, 2);


            rechamberTime = rateOfPrimaryFire;
            Debug.Log(hit);
        }

        public void SecondaryFire()
        {

        }
    }

}
