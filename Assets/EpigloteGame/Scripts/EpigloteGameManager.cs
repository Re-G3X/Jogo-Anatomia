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
        StartCoroutine(DispenseAirRoutine());
        StartCoroutine(DispenseAirUpRoutine());

        oxigenTime = 25;
        StartCoroutine(TimeTick());

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
            yield return new WaitForSeconds(1f);
            if (Random.Range(0,10) < 2)
            {
                yield return new WaitForSeconds(2f);
            }
            if (foodDispenser != null)
            {
                foodDispenser.Dispense();
            }
        }
    }
    IEnumerator DispenseAirRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.2f);
            if (airDispenser != null)
            {
                airDispenser.Dispense();
            }
        }
    }

    IEnumerator DispenseAirUpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.6f);
            if (airUpDispenser != null)
            {
                airUpDispenser.Dispense();
            }
        }
    }
    public void Score()
    {
        score += 1;
        canva.Score(score);
    }

    IEnumerator TimeTick()
    {
        while (oxigenTime > 0) { 
            yield return new WaitForSeconds(1);
            oxigenTime -= 1;
            if(oxigenTime < 0) oxigenTime = 0;
            canva.Time(oxigenTime);
        }
        if(oxigenTime <= 0)
        {
            oxigenTime = 0;
            canva.Time(oxigenTime);
        }
    }

    public void TimeAddition()
    {
        oxigenTime += 1;
        canva.Time(oxigenTime);
    }
    public void TimeDeduct(int deduction)
    {
        oxigenTime -= deduction;
        canva.Time(oxigenTime);
    }
}
