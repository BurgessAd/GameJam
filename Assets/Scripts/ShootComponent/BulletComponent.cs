using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    [Range(0, 20)]
    [SerializeField]
    float bulletDamage = 10.0f;
    [Range(0, 1)]
    [SerializeField]
    float bulletSpeed = 10.0f;
    [Range(0, 100)]
    [SerializeField]
    int bulletFramesSurviveTime = 60;
  
    [SerializeField]
    private Transform bulletTransform;
    [SerializeField]
    private GameObject hitAnimator;

    private Vector2 currentPos;
    private Vector2 bulletDir;
    private RaycastHit2D[] m_Result = new RaycastHit2D[1];
    private bool inFlight = true;
    private int currentBulletTime = 0;
    private int thisLayerMask;

    public void SetBulletLayerAndStart(int bulletLayer)
    {
        currentPos = bulletTransform.position;
        bulletDir = bulletTransform.up;
        thisLayerMask = (1 << LayerMask.NameToLayer("FriendlyCollisions")) + (1 << LayerMask.NameToLayer("EnemyCollisions")) + (1 << LayerMask.NameToLayer("MutantCollisions")) + (1 << LayerMask.NameToLayer("TerrainAndObjectCollisions"));
        thisLayerMask &= ~(1 << bulletLayer);
        StartCoroutine(RunBullet());
    }
    IEnumerator RunBullet()
    {
        
        while (inFlight)
        {
            Physics2D.RaycastNonAlloc(currentPos, bulletDir, m_Result, bulletSpeed, thisLayerMask);
            if (m_Result[0])
            {
                bulletTransform.position = m_Result[0].point;
                inFlight = false;
                HealthComponent healthObj = m_Result[0].collider.GetComponent<HealthComponent>();
                if (healthObj)
                {
                    healthObj.ProcessHit(bulletDamage);
                }
   
            }
            else
            {
                currentPos += bulletSpeed * bulletDir;
                bulletTransform.position = currentPos;
            }
            currentBulletTime += 1;
            if (currentBulletTime > bulletFramesSurviveTime)
            {
                Destroy(gameObject);
            }
            yield return null;
        }

        Instantiate(hitAnimator, bulletTransform.position, bulletTransform.rotation);
        yield return null;
        yield return null;
        Destroy(gameObject);
    }

}
