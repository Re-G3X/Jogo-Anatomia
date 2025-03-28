using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;
    private bool isInMissLine = false; // Verifica se a nota está colidindo com a MissLine
    private bool isInPerfectLine = false; // Verifica se a nota está colidindo com a PerfectLine
    private bool isHit = false; // Verifica se a nota foi acertada
    private Lane parentLane; // Referência à Lane da nota

    void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
        parentLane = GetComponentInParent<Lane>(); // Obtém a Lane correspondente à nota
    }

    void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));

        if (t > 1)
        {
            // Se o tempo de vida passar e a nota não foi acertada, conta como miss
            if (!isHit)
            {
                ScoreManager.Miss();
                //Debug.Log("Miss!");
            }
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.right * SongManager.Instance.noteSpawnY,
                                                   Vector3.right * SongManager.Instance.noteDespawnY, t);
            GetComponent<SpriteRenderer>().enabled = true;
        }

        // Verifica se a nota está na MissLine e o jogador pressionou a tecla correta
        if (isInMissLine && Input.GetKeyDown(parentLane.input) && !isHit)
        {
            isHit = true; // Marca como acerto
            ScoreManager.Hit(); // Conta o acerto normal
            Debug.Log("Hit na MissLine!");
            Destroy(gameObject); // Destrói a nota após o acerto
        }

        // Verifica se a nota está na PerfectLine e o jogador pressionou a tecla correta
        if (isInPerfectLine && Input.GetKeyDown(parentLane.input) && !isHit)
        {
            isHit = true; // Marca como acerto perfeito
            ScoreManager.PerfectHit(); // Conta o acerto perfeito
            Debug.Log("Perfect Hit!");
            Destroy(gameObject); // Destrói a nota após o acerto
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MissLine"))
        {
            isInMissLine = true; // Marca que a nota está na MissLine
        }
        if (other.CompareTag("PerfectLine"))
        {
            isInPerfectLine = true; // Marca que a nota está na PerfectLine
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MissLine"))
        {
            isInMissLine = false; // Reseta o status da MissLine
        }
        if (other.CompareTag("PerfectLine"))
        {
            isInPerfectLine = false; // Reseta o status da PerfectLine
        }
    }
}
