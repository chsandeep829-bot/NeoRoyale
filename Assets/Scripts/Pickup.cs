using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType { Ammo, Heal }

    public PickupType type = PickupType.Ammo;
    [Min(1)] public int amount = 30;
    public bool destroyAfterPickup = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        bool collected = false;

        if (type == PickupType.Ammo)
        {
            WeaponController weapon = other.GetComponentInParent<WeaponController>();
            if (weapon)
            {
                weapon.AddAmmo(amount);
                collected = true;
            }
        }
        else
        {
            Health health = other.GetComponentInParent<Health>();
            if (health && !health.IsDead && health.CurrentHealth < health.maxHealth)
            {
                health.Heal(amount);
                collected = true;
            }
        }

        if (collected && destroyAfterPickup)
            Destroy(gameObject);
    }
}