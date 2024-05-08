using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpikeProjectile : MonoBehaviour {
    public IceSpikeSO iceSpikeSO;

    void OnCollisionEnter( Collision collision ) {
        iceSpikeSO.ApplySlowEffect(collision.collider);
        //Instantiate( iceSpikeSO.spellEffectPrefab, collision.collider.transform, true);
        Destroy(gameObject);
    }
}
