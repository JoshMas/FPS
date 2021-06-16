using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravGrenade : MonoBehaviour
{
    public float pullRadius = 100;
    public float pullForce = 1000;
    private GameObject[] otherTeam;
    
    
   
    private Rigidbody[] rbs;
    
    

    private void Start()
    {
       if(gameObject.tag == "Red Player")
        {
            otherTeam = GameObject.FindGameObjectsWithTag("Blue Player");
        }
        else if (gameObject.tag == "Blue Player")
        {
            otherTeam = GameObject.FindGameObjectsWithTag("Red Player");
        }
        else
        {
            otherTeam = new GameObject[0];
        }

        //blueObj = GameObject.FindGameObjectsWithTag("Blue Player");
            //redObj = GameObject.FindGameObjectsWithTag("Red Player");
            //allObj = GameObject.FindGameObjectsWithTag(tag);
        
       
       
        
        rbs = new Rigidbody[otherTeam.Length];
      

        for (int i = 0; i < otherTeam.Length; i++)
        {
            GameObject otherPlayer = otherTeam[i];
            //GameObject redPlayer = redObj[i];
            rbs[i] = otherPlayer.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit " + other.name + "!");
        foreach (Rigidbody rb in rbs)
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
