using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float lifetime;
    private Rigidbody rb;
    
    [Header("Bullet Settings")]
    public int damage = 1;
    public LayerMask enemyLayer;
    public GameObject hitEffect;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
    }
    
    public void Initialize(Vector2 dir, float spd, float life)
    {
        direction = dir.normalized;
        speed = spd;
        lifetime = life;
        
        if (rb != null)
        {
            rb.velocity = new Vector3(direction.x * speed, direction.y * speed, 0);
        }
        
        Destroy(gameObject, lifetime);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            return;
        
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }
            
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall") || other.CompareTag("Ground"))
        {
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }
            
            Destroy(gameObject);
        }
    }
}