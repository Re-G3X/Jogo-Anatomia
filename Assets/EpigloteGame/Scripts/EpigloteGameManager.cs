using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpigloteGameManager : MonoBehaviour
{
    public FoodDispenser foodDispenser;
    public FoodDispenser airDispenser;
    public FoodDispenser airUpDispenser;
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
            yield return new WaitForSeconds(time);
            int rand = Random.Range(0, 4);
            switch (rand) { 
                case 0:
                    if (foodDispenser != null)
                    {
                        foodDispenser.Dispense();
                    }
                    break;
                case 1:
                    if (airDispenser != null) {
                        airDispenser.Dispense();
                        foodDispenser.Dispense();
                    }
                    break;

                case 2:
                    if (airUpDispenser != null)
                    {
                        airUpDispenser.Dispense();
                    }
                    break;
            } 
        }
    }
    public void Score()
    {
        score += 1;
        canva.Score(score);
    }
}
