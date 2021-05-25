using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Support : MonoBehaviour
{
    #region Stats
    public float curHealth = 100;
    public float maxHealth = 100;
    public float movement = 40;
    #endregion

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
    #endregion

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
    }

    private IEnumerator DestroyMissileAfterTime(GameObject smoke, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(smoke);
    }
}
