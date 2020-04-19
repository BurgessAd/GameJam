using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// visual representation of the remaining health of the entity
// requires healthBarImage being set in the inspector
public class HealthBarComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject healthBarImage;
    void Start()
    {
        healthBarImage.GetComponent<SpriteRenderer>().color = Color.green;
        healthBarImage.transform.parent = gameObject.transform;
        
        GetComponentInParent<HealthComponent>().OnCurrentHealthChanged += HealthChanged;
        healthBarImage.transform.RotateAround(healthBarImage.transform.parent.position, new Vector3(0, 0, 1), -healthBarImage.transform.parent.eulerAngles.z);
        healthBarImage.transform.position += new Vector3(0, 0.8f, 0);
    }
    void HealthChanged(float newHealthPercentage)
    {
        healthBarImage.transform.localScale = new Vector3(newHealthPercentage / 100, healthBarImage.transform.localScale.y, healthBarImage.transform.localScale.z);
       
    }
    void LateUpdate()
    {
        
    }
}
