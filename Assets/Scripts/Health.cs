using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Min(1f)] public float maxHealth = 100f;

    public float CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0f;

    public event Action<Health> OnHealthChanged;
    public event Action<Health> OnDied;

    private void Awake()
    {
        maxHealth = Mathf.Max(1f, maxHealth);
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead || amount <= 0f) return;

        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
        OnHealthChanged?.Invoke(this);

        if (IsDead)
        {
            OnDied?.Invoke(this);

            // Keep the player object so the HUD can display the result.
            if (!CompareTag("Player"))
                Destroy(gameObject, 1.5f);
        }
    }

    public void Heal(float amount)
    {
        if (IsDead || amount <= 0f) return;

        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        OnHealthChanged?.Invoke(this);
    }
}