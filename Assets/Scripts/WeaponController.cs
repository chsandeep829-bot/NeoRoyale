using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Camera aimCamera;
    [Min(0f)] public float damage = 22f;
    [Min(1f)] public float range = 150f;
    [Min(0.1f)] public float shotsPerSecond = 6f;
    [Min(1)] public int magazineSize = 30;
    [Min(0)] public int reserveAmmo = 120;
    [Min(0.1f)] public float reloadDuration = 1.5f;
    public LayerMask hitMask = Physics.DefaultRaycastLayers;

    public int AmmoInMagazine { get; private set; }
    public bool IsReloading { get; private set; }
    private float nextShotTime;

    private void Awake()
    {
        magazineSize = Mathf.Max(1, magazineSize);
        AmmoInMagazine = magazineSize;
        if (!aimCamera) aimCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || TouchInput.ConsumeReload())
            StartReload();

        bool desktopFire = Input.GetMouseButton(0) && Cursor.lockState == CursorLockMode.Locked;
        if (desktopFire || TouchInput.FireHeld)
            Fire();
    }

    private void Fire()
    {
        if (IsReloading || Time.time < nextShotTime) return;

        if (AmmoInMagazine <= 0)
        {
            StartReload();
            return;
        }

        nextShotTime = Time.time + 1f / Mathf.Max(0.1f, shotsPerSecond);
        AmmoInMagazine--;
        if (!aimCamera) return;

        Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, range, hitMask, QueryTriggerInteraction.Ignore))
        {
            Health target = hit.collider.GetComponentInParent<Health>();
            if (target && !target.CompareTag("Player"))
                target.TakeDamage(damage);
        }
    }

    public void StartReload()
    {
        if (!IsReloading && AmmoInMagazine < magazineSize && reserveAmmo > 0)
            StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        IsReloading = true;
        yield return new WaitForSeconds(reloadDuration);
        int loaded = Mathf.Min(magazineSize - AmmoInMagazine, reserveAmmo);
        AmmoInMagazine += loaded;
        reserveAmmo -= loaded;
        IsReloading = false;
    }

    public void AddAmmo(int amount) => reserveAmmo += Mathf.Max(0, amount);
}