using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Status", menuName = "Status")]

public class Status : ScriptableObject
{
    public string statusName;
    public Sprite icon;

}
