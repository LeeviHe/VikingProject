using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpikeProjectile : MonoBehaviour {
    public IceSpikeSO iceSpikeSO;
    public bool hit;

    void OnTriggerEnter ( Collider other ) {
        if (!hit) {
            hit = true;
            Destroy(gameObject);
            Debug.Log("hit : " + other);
            iceSpikeSO.ApplySlowEffect(other);
        } else {
            Debug.Log("aleradyy hit");
        }
    }
}
