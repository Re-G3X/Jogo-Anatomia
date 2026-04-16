using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction; // Restrição de nota para essa lane 
    public KeyCode input; // Tecla associada para ativar essa lane
    public int inputNumber; // Substitui a tecla input, para funcionar com o novo InputSystem
    public GameObject notePrefab;
    List<Note> notes = new List<Note>(); // Lista de notas ativas na lane 
    public List<double> timeStamps = new List<double>();

    int spawnIndex = 0;

    public static CordasVocais vocalCords;
    public Animator cordasVocaisAnimator;

    private void OnEnable()
    {
        InputManagerCordasVocais.OnLanePressed += OnInput;
    }

    private void OnDisable()
    {
        InputManagerCordasVocais.OnLanePressed -= OnInput;
    }

    void OnInput(int pressedLane)
    {
        Debug.Log(pressedLane);
        if (pressedLane == inputNumber)
        {
            OnInput();
        }
    }

    void Start()
    {
        vocalCords = FindObjectOfType<CordasVocais>();
    }

    // Configura os timestamps das notas filtrando apenas as que pertencem a esta lane
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }

    void Update()
    {
        notes.RemoveAll(note => note == null);

        //if (UnityEngine.Input.GetKeyDown(input))
        //{
        //    OnInput();
        //}

        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
            {
                var noteObj = Instantiate(notePrefab, transform);
                Note note = noteObj.GetComponent<Note>();

                note.assignedTime = (float)timeStamps[spawnIndex];

                notes.Add(note);

                spawnIndex++;
            }
        }
    }

    // Jogador acertou uma nota
    private void Hit()
    {
        ScoreManager.Hit(noteRestriction.ToString());
    }

    // Jogador errou uma nota
    private void Miss()
    {
        ScoreManager.Miss();
    }

    public void OnInput()
    {
        if (notes.Count == 0)
            return;

        // procura a primeira nota válida
        Note noteToHit = null;

        foreach (var note in notes)
        {
            if (note != null && note.CanBeHit())
            {
                noteToHit = note;
                break;
            }
        }

        if (noteToHit == null)
            return;

        if (noteToHit.IsPerfect())
        {
            ScoreManager.PerfectHit();
        }
        else
        {
            ScoreManager.Hit(noteRestriction.ToString());
        }

        noteToHit.Hit();
        notes.Remove(noteToHit);
    }
}
