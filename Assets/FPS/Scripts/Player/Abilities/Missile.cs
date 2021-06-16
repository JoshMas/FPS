using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shooter.Player;

public class Missile : MonoBehaviour
{
    public float speed = 1f;
    private PlayerStats player;
    private Transform[] target;
    private GameObject[] otherTeam;
    Transform tMin = null;


    private void Start()
    {
        if (gameObject.tag == "Red Player")
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
        target = new Transform[otherTeam.Length];

        for (int i = 0; i < otherTeam.Length; ++i)
        {
            target[i] = otherTeam[i].transform;
        }
    }

    void Update()
    {
        FindClosest(target);
        if (tMin == null)
            transform.Translate(Vector3.forward * Time.deltaTime);
        else
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
        if(gameObject.tag == "Red Player" && other.gameObject.tag == "Blue Player")
        {
            player.currentHealth -= 50;
        }
        if (gameObject.tag == "Blue Player" && other.gameObject.tag == "Red Player")
        {
            player.currentHealth -= 50;
        }
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
