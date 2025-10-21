using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpigloteGameManager : MonoBehaviour
{
    public FoodDispenser foodDispenser;
    public float time;

    // Colisores
    [SerializeField] private Collider ColliderRespiratorio;
    [SerializeField] private Collider ColliderDigestivo;

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
        while (true)
        {
            Debug.Log("ah!");
            yield return new WaitForSeconds(time);
            foodDispenser.Dispense();
            Debug.Log("DISPENSE!");
        }
    }
}
