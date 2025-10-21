using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirUp : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float gravityUp = 4;
    [SerializeField] private float destroyHeight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > destroyHeight)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        rb.AddForce(Vector3.up * gravityUp, ForceMode.Acceleration);
    }
}
