using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;
    
    [Header("Death Settings")]
    public string deathAnimationBool = "isDead"; // Nome do parâmetro Bool no Animator
    public float deathAnimationDuration = 1f; // Duração da animação de morte
    public bool useAnimator = true; // Se usa Animator ou destrói direto
    
    [Header("Effects")]
    public GameObject deathEffect;
    public float knockbackForce = 5f;
    
    [Header("Feedback")]
    public float hitFlashDuration = 0.1f;
    public Color hitFlashColor = Color.red;
    
    private Renderer[] renderers;
    private Color[] originalColors;
    private bool isFlashing = false;
    private bool isDead = false;
    private Animator animator;
    
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        
        // Pega todos os renderers para o efeito de flash
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];
        
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
            {
                originalColors[i] = renderers[i].material.color;
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (isDead) return; // Não recebe dano se já está morto
        
        currentHealth -= damage;
        
        // Efeito visual de hit
        if (!isFlashing)
        {
            StartCoroutine(HitFlash());
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    System.Collections.IEnumerator HitFlash()
    {
        isFlashing = true;
        
        // Muda a cor para a cor de hit
        foreach (Renderer rend in renderers)
        {
            if (rend.material.HasProperty("_Color"))
            {
                rend.material.color = hitFlashColor;
            }
        }
        
        yield return new WaitForSeconds(hitFlashDuration);
        
        // Volta para a cor original
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
            {
                renderers[i].material.color = originalColors[i];
            }
        }
        
        isFlashing = false;
    }
    
    void Die()
    {
        isDead = true;
        
        // Desativa colisões para não receber mais dano
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
        
        // Para o movimento do inimigo (se tiver IA)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }
        
        // Cria efeito de morte
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        
        // Toca a animação de morte se tiver Animator
        if (useAnimator && animator != null)
        {
            animator.SetBool(deathAnimationBool, true);
            // Destroi após a animação terminar
            Destroy(gameObject, deathAnimationDuration);
        }
        else
        {
            // Se não tem animação, destroi imediatamente
            Destroy(gameObject);
        }
    }
}