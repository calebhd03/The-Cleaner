using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            AudioSource source = GetComponent<AudioSource>();
            //play health sound
            source.Play();

            //add health to player
            other.GetComponent<Character>().HealthToMax();

            //Removes sprite and collider
            Destroy(GetComponentInChildren<SpriteRenderer>());
            Destroy(GetComponent<Collider>());

            //delete object
            Destroy(gameObject, source.clip.length);
        }
    }
}
