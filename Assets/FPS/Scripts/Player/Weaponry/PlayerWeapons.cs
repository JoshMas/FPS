using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


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
    
    
    private float bloom;

    private Transform camTransform;
    

    // Start is called before the first frame update
    void Start()
    {
        
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
        if(rechamberTime > 0)
        {
            rechamberTime -= Time.deltaTime;
            
        }
        else if(rechamberTime <= 0)
        {
            
            if (Input.GetMouseButton(0))
            {
                PrimaryFire();
               
            }
        }

      
        if(bloom >= bloomMax)
        {
            bloom = bloomMax;
        }

        if (bloom > bloomMin)
        {
            bloom -= bloomReduct *  60 * Time.deltaTime;
        }
       
            
    }

    public void PrimaryFire()
    {
        Vector3 currentBloom = new Vector3(Random.Range(-bloom, bloom), Random.Range(-bloom, bloom), 0);
        
        int layerMask = 1 << 8;
        layerMask  = ~layerMask;
        
        if (bloom < bloomMax)
        {
            bloom += bloomInc * 60*  Time.deltaTime;
            Debug.Log((bloom));
        }
       
        RaycastHit hit;
        if (Physics.Raycast(camTransform.position, camTransform.TransformDirection(Vector3.forward)+ gameObject.transform.TransformDirection(Vector3.forward), out hit, 20f, 3 ))
        {
            
        }
        Debug.DrawLine(camTransform.position, (camTransform.TransformDirection(Vector3.forward) + gameObject.transform.TransformDirection(Vector3.forward) + currentBloom) * 20f, Color.red, 2);
       
        
        rechamberTime = rateOfPrimaryFire;
        Debug.Log(hit);
    }

    public void SecondaryFire()
    {
        
    }
}
