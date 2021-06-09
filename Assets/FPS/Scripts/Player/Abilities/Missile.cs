using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 1f;

    private Transform[] target;
    private GameObject[] otherTeam;
    Transform tMin = null;


    private void Start()
    {
        if (gameObject.tag == "Red Player")
        {
            
            otherTeam = GameObject.FindGameObjectsWithTag("Blue Player");
            target = new Transform[otherTeam.Length];
        }
        if (gameObject.tag == "Blue Player")
        {
            otherTeam = GameObject.FindGameObjectsWithTag("Red Player");
            target = new Transform[otherTeam.Length];
        }
       
        
    }

    void Update()
    {

        FindClosest(target);
        transform.position = Vector3.MoveTowards(transform.position, tMin.position, speed * Time.deltaTime);
        Destroy(this.gameObject, 5);
    }

  

    /*IEnumerator OnHitSlow()
    {
        Time.timeScale = 0.4f;
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1f;
    }
    */
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit " + other.name + "!");

        Destroy(gameObject);
    }

   
    private Transform FindClosest(Transform[] enemies)
    {
        
        float distanceToClosestEnemy = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach(Transform t in enemies)
        {
            float distanceToEnemy = Vector3.Distance(t.position, currentPos);
            if(distanceToEnemy < distanceToClosestEnemy)
            {
                tMin = t;
                distanceToClosestEnemy = distanceToEnemy;
               
            }
        }
        return tMin;
         /*if(otherTeam.Length > 0)
         {
             transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
         }
        */
    }
}
