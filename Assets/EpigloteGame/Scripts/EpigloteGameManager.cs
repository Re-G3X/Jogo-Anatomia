using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpigloteGameManager : MonoBehaviour
{
    public FoodDispenser foodDispenser;
    public float time;

    // Colisores
    [SerializeField] private Collider ColliderRespiratorio;
    [SerializeField] private Collider ColliderDigestivo;
    [SerializeField] private CanvaManager canva;
    // Pontuação
    [SerializeField] private int score;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        StartCoroutine(DispenseFoodRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DispenseFoodRoutine()
    {
        while (true)
        {
            Debug.Log("ah!");
            yield return new WaitForSeconds(time);
            foodDispenser.Dispense();
            Debug.Log("DISPENSE!");
        }
    }
    public void Score()
    {
        score += 1;
        canva.Score(score);
    }
}
