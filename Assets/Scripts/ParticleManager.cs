using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

    public static ParticleManager instance = null;

    Transform particleLocation;
    public ParticleSystem explosionSystem;
    public ParticleSystem shineSystem;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        particleLocation = GetComponent<Transform>();
    }

    public void generateParticles(string particleType, Transform generatorLocation)
    {
        Debug.Log(generatorLocation);
        particleLocation.position = new Vector3(generatorLocation.position.x,generatorLocation.position.y,0);

        //Location.Translate(0, 0, -5);

        if (particleType == "explosion")
        {
            Instantiate<ParticleSystem>(explosionSystem, particleLocation);

        }
        else if(particleType == "shine")
        {
            Instantiate<ParticleSystem>(shineSystem, particleLocation);
        }
    }
}
