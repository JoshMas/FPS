using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float explosionForce = 10.0f;
    [SerializeField] private float explosionRadius = 2.0f;
    [SerializeField] private float upwardsModifier = 2.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Explode(Rigidbody target)
    {
        target.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier, ForceMode.VelocityChange);
    }
}
