using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    private bool x = false;
    public ParticleSystem smoke;

    private void Update()
    {
        if (x)
        {
            smoke.Play();
        }
        else if (!x)
        {
            smoke.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        x = true;
        Debug.Log("hit " + other.name + "!");
        
        
        Destroy(gameObject);
       
    }
}
