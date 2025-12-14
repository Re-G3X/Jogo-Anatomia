using System.Collections;
using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.15f; // Taxa de tiro (similar ao Cuphead)
    public float bulletSpeed = 20f;
    public float bulletLifetime = 2f;
    
    [Header("Aim Settings")]
    public float minAimMagnitude = 0.3f; // Magnitude mínima para começar a atirar
    public bool useEightDirections = true; // Usa 8 direções fixas como Cuphead
    public bool invertX = false; // Inverte o eixo X se necessário
    public bool invertY = false; // Inverte o eixo Y se necessário
    
    [Header("Visual Feedback")]
    public Transform aimIndicator; // Opcional: indicador visual da direção
    public float aimIndicatorDistance = 1.5f;
    
    private PlayerController playerController;
    private float nextFireTime = 0f;
    private Vector2 currentAimDirection;
    private bool isShooting = false;
    
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        
        if (firePoint == null)
        {
            // Cria um ponto de disparo automático se não existir
            GameObject fp = new GameObject("FirePoint");
            fp.transform.parent = transform;
            fp.transform.localPosition = new Vector3(0.5f, 0.5f, 0);
            firePoint = fp.transform;
        }
    }
    
    void Update()
    {
        HandleAiming();
        HandleShooting();
    }
    
    void HandleAiming()
    {
        Vector2 lookInput = playerController.look;
        
        // Verifica se o jogador está usando o joystick de mira
        if (lookInput.magnitude >= minAimMagnitude)
        {
            isShooting = true;
            
            // Aplica inversões se necessário
            float x = invertX ? -lookInput.x : lookInput.x;
            float y = invertY ? -lookInput.y : lookInput.y;
            
            // Normaliza a direção
            Vector2 aimDir = new Vector2(x, y).normalized;
            
            // Se usar 8 direções fixas (estilo Cuphead)
            if (useEightDirections)
            {
                aimDir = GetEightDirection(aimDir);
            }
            
            currentAimDirection = aimDir;
            
            // Debug para verificar a direção
            Debug.DrawRay(transform.position, new Vector3(aimDir.x, aimDir.y, 0) * 2f, Color.red);
            
            // Atualiza indicador visual (opcional)
            if (aimIndicator != null)
            {
                aimIndicator.gameObject.SetActive(true);
                Vector3 indicatorPos = transform.position + new Vector3(aimDir.x, aimDir.y, 0) * aimIndicatorDistance;
                aimIndicator.position = indicatorPos;
            }
        }
        else
        {
            isShooting = false;
            
            if (aimIndicator != null)
            {
                aimIndicator.gameObject.SetActive(false);
            }
        }
    }
    
    void HandleShooting()
    {
        if (isShooting && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }
    
    void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet Prefab não está atribuído!");
            return;
        }
        
        // Instancia o projétil
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        
        // Configura a direção do projétil
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(currentAimDirection, bulletSpeed, bulletLifetime);
        }
        else
        {
            // Fallback se não tiver o script Bullet
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = new Vector3(currentAimDirection.x * bulletSpeed, currentAimDirection.y * bulletSpeed, 0);
            }
            
            Destroy(bullet, bulletLifetime);
        }
        
        // Rotaciona o projétil para apontar na direção correta
        float angle = Mathf.Atan2(currentAimDirection.y, currentAimDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    
    // Converte qualquer direção para uma das 8 direções fixas
    Vector2 GetEightDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Normaliza o ângulo para 0-360
        if (angle < 0) angle += 360;
        
        // Determina qual das 8 direções está mais próxima
        int index = Mathf.RoundToInt(angle / 45f) % 8;
        
        switch (index)
        {
            case 0: return new Vector2(1, 0);      // Direita
            case 1: return new Vector2(1, 1).normalized;   // Diagonal direita-cima
            case 2: return new Vector2(0, 1);      // Cima
            case 3: return new Vector2(-1, 1).normalized;  // Diagonal esquerda-cima
            case 4: return new Vector2(-1, 0);     // Esquerda
            case 5: return new Vector2(-1, -1).normalized; // Diagonal esquerda-baixo
            case 6: return new Vector2(0, -1);     // Baixo
            case 7: return new Vector2(1, -1).normalized;  // Diagonal direita-baixo
            default: return new Vector2(1, 0);
        }
    }
}