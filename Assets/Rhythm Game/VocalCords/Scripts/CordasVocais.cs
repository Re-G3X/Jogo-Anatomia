using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CordasVocais : MonoBehaviour
{
    public Animator animator;
    public InputActionReference hit;
    public Material vibrationMaterial;
    public ParticleSystem noteHitParticle;
    public ParticleSystem noteMissedParticle;
    public ParticleSystem noteLine;

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
    }
    private void HitAction(InputAction.CallbackContext context)
    {
        // animator.SetInteger("Animation", animator.GetInteger("Animation") + 1);
        HitAnimation();
    }
    public void HitAnimation()
    {
        animator.SetInteger("Animation", UnityEngine.Random.Range(1, 6));
        //vibrationMaterial.SetFloat("_VibrationStrengh", 0.07f);
        vibrationMaterial.SetFloat("_VibrationStrengh", 0.07f);
        StartCoroutine(AnimationRestart());

        noteHitParticle.Play();
        //noteLine.Play();
    }
    public void MissedHitAnimation()
    {
        animator.SetInteger("Animation", 7);
        vibrationMaterial.SetFloat("_VibrationStrengh", 0.07f);
        StartCoroutine(AnimationRestart());

        noteMissedParticle.Play(); 
    }


    private IEnumerator AnimationRestart()
    {
        yield return new WaitForSeconds(0.7f);
        animator.SetInteger("Animation", 0);
        vibrationMaterial.SetFloat("_VibrationStrengh", 0.0f);
    }

    
}
