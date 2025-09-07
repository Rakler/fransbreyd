using System.Collections;
using UnityEngine;

public class FireHealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int growthRate = 5; // Health increase per second
    public int damageRate = 1; // Health decrease per second when in contact with water

    private float fireSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = 1;
        fireSize = (float)currentHealth / 100;
        transform.localScale = new Vector2(fireSize, fireSize);

        StartCoroutine(GrowFireLoop());
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

    IEnumerator GrowFireLoop()
    {
        while (true)
        {
            GrowFire(growthRate);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void GrowFire(int growth)
    {
        currentHealth += growth;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        fireSize = (float)currentHealth / 100;
        transform.localScale = new Vector2(fireSize, fireSize);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            TakeDamage(damageRate);
        }
    }
}
