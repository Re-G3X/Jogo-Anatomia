using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioSource theMusic;

    public bool startPlaying;

    public BeatScroller theBS;

    public static GameManager instance;

    public GameObject vocalChordsModel;
    [SerializeField]
    private CordasVocais vocalChords;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        vocalChords = vocalChordsModel.GetComponent<CordasVocais>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                theBS.hasStarted = true;

                theMusic.Play();
            }
        }
    }

    public void NoteHit()
    {
        Debug.Log("Hit On Time");
        vocalChords.HitAnimation();

    }

    public void NoteMissed()
    {
        Debug.Log("Missed Note");
        vocalChords.MissedHitAnimation();
    }
}
