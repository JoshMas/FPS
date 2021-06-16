using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Smoke : NetworkBehaviour
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
