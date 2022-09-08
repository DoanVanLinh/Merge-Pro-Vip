using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "State", menuName = "ScriptableObjects/State", order = 1)]
public class State : ScriptableObject
{
    public string nameState;
    public string id;
    public List<Level> listLevel;
}
