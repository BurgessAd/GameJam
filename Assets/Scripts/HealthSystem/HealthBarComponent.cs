using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// visual representation of the remaining health of the entity
// requires healthBarImage being set in the inspector
public class HealthBarComponent : MonoBehaviour
{
    [SerializeField]
    public GameObject healthBarImagePrefab;
    private GameObject healthBarImage;
    private Quaternion rotation;
    private Vector3 position;
    private float scale;
    void Start()
    {


        healthBarImage = Instantiate(healthBarImagePrefab, gameObject.transform.position, Quaternion.identity);
        healthBarImage.GetComponent<SpriteRenderer>().color = Color.green;
        healthBarImage.transform.parent = gameObject.transform;
        healthBarImage.GetComponent<SpriteRenderer>().sortingOrder = 2;
        GetComponent<HealthComponent>().OnCurrentHealthChanged += HealthChanged;
        scale = GetComponent<SpriteRenderer>().sprite.textureRect.height / 128f;
        //healthBarImage.transform.RotateAround(healthBarImage.transform.parent.position, new Vector3(0, 0, 1), -healthBarImage.transform.parent.eulerAngles.z);
        rotation = healthBarImage.transform.rotation;
        position = healthBarImage.transform.position;
        healthBarImage.transform.localScale *= scale;
        
        healthBarImage.transform.position += new Vector3(0,0.8f*scale, 0);

        
        GetComponentInParent<HealthComponent>().OnCurrentHealthChanged += HealthChanged;
        
    }
    void HealthChanged(float newHealthPercentage)
    {
        healthBarImage.transform.localScale = new Vector3(newHealthPercentage / 100, healthBarImage.transform.localScale.y, healthBarImage.transform.localScale.z)*scale;
       
    }
    void LateUpdate()
    {
        //healthBarImage.transform.rotation = rotation;
        
    }
}
