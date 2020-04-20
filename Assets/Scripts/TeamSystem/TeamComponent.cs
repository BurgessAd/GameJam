using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamComponent : MonoBehaviour
{
    public bool isPlayer;
    public void SetPlayer(bool isPlayer)
    {
        this.isPlayer = isPlayer;
        if (isPlayer)
        {
            gameObject.layer = LayerMask.NameToLayer("FriendlyCollisions");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("EnemyCollisions");
        }
    }
}
