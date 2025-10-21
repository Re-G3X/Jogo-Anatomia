using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDispenser : MonoBehaviour
{
    public GameObject food;
    public GameObject air;
    public Vector3 instantiatePosition;
    public float offSet;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Dispense()
    {
        if (Random.Range(0, 3) % 2 == 1) {
            Instantiate(food, instantiatePosition + Vector3.right * Random.Range(-offSet, offSet), Quaternion.identity);
        }
        else
        {
            Instantiate(air, instantiatePosition + Vector3.right * Random.Range(-offSet, offSet), Quaternion.identity);
        }
        
    }
}
