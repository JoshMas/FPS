using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField]
    public float rateOfPrimaryFire;

    private float rechamberTime;
    [SerializeField]
    private GameObject secondaryFire;
    [SerializeField]
    private float secondaryCooldown;

    private Transform camTransform;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        camTransform = Camera.main.transform;
        rechamberTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(rechamberTime > 0)
        {
            rechamberTime -= Time.deltaTime;
        }
        if(rechamberTime == 0)
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                PrimaryFire();
               
            }
        }

        
            
    }

    public void PrimaryFire()
    {
       
        Ray bullet = new Ray(camTransform.position, camTransform.TransformDirection(Vector3.forward));
        rechamberTime = rateOfPrimaryFire;
        Debug.Log(Physics.Raycast(bullet));
    }

    public void SecondaryFire()
    {
        
    }
}
