using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FireStyle
{
    SingleShot,
    Barrage
}

public class ShooterComponent : MonoBehaviour
{
    private GunComponent[] gunComponents;

    [SerializeField]
    FireStyle fireStyle = FireStyle.Barrage;
    private SharedProperties fireDelay;

    bool currentShootState;
    bool coroutineStarted = false;

    public void SetFireDelay(SharedProperties fireDelay)
    {
        this.fireDelay = fireDelay;
    }
    private void Awake()
    {
        gunComponents = GetComponentsInChildren<GunComponent>();
    }
    public void SetShootState(bool newShootState)
    {
        if (currentShootState != newShootState)
        {
            currentShootState = newShootState;
            if (newShootState && !coroutineStarted)
            {
                StartCoroutine(ShootRoutine());
            }
        }
    }

    private IEnumerator ShootRoutine()
    {
        coroutineStarted = true;
        while (currentShootState)
        {

            for (int i = 0; i <gunComponents.Length; i++)
            {
                if (!currentShootState)
                {
                    break;
                }
                gunComponents[i].FireWeapon(fireDelay.Value);
                if (fireStyle.Equals(FireStyle.Barrage))
                {
                    yield return new WaitForSeconds(fireDelay.Value / gunComponents.Length);
                }
            }
            if (fireStyle.Equals(FireStyle.SingleShot))
            {
                yield return new WaitForSeconds(fireDelay.Value);
            }
            yield return null;
        }
        coroutineStarted = false;
    }
}
