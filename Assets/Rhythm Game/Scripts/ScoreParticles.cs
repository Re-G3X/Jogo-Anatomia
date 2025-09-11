using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreParticles : MonoBehaviour
{
    public ParticleSystem goodParticle;
    public ParticleSystem perfectParticle;
    public ParticleSystem missedParticle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ParticleMiss()
    {
        missedParticle.Play();
    }
    public void ParticleHit()
    {
        goodParticle.Play();
    }
    public void ParticlePerfectHit()
    {
        perfectParticle.Play();
    }
}
