using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Items/Item")]
public class itemData : ScriptableObject
{
    public string description;

    //Icon to be displayed in UI
    public Sprite thumbnail;

    //GameObject to be shown in the scene
    public GameObject gameModel;
}
