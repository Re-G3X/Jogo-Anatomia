using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySong : MonoBehaviour
{
    public AudioSource audioSource_som;
    
    public void Play(float delay)
    {
        StartCoroutine(WaitAndPlay(delay));
        
    }

    IEnumerator WaitAndPlay(float time)
    {
        yield return new WaitForSeconds(time);
        audioSource_som.Play();
    }
}
