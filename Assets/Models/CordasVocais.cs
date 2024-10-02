using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CordasVocais : MonoBehaviour
{
    private Animator animator;
    public InputActionReference hit;
    public Material vibrationMaterial;
    public ParticleSystem noteParticle;
    public ParticleSystem noteLine;

    private void OnEnable()
    {
        hit.action.started += HitAction;
    }
    private void OnDisable()
    {
        hit.action.started -= HitAction;
    }

    private void HitAction(InputAction.CallbackContext context)
    {
        // animator.SetInteger("Animation", animator.GetInteger("Animation") + 1);
        animator.SetInteger("Animation", UnityEngine.Random.Range(1, 6));
        vibrationMaterial.SetFloat("_VibrationStrengh", 0.07f); 
        StartCoroutine(AnimationRestart());

        noteParticle.Play();
        noteLine.Play();
    }

    private IEnumerator AnimationRestart()
    {
        yield return new WaitForSeconds(0.7f);
        animator.SetInteger("Animation", 0);
        vibrationMaterial.SetFloat("_VibrationStrengh", 0.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
 
        
        
    }
}
