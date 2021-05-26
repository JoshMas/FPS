using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shooter.Abilities;

public class Missile : MonoBehaviour
{
    private Attacker attacker;
    private float speed = 5;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, attacker.target.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit " + other.name + "!");

        Destroy(gameObject);
    }
}
