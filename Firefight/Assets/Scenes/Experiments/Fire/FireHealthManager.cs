using UnityEngine;

public class FireHealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead");
            // Handle player death (e.g., respawn, game over, etc.)
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        Debug.Log("Collision detected with: " + collider.gameObject.name);
        if (collider.gameObject.CompareTag("Water"))
        {
            TakeDamage(1);
            Debug.Log("Player is taking damage from fire");
        }
    }
}
