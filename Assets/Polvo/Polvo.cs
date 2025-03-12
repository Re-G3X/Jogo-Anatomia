using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Polvo : MonoBehaviour
{
    
    public List<Vector3> atackAimingPoints = new List<Vector3>();

    private GameObject player;
    private Transform parent;
    [SerializeField] 
    private bool canFlyAtacking = true;

    float time = 0f;
    float interval = 4f;


    public float t = 0;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        parent = transform.parent;
        
    }

    // Update is called once per frame
    void Update()
    {
        /*time += Time.deltaTime;
        while (time < interval) { 
            canFlyAtacking = true;
            time -= Time.deltaTime;
        }


        if (canFlyAtacking)
        {
            
        }*/
        //StartCoroutine(FlyingAtack());
        StartCoroutine(FlyingAtack());
    }

    private void HandleOnMovement()
    {
        
    }
    private List<Vector3> GetFlyingAtackPoints(List<Vector3> atackAimingPoints)
    {
        atackAimingPoints.Clear();
        atackAimingPoints.Add(transform.position);
        atackAimingPoints.Add(player.transform.position); // Posição do player
        float x_distance = player.transform.position.x - this.transform.position.x;
        /*if (x_distance < 0) { // player a esquerda
            x_distance = x_distance;  
        }*/
        // Outro lado
        atackAimingPoints.Add(new Vector3(transform.position.x + (x_distance * 2), transform.position.y, transform.position.y));
        return atackAimingPoints;
    }

     

    IEnumerator FlyingAtack()
    {
        if ((canFlyAtacking))
        { 
            canFlyAtacking = false; 
            float duration = 1f;
            GetFlyingAtackPoints(atackAimingPoints);
            float height = atackAimingPoints[0].y - atackAimingPoints[1].y;
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                
                
                // Interpolação linear para x e z
                Vector3 pos = Vector3.Lerp(atackAimingPoints[0], atackAimingPoints[2], t);

                // Ajusta a altura (faz uma parábola)
                pos.y += -height * Mathf.Sin(t * Mathf.PI);

                transform.position = pos;

                yield return null;
            }
            t = 0f;
            RestartPosition();
        }
        
    }

     private void RestartPosition()
    {
        parent.transform.position = this.transform.position;
        this.transform.localPosition = Vector3.zero;
    }
}
