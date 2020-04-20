using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamComponent : MonoBehaviour
{
    public bool isPlayer;
    public void SetPlayer(bool isPlayer)
    {
        this.isPlayer = isPlayer;
        Debug.Log(isPlayer);
        if (isPlayer)
        {
            gameObject.layer = LayerMask.NameToLayer("FriendlyCollisions");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("EnemyCollisions");
        }
        GunComponent[] gunComponents = GetComponentsInChildren<GunComponent>(); foreach (GunComponent gunComponent in gunComponents) { gunComponent.bulletLayer = gameObject.layer; }
    }
}
