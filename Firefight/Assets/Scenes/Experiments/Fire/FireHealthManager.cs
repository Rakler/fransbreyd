using UnityEngine;

public class FireHealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private float fireSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        fireSize = (float)maxHealth / 100;
        transform.localScale = new Vector2(fireSize, fireSize);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        fireSize = (float)currentHealth / 100;
        transform.localScale = new Vector2(fireSize, fireSize);

        if (currentHealth <= 0)
        {
            // Handle player death (e.g., respawn, game over, etc.)
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Water"))
        {
            TakeDamage(1);
        }
    }
}
