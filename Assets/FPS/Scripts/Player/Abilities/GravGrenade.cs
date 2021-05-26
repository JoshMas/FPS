using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shooter.Abilities;
public class GravGrenade : MonoBehaviour
{
   


    public float pullRadius = 100;
    public float pullForce = 1000;
    


    private Defender defender;

  

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit " + other.name + "!");
        foreach (Rigidbody rb in defender.rbs)
        {
            if(Vector2.Distance(gameObject.transform.position, rb.transform.position) < pullRadius)
            {
                Debug.Log(rb);
                // calculate direction from target to me
                Vector3 forceDirection = gameObject.transform.position - rb.transform.position;

                // apply force on target towards me
                rb.GetComponent<Rigidbody>().AddForce(forceDirection * pullForce/* * Time.deltaTime*/);
            }
           


        }
        Destroy(gameObject);
    }
}
