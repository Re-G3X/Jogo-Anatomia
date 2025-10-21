using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    [SerializeField] private bool detectFood;
    private string type = "Air";
    // Start is called before the first frame update
    private void Start()
    {
        if (detectFood)
        {
            type = "Food";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(type)) {
            Debug.Log("ACERTOU!");
        }
        else
        {
            Debug.Log("ERROU!");
        }
    }
}
