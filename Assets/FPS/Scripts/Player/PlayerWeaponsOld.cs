using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsOld : MonoBehaviour
{
    [SerializeField]
    private float rateOfPrimaryFire;
    [SerializeField]
    private GameObject secondaryFire;
    [SerializeField]
    private float secondaryCooldown;

    private float health = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PrimaryFire();
        if (Input.GetMouseButtonDown(1))
            SecondaryFire();
    }

    public void PrimaryFire()
    {
        Transform camTransform = Camera.main.transform;
        Ray bullet = new Ray(camTransform.position, camTransform.TransformDirection(Vector3.forward));
        RaycastHit hit = new RaycastHit();
        Debug.Log(Physics.Raycast(bullet, out hit));
        if(hit.transform.CompareTag("Player"))
            Debug.Log(hit.transform.GetComponent<PlayerWeaponsOld>().LoseHealth());
    }

    public float LoseHealth()
    {
        health -= 10.0f;
        return health;
    }

    public void SecondaryFire()
    {
        Debug.Log(health);
    }
}
