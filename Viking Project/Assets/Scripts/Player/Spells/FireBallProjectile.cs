using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireBallProjectile : MonoBehaviour {
    public FireballSO fireballSO;
    public bool hit;
    public NavMeshAgent navMeshAgent;
    public Collider[] withinAggroColliders;
    public LayerMask targetLayerMask;
    public float targetingRange;
    private Vector3 fireballVelocity;

    private Collider originalClosestEnemy;

    void Update() {
        if (target == null || !target.gameObject.activeSelf) {
            FindClosestEnemy();
        }

        if (target != null) {
            transform.parent = null;
            ChaseTarget(target);
        }
    }

    void FindClosestEnemy() {
        withinAggroColliders = Physics.OverlapSphere(transform.position, targetingRange, targetLayerMask);
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in withinAggroColliders) {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                originalClosestEnemy = collider;
            }
        }

        if (originalClosestEnemy != null) {
            SetTarget(originalClosestEnemy.transform);
        }
    }

    private Transform target;

    public void SetTarget( Transform newTarget ) {
        target = newTarget;
    }

    void ChaseTarget( Transform target ) {
        Vector3 direction = (target.position - transform.position).normalized;

        // Calculate velocity using acceleration
        Vector3 acceleration = direction * fireballSO.acceleration;
        fireballVelocity += acceleration * Time.deltaTime;

        // Limit velocity to maximum speed
        fireballVelocity = Vector3.ClampMagnitude(fireballVelocity, fireballSO.spellSpeed);

        // Move the fireball
        transform.Translate(fireballVelocity * Time.deltaTime, Space.World);

        // Rotate the fireball to face the direction it's moving
        if (fireballVelocity != Vector3.zero) {
            Quaternion rotation = Quaternion.LookRotation(fireballVelocity.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, fireballSO.rotationSpeed * Time.deltaTime);
        }
    }

    // Handle hit when trigger is hit
    void OnTriggerEnter( Collider other ) {
        if (!hit) {
            hit = true;
            Destroy(gameObject);
            Debug.Log("hit : " + other);
            fireballSO.Explosion(transform);
        } else {
            Debug.Log("aleradyy hit");
        }
    }
}
