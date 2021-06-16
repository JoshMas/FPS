using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Shooter.Score;


using Mirror;


namespace Shooter.Player.Weapons
{
    public class PlayerWeapons : NetworkBehaviour
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

        [SerializeField] private ParticleSystem gunEffect;
        private LineRenderer gunTrail;
        private Vector3[] defaultTrailPos;

        Scoring GM;
        PlayerStats thisPlayer;



        private float bloom;

        private Transform camTransform;


        // Start is called before the first frame update
        void Start()
        {
            thisPlayer = gameObject.GetComponent<PlayerStats>();
            GM = FindObjectOfType<Scoring>();
            gunTrail = GetComponent<LineRenderer>();
            defaultTrailPos = new Vector3[2];
            defaultTrailPos[0] = Vector3.up * 1000;
            defaultTrailPos[1] = Vector3.up * 1000;
            gunTrail.SetPositions(defaultTrailPos);
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
            if (!hasAuthority)
                return;

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
                if (Input.GetMouseButtonUp(1))
                {
                    CmdResetTrail();
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

            if (Physics.Raycast(camTransform.position, (camTransform.TransformDirection(Vector3.forward) + currentBloom), out RaycastHit hit, range))
            {
                CmdGunEffect(hit.point);

                if (gameObject.CompareTag("Player"))
                {
                    PlayerStats enemyStats = hit.collider.GetComponent<PlayerStats>();

                    if (enemyStats != null)
                    {
                        if (hasAuthority)
                        {
                            DealDamage(enemyStats, damage); ;
                            enemyStats.UpdateHealth();

                        }
                        Debug.Log(enemyStats.currentHealth);
                        if (enemyStats.currentHealth <= 0)
                        {
                            thisPlayer.kills++;
                            GM.IncreaseScore(thisPlayer.teamNumber);
                        }
                    }






                }


            }
            Debug.DrawLine(camTransform.position, (camTransform.TransformDirection(Vector3.forward) + currentBloom) * 20f, Color.red, 2);


            rechamberTime = rateOfPrimaryFire;
            Debug.Log(hit);
        }

        [Command]
        public void DealDamage(PlayerStats _target, int _damage)
        {
            _target.LoseHealth(_damage);
        }

        [Command]
        private void CmdGunEffect(Vector3 hit)
        {
            RpcGunEffect(hit);
        }

        [ClientRpc]
        private void RpcGunEffect(Vector3 hit)
        {
            gunTrail.SetPosition(0, transform.position);
            gunTrail.SetPosition(1, hit);
            Instantiate(gunEffect.gameObject, hit, Quaternion.identity);
        }

        [Command]
        private void CmdResetTrail()
        {
            RpcResetTrail();
        }

        [ClientRpc]
        private void RpcResetTrail()
        {
            gunTrail.SetPositions(defaultTrailPos);
        }
    }
}
