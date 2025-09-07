using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class FireHealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int growthRate = 5; // Health increase per second
    public int damageRate = 1; // Health decrease per second when in contact with water

    public ScoreManager scoreManager;

    private float fireSize;

    public bool isDead { get; private set; }

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


        if (currentHealth <= 0)
        {

            Debug.Log("Fire death!");
            // Handle player death (e.g., respawn, game over, etc.)
            var isSmoke = true;
            var animator = GetComponent<Animator>();
            animator.SetBool("IsSmoke", isSmoke);
            isDead = true;
            //transform.localScale = new Vector2(1, 1);
            StartCoroutine(HandleDeath(isSmoke, animator));
        }
    }

    IEnumerator HandleDeath(bool isSmoke, Animator localAnimator)
    {
        if (scoreManager)
        {
            scoreManager.IncrementScore();
        }

        yield return new WaitForSeconds(5f);

        // Handle transition over to smoke
        localAnimator.SetBool("IsSmoke", !isSmoke);
        transform.localScale = new Vector2(fireSize, fireSize);
        Destroy(gameObject);

    }


    IEnumerator GrowFireLoop()
    {
        while (!isDead)
        {
            GrowFire(growthRate);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void GrowFire(int growth)
    {
        if (isDead) return;

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
        if (!isDead && collider.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            TakeDamage(damageRate);
        }
    }
}
