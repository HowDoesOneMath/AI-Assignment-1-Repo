using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Controller script
//Contains references to modules, which indicate the purpose of specific button groups
//I actually wrote this code and the module code 9 months ago
//I just like using it so much I keep it in every project
//This is only for keyboard/mouse input, nothing more, so I figured it would be ok to port over.

[CreateAssetMenu(fileName = "Controller", menuName = "ScriptableObjects/Controller")]
public class Controller : ScriptableObject
{
    public AbstractModuleDirectional movement;
    public AbstractModuleDirectional rotate;
    public AbstractModuleButton detonate;
    public AbstractModuleButton quit;
    public AbstractModuleButton recreate;
}
