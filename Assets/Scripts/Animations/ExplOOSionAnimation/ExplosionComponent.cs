using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionObjectPrefab;
    [SerializeField]
    int numInitialExplosion;


    [SerializeField]
    float finalExplosionSize;
    [SerializeField]
    float startExplosionSize;
    [SerializeField]
    float timeAlive;
    float timeOfExplosion;

    private void Awake()
    {
        GetComponent<HealthComponent>().OnObjectDied += ObjectDied;
        
    }

    private void ObjectDied()
    {
        StartCoroutine(ExplodeRoutine());
    }

    IEnumerator ExplodeRoutine()
    {
        while (timeAlive > timeOfExplosion)
        {
            Vector2 point = Random.insideUnitCircle;
            SingleExplosion explosion = Instantiate(explosionObjectPrefab, transform.position + new Vector3(point.x, point.y, 0), transform.rotation).GetComponent<SingleExplosion>();
            explosion.gameObject.SetActive(true);
            explosion.SetSize(startExplosionSize);
            explosion.Begin();
            
            timeOfExplosion += timeAlive / numInitialExplosion;
            yield return new WaitForSeconds(timeAlive / numInitialExplosion);
        }
        SingleExplosion bigexplosion = Instantiate(explosionObjectPrefab, transform.position, transform.rotation).GetComponent<SingleExplosion>();
        bigexplosion.gameObject.SetActive(true);
        bigexplosion.SetSize(finalExplosionSize);
        bigexplosion.Begin();
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
        yield return null;
    }
}
