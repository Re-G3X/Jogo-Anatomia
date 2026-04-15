using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;
    private bool isInGoodLine = false; // Verifica se a nota está colidindo com a GoodLine
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
        //keyPressedThisFrame = false; // Reseta a flag a cada frame
        //Debug.Log("KeyPressedthisFrame = " + keyPressedThisFrame);
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));

        if (t > 1)
        {
            // Se o tempo de vida passar e a nota não foi acertada, conta como miss
            if (!isHit)
            {
                ScoreManager.Miss();
            }
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.right * SongManager.Instance.noteSpawnY,
                                                   Vector3.right * SongManager.Instance.noteDespawnY, t);
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GoodLine"))
        {
            isInGoodLine = true; // Marca que a nota está na GoodLine
        }
        if (other.CompareTag("PerfectLine"))
        {
            isInPerfectLine = true; // Marca que a nota está na PerfectLine
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("GoodLine"))
        {
            isInGoodLine = false; // Reseta o status da GoodLine
        }
        if (other.CompareTag("PerfectLine"))
        {
            isInPerfectLine = false; // Reseta o status da PerfectLine
        }
    }

    public bool CanBeHit()
    {
        return (isInPerfectLine || isInGoodLine) && !isHit;
    }

    public bool IsPerfect()
    {
        return isInPerfectLine;
    }

    public void Hit()
    {
        isHit = true;
        Destroy(gameObject);
    }
}
