using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shooter.Player.Weapons;
using Shooter.Player;
using FramedWok.PlayerController;
using Mirror;
public class Attacker : NetworkBehaviour
{
    

    #region missile
    public GameObject missilePrefab;
    public Transform missileSpawn;
    public float missileSpeed = 30;
    public float lifeTime = 3;
    #endregion
    #region Cooldowns
    public float altFireCD = 5f;
    private float nextAltFireTime = 0;
    private float nextPowerTime = 0;
    public float powerCD = 20f;
    #endregion
    private PlayerWeapons playerWeapons;
    private PlayerStats playerStats;
    private PlayerController playerController;

    public bool kill = false;

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
                gameObject.GetComponent<PlayerWeapons>().rateOfPrimaryFire -= 5 * Time.deltaTime;
                gameObject.GetComponent<PlayerController>().walkSpeed -= 25 * Time.deltaTime;
                nextPowerTime = Time.time + powerCD;
            }
        }


        if (kill == true)
        {
            altFireCD -= 3f;
        }
    }

    private void AltFire()
    {
        GameObject missile = Instantiate(missilePrefab);
        missile.tag = gameObject.tag;
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
        gameObject.GetComponent<PlayerWeapons>().rateOfPrimaryFire += 5;
        gameObject.GetComponent<PlayerController>().walkSpeed += 25;
        
    }

    private IEnumerator DestroyMissileAfterTime(GameObject missile, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(missile);
    }
}
