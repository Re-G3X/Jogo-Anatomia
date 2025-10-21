using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDispenser : MonoBehaviour
{
    public GameObject food;
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
                Instantiate(airUp, instantiatePosition + Vector3.right * Random.Range(-offSet, offSet), Quaternion.identity);
            }
            else {
                Instantiate(air, instantiatePosition + Vector3.right * Random.Range(-offSet, offSet), Quaternion.identity);
            }
                
        }
        else
        {
            if (Random.Range(0, 3) % 2 == 1)
            {
                Instantiate(food, instantiatePosition + Vector3.right * Random.Range(-offSet, offSet), Quaternion.identity);
            }
            else
            {
                Instantiate(air, instantiatePosition + Vector3.right * Random.Range(-offSet, offSet), Quaternion.identity);
            }
        }
        
        
    }
}
