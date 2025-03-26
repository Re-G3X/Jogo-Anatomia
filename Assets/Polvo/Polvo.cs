using System;
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
    [SerializeField]
    private float followDistance = 10;
    [SerializeField]
    private float followSpeed = 3;
    [SerializeField]
    private float atackDistance = 5.7f;

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
         
        HandleOnMovement();
    }

    private void HandleOnMovement()
    {
        bool onRightFromPlayer;
        Vector3 startAtackingPosition;
        if (transform.position.x < player.transform.position.x)
        {
            onRightFromPlayer = false;
            startAtackingPosition = player.transform.position + (Vector3.right * -4) + Vector3.up * 4; 
        }
        else
        {
            onRightFromPlayer = true;
            startAtackingPosition = player.transform.position + (Vector3.right * 4) + Vector3.up * 4;
        }
         
         
        if (!canFlyAtacking)
        {
            transform.position = Vector3.MoveTowards(transform.position, startAtackingPosition, followSpeed * Time.deltaTime);
        }
        else
        {  
            float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
            Debug.Log("Distance from Player :" + distanceFromPlayer);
            if (distanceFromPlayer < followDistance && distanceFromPlayer > atackDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, startAtackingPosition, followSpeed * Time.deltaTime);

                Debug.Log("distanceFromPlayer : " + distanceFromPlayer + "atackDistance: " + atackDistance);
            }
            if (distanceFromPlayer < atackDistance)
            {
                StartCoroutine(FlyingAtack());
            }
        } 
    }
    private List<Vector3> GetFlyingAtackPoints(List<Vector3> atackAimingPoints)
    {
        atackAimingPoints.Clear();
        atackAimingPoints.Add(transform.position);
        atackAimingPoints.Add(player.transform.position); // Posição do player
        float x_distance = player.transform.position.x - this.transform.position.x;
        if(x_distance > 0 && x_distance < 3)
        {
            x_distance = 3;
        }
        else if (x_distance < 0 && x_distance > -3)
        {
            x_distance = -3;
        }
        atackAimingPoints.Add(new Vector3(transform.position.x + (x_distance * 2), transform.position.y, transform.position.z));
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
            StartCoroutine(FlyingAtackResetTime());
        }   
    }

    IEnumerator FlyingAtackResetTime()
    {
        yield return new WaitForSeconds(1f);
        canFlyAtacking = true;
    }

     private void RestartPosition()
    {
        parent.transform.position = this.transform.position;
        this.transform.localPosition = Vector3.zero;
    }
}
