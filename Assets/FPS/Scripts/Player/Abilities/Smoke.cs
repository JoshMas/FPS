using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    
    public ParticleSystem smoke;

 

    private void OnTriggerEnter(Collider other)
    {
       
        Debug.Log("hit " + other.name + "!");
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        if (!smoke.isPlaying)
        {
            smoke.Play();

        }



    }

 
}
