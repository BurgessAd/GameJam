using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// scriptable object representing an object. think of it like an enum, with data fields.
[CreateAssetMenu]
public class ItemObject : ScriptableObject
{
    public Sprite objectImage;
    public Sprite UIImage;
}