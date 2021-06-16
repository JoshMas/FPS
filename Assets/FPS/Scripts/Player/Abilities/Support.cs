using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shooter.Player;
using Mirror;

public class Support : NetworkBehaviour
{
    

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
    private float nextPassiveHealTime = 0;
    public float passiveCD = 6f;
    #endregion
    private GameObject[] sameTeam;
    private PlayerStats playerStats;

    private void Start()
    {
        if (gameObject.tag == "Red Player")
        {
            sameTeam = GameObject.FindGameObjectsWithTag("Red Player");
            
        }
        if (gameObject.tag == "Blue Player")
        {
            sameTeam = GameObject.FindGameObjectsWithTag("Blue Player");
            
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

        if(Time.time > nextPassiveHealTime)
        {
            if(gameObject.GetComponent<PlayerStats>().currentHealth < playerStats.maxHealth)
            Passive();
            nextPassiveHealTime = Time.time + passiveCD;
        }


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
        foreach(GameObject obj in sameTeam)
        {
            obj.GetComponent<PlayerStats>().currentHealth = playerStats.maxHealth; 
        }
    }

    private void Passive()
    {
        gameObject.GetComponent<PlayerStats>().currentHealth += 3;
    }

    private IEnumerator DestroyMissileAfterTime(GameObject smoke, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(smoke);
    }
}
