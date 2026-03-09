using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDispenser : MonoBehaviour
{
    public GameObject food_1;
    public GameObject food_2;
    public GameObject air;
    public GameObject airUp;
    public Vector3 instantiatePosition;
    public float offSet;

    public bool onlyAir;
    public bool goingUp;
    // Start is called before the first frame update

    public void Dispense()
    {
        if (onlyAir)
        {
            if (goingUp)
            {
                Instantiate(airUp, instantiatePosition, Quaternion.identity);
            }
            else {
                Instantiate(air, instantiatePosition, Quaternion.identity);
            }
                
        }
        else
        {
            if(UnityEngine.Random.Range(0, 2) == 0)
            {
                Instantiate(food_1, instantiatePosition, Quaternion.identity);
            }
            else {
                Instantiate(food_2, instantiatePosition, Quaternion.identity);
            }
        }
        
        
    }
}
