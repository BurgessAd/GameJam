using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// most entities should have this - process hit should be called by the bullet which impacts this.
public class HealthComponent : MonoBehaviour
{
    [SerializeField]
    private float StartingHP;
    private float CurrentHP;
    private void Awake()
    {
        CurrentHP = StartingHP;
        OnCurrentHealthChanged?.Invoke(CurrentHP / StartingHP);
    }

    public void ProcessHit(in float damage)
    {
        CurrentHP -= damage;

        OnCurrentHealthChanged?.Invoke(CurrentHP/StartingHP);

        if (CurrentHP < 0)
        {
            OnObjectDied?.Invoke();
        }
    }

    public event Action OnObjectDied;
    public event Action<float> OnCurrentHealthChanged;
}
