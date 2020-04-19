using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FireStyle
{
    SingleShot,
    Barrage
}

[RequireComponent(typeof(AttackAnimator))]
public class ShooterComponent : MonoBehaviour
{
    private GunComponent[] gunComponents;

    [SerializeField]
    FireStyle fireStyle = FireStyle.Barrage;

    [SerializeField]
    private SharedProperties fireDelay;

    bool currentShootState;
    bool coroutineStarted = false;

    private void Awake()
    {
        gunComponents = GetComponentsInChildren<GunComponent>();
        if (fireDelay == null)
        {
            fireDelay = ScriptableObject.CreateInstance<SharedProperties>() as SharedProperties;
            fireDelay.Value = 2.0f;
        }
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
                gunComponents[i].FireWeapon();
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
