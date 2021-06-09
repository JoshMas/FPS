using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FramedWok.PlayerController;
public class Defender : MonoBehaviour
{
    #region Stats
    public float curHealth = 175;
    public float maxHealth = 175;
    public float movement = 40;
    #endregion

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
    public TMP_Text powerCDText;
    #endregion
    #region Power
    public float pushRadius = 50;
    public float pushForce = 1000;
    private GameObject[] enemyObj;
    private Rigidbody[] rbs;
    #endregion
    #region Passive
    [SerializeField] private float passiveDistance = 10;
    private PlayerController playerController;
    #endregion

    private void Start()
    {
        if(gameObject.tag == "Red Player")
        {
            enemyObj = GameObject.FindGameObjectsWithTag("Blue Player");
            rbs = new Rigidbody[enemyObj.Length];
        }
        if (gameObject.tag == "Blue Player")
        {
            enemyObj = GameObject.FindGameObjectsWithTag("Red Player");
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

        powerCDText.text = powerCD.ToString();


        //passive
        foreach(GameObject enemy in enemyObj)
        {
            if(Vector3.Distance(enemy.transform.position, gameObject.transform.position) < passiveDistance)
            {
               if(enemy.GetComponent<PlayerController>() != null)
                {
                    enemy.GetComponent<PlayerController>().walkSpeed -= 25;
                }
            }
        }

    }

    private void AltFire()
    {
        GameObject grenade = Instantiate(grenadePrefab);

        grenade.tag = gameObject.tag;

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
            if(Vector2.Distance(gameObject.transform.position, rb.transform.position) < pushRadius)
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