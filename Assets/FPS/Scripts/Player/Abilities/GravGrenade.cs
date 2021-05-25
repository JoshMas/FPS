using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravGrenade : MonoBehaviour
{
    public float pullRadius = 100;
    public float pullForce = 1000;
    private GameObject[] moveableObj;
    private Rigidbody[] rbs;
    

    private void Start()
    {
        moveableObj = GameObject.FindGameObjectsWithTag("Player");
        rbs = new Rigidbody[moveableObj.Length];

        for(int i = 0; i < moveableObj.Length; i++)
        {
            GameObject player = moveableObj[i];
            rbs[i] = player.GetComponent<Rigidbody>();
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
                rb.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * pullForce * Time.deltaTime);
            }
           


        }
        Destroy(gameObject);
    }
}
