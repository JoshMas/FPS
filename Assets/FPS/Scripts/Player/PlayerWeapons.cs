using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField]
    private float rateOfPrimaryFire;
    [SerializeField]
    private GameObject secondaryFire;
    [SerializeField]
    private float secondaryCooldown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PrimaryFire();
    }

    public void PrimaryFire()
    {
        Transform camTransform = Camera.main.transform;
        Ray bullet = new Ray(camTransform.position, camTransform.TransformDirection(Vector3.forward));
        Debug.Log(Physics.Raycast(bullet)); 
    }

    public void SecondaryFire()
    {

    }
}
