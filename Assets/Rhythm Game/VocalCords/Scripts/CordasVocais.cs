using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

public class CordasVocais : MonoBehaviour
{
    public Animator animator;
    public InputActionReference hit;
    public Material vibrationMaterial;
    public ParticleSystem noteHitParticle;
    public ParticleSystem noteHitStreakParticle;
    public ParticleSystem noteMissedParticle;
    public ParticleSystem noteLine;
    [SerializeField]
    private int streak = 0;
    public GameObject lightsLevel_1;
    public GameObject lightsLevel_2;
    public Light spotLight;
    public Light spotLightUp;



    private void OnEnable()
    {
        hit.action.started += HitAction;
    }
    private void OnDisable()
    {
        hit.action.started -= HitAction;
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        spotLight.intensity = 0;
        ResetStreak();
    }
    private void HitAction(InputAction.CallbackContext context)
    {
        // animator.SetInteger("Animation", animator.GetInteger("Animation") + 1);
        HitAnimation();
    }
    public void HitAnimation()
    {
        streak++;
        if (streak == 5) { 
            lightsLevel_1.SetActive(false);
            lightsLevel_2.SetActive(true);
        }

        animator.SetInteger("Animation", UnityEngine.Random.Range(1, 6));
        vibrationMaterial.SetFloat("_VibrationStrengh", 0.07f);
        StartCoroutine(AnimationRestart());
        if (streak > 5) {
            noteHitStreakParticle.Play();
            if (streak < 15)
            {
                spotLightUp.intensity = streak * 30;
            }
            if (streak > 25 && streak < 40) {
                spotLight.intensity = (streak - 25) * 30;
            }
            return;
        }

        noteHitParticle.Play();
    }
    public void MissedHitAnimation()
    {
        ResetStreak();
        animator.SetInteger("Animation", 7);
        vibrationMaterial.SetFloat("_VibrationStrengh", 0.07f);
        StartCoroutine(AnimationRestart());

        noteMissedParticle.Play(); 
    }

    private void ResetStreak()
    {
        streak = 0;
        lightsLevel_2.SetActive(false);
        lightsLevel_1.SetActive(true);
    }

    private IEnumerator AnimationRestart()
    {
        yield return new WaitForSeconds(0.7f);
        animator.SetInteger("Animation", 0);
        vibrationMaterial.SetFloat("_VibrationStrengh", 0.0f);
    }

    
}
