using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpigloteGameManager : MonoBehaviour
{
    public FoodDispenser foodDispenser;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DispenseFoodRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DispenseFoodRoutine()
    {
        yield return new WaitForSeconds(1f);
        foodDispenser.Dispense();
        StartCoroutine(DispenseFoodRoutine());
    }
}
