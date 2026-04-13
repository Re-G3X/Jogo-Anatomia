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

    [SerializeField] private InputManager inputManager;
    // 
    private bool gameOver = false;
    [SerializeField] private DialogueTrigger finalDialogue;
    [SerializeField] private DialogueTrigger gameoverDialogue;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        StartCoroutine(DispenseFoodRoutine());
        StartCoroutine(DispenseAirRoutine());
        StartCoroutine(DispenseAirUpRoutine());

        oxigenTime = 30;
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
        while (!gameOver)
        {
            yield return new WaitForSeconds(1f);
            if (Random.Range(0,10) < 3)
            {
                yield return new WaitForSeconds(4f);
            }
            if (foodDispenser != null)
            {
                foodDispenser.Dispense();
            }
        }
    }
    IEnumerator DispenseAirRoutine()
    {
        while (!gameOver)
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
        while (!gameOver)
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
        if (!gameOver && score < 100)
        {
            score += 1;
            canva.Score(score);
        }
    }

    IEnumerator TimeTick()
    {
        while (oxigenTime > 0 && score < 100) { 
            yield return new WaitForSeconds(1);
            oxigenTime -= 1;
            if(oxigenTime < 0) oxigenTime = 0;
            canva.Time(oxigenTime);
        }
        if(oxigenTime <= 0) {
            oxigenTime = 0;
            canva.GameOver();
            gameoverDialogue.TriggerDialogue();
        }
        else if(score >= 100) {
            canva.GameOver();
            finalDialogue.TriggerDialogue();
        }
        Debug.Log("3");
        gameOver = true;
        canva.Time(oxigenTime);
        inputManager.Disable();
        
    }

    public void TimeAddition()
    {
        if (!gameOver)
        {
            oxigenTime += 1;
            canva.Time(oxigenTime);
        }
    }
    public void TimeDeduct(int deduction)
    {
        if (!gameOver){   
            oxigenTime -= deduction;
            canva.Time(oxigenTime);
        }
    }

    
 
}
