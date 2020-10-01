using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A Vector3 direction in space
public abstract class AbstractModuleDirectional : AbstractModule
{
    public abstract Vector3 GetDirectionalInput();
}
