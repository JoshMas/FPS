using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class Projectile : NetworkBehaviour
{
    [SerializeField] private float explosionForce = 10.0f;
    [SerializeField] private float explosionRadius = 2.0f;
    [SerializeField] private float upwardsModifier = 2.0f;

    private List<Rigidbody> targets;

    private void Awake()
    {
        targets = new List<Rigidbody>();
    }

    private void Explode(Rigidbody _target)
    {
        _target.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        targets.Add(other.attachedRigidbody);
    }

    private void OnTriggerExit(Collider other)
    {
        targets.Remove(other.attachedRigidbody);
    }

    private void OnDestroy()
    {
        foreach(Rigidbody target in targets)
        {
            if(target != null)
                Explode(target);
        }
    }
}
