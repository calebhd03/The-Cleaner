using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;

    void Update() {
        
    }

    /// 'Hits' the target for a certain amount of damage
    public void Hit(float damage) {
        health -= damage;
        
        if(health <= 0) {
            Destroy(gameObject);
        }
    }
}
