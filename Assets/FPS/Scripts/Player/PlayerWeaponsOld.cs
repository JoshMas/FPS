using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class PlayerWeaponsOld : NetworkBehaviour
{
    [SerializeField]
    private float rateOfPrimaryFire;
    [SerializeField]
    private GameObject secondaryFire;
    [SerializeField]
    private float secondaryCooldown;

    [SyncVar]
    public float health = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetMouseButtonDown(0))
            PrimaryFire();
        if (Input.GetMouseButtonDown(1))
            SecondaryFire();
    }

    public void PrimaryFire()
    {
        Transform camTransform = Camera.main.transform;
        Ray bullet = new Ray(camTransform.position, camTransform.TransformDirection(Vector3.forward));
        Debug.Log(Physics.Raycast(bullet, out RaycastHit hit));
        if (hit.transform == null)
            return;
        if (hit.transform.CompareTag("Player"))
        {
            PlayerWeaponsOld script = hit.transform.GetComponent<PlayerWeaponsOld>();
            if (hasAuthority)
            {
                DealDamage(script, 10.0f); ;
            }
            Debug.Log(script.health);
        }
    }

    [Command]
    public void DealDamage(PlayerWeaponsOld _target, float _damage)
    {
        _target.LoseHealth(_damage);
    }

    public void LoseHealth(float _damage)
    {
        health -= _damage;
    }

    public void SecondaryFire()
    {
        Debug.Log(health);
    }
}
