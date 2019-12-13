using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Menu Choice", menuName = "Player Menu Choice")]
public class PlayerMenuChoice : ScriptableObject
{
    public Sprite image;
    public string description;
    public TypeOfCommand typeOfCommand;

}
