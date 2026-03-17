using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpigloteGameManager : MonoBehaviour
{
    public FoodDispenser foodDispenser;
    public FoodDispenser airDispenser;
    public FoodDispenser airUpDispenser;
    public float dispenseTime;

    // Colisores
    [SerializeField] private Collider ColliderRespiratorio;
    [SerializeField] private Collider ColliderDigestivo;
    [SerializeField] private CanvaManager canva;
    // Pontuação
    [SerializeField] private int score;



    [SerializeField] private int oxigenTime;


    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        StartCoroutine(DispenseFoodRoutine());

        oxigenTime = 10;
        StartCoroutine(TimeDeduct());

        canva.Score(score);
        canva.Time(oxigenTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DispenseFoodRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(dispenseTime);
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
                    }
                    break;
                case 2:
                    if (airDispenser != null)
                    {
                        airDispenser.Dispense();
                    }
                    break;
                case 3:
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

    IEnumerator TimeDeduct()
    {
        while (oxigenTime > 0) { 
            yield return new WaitForSeconds(1);
            oxigenTime -= 1;
            if(oxigenTime < 0) oxigenTime = 0;
            canva.Time(oxigenTime);
        }
    }

    public void TimeAddition()
    {
        oxigenTime += 3;
        canva.Time(oxigenTime);
    }
}
