using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction; // Restrição de nota para essa lane 
    public KeyCode input; // Tecla associada para ativar essa lane
    public GameObject notePrefab;
    List<Note> notes = new List<Note>(); // Lista de notas ativas na lane 
    public List<double> timeStamps = new List<double>();

    int spawnIndex = 0;
    int inputIndex = 0; 
    public static bool keyPressedThisFrame = false; // Impede múltiplas notas no mesmo frame
    public static CordasVocais vocalCords;

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
        keyPressedThisFrame = false; // Reseta a flag para o próximo frame

        // Verifica se ainda há notas para spawnar e se é o momento correto
        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
            {
                // Instancia uma nova nota e armazena na lista
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                spawnIndex++;
            }
        }
        
        // Verifica se ainda há notas para capturar input e processar acertos/erros
        if (inputIndex < notes.Count && Input.GetKeyDown(input) && keyPressedThisFrame == false)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            
            

            // Verifica se a tecla foi pressionada no tempo correto
            if (Math.Abs(audioTime - timeStamp) < marginOfError) // Esse cara não tá entrando...
            { 
                // Nota acertada dentro da margem de erro
                Hit();
                //keyPressedThisFrame = true; // Impede que outras notas sejam acertadas no mesmo frame
                Destroy(notes[inputIndex].gameObject);
                notes.RemoveAt(inputIndex); // Remove a nota da lista para evitar erros futuros
                
            }
        }

    }

    // Jogador acertou uma nota
    private void Hit()
    {
        ScoreManager.Hit();
    }

    // Jogador errou uma nota
    private void Miss()
    {
        ScoreManager.Miss();
    }

    public void SetKeyIsPressedThisFrame()
    {
        keyPressedThisFrame = true;
        vocalCords.HitAnimation(noteRestriction.ToString());
    }
    public bool GetKeyIsPressedThisFrame()
    {
        return keyPressedThisFrame;
    }
}
