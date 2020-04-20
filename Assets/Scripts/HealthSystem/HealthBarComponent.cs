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
    private float baseScale;
    private float scale;
    void Start()
    {


        healthBarImage = Instantiate(healthBarImagePrefab, gameObject.transform.position, Quaternion.identity);
        healthBarImage.GetComponent<SpriteRenderer>().color = Color.green;
        healthBarImage.transform.parent = gameObject.transform;
        healthBarImage.GetComponent<SpriteRenderer>().sortingOrder = 4;
        GetComponent<HealthComponent>().OnCurrentHealthChanged += HealthChanged;
        if (GetComponent<SpriteRenderer>() != null)
        {
            scale = GetComponent<SpriteRenderer>().sprite.textureRect.height / 128f;
        }
        else
        {
            foreach(Transform child in gameObject.transform)
            {
                if (child.gameObject.GetComponent<SpriteRenderer>().sprite != null)
                {
                    scale = child.gameObject.GetComponent<SpriteRenderer>().sprite.textureRect.height / 128f;
                    break;
                }
                
            }
        }
        
        //healthBarImage.transform.RotateAround(healthBarImage.transform.parent.position, new Vector3(0, 0, 1), -healthBarImage.transform.parent.eulerAngles.z);
        rotation = healthBarImage.transform.rotation;
        position = healthBarImage.transform.position;
        healthBarImage.transform.localScale *= scale;
        baseScale = healthBarImage.transform.localScale.x;
        healthBarImage.transform.position += new Vector3(0,0.8f*scale, 0);

        
        GetComponentInParent<HealthComponent>().OnCurrentHealthChanged += HealthChanged;
        
    }
    void HealthChanged(float newHealthPercentage)
    {
        if (newHealthPercentage <= 0)
        {
            healthBarImage.transform.localScale = Vector3.zero;
        }
        else
        {
            healthBarImage.transform.localScale = new Vector3(newHealthPercentage * baseScale, healthBarImage.transform.localScale.y, healthBarImage.transform.localScale.z);
        }
        
    }
    void LateUpdate()
    {
        //healthBarImage.transform.rotation = rotation;
        
    }
}
