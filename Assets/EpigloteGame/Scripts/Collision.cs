using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    [SerializeField] private bool detectFood;
    [SerializeField] private bool detectAirUp;
    [SerializeField] private EpigloteGameManager gameManager;
    private string type = "Air";
    // Start is called before the first frame update
    private void Start()
    {
        if (detectFood)
        {
            type = "Food";
        }
        if (detectAirUp) {
            type = "AirUp";
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
            gameManager.Score();
        }
        else
        {
            Debug.Log("ERROU!");
        }
    }
}
