using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// in-game representation of a dropped piece of carbon/uranium (necessitates adding in pressing space for picking up the item)
[RequireComponent(typeof(SpriteRenderer))]
public class ItemObjectPickable : MonoBehaviour
{
    private ItemObject objectType;
    private int objectNum;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetItemObject(ItemObject objectType, in int number)
    {
        this.objectType = objectType;
        spriteRenderer.sprite = objectType.objectImage;
        spriteRenderer.drawMode = SpriteDrawMode.Simple;
        objectNum = number;
    }

    public void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        other.GetComponent<InventoryComponent>().AddItem(objectType, objectNum);
    }
}
