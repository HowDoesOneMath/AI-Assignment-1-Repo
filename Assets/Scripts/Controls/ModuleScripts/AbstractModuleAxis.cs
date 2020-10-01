using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A single axis, or 2 buttons that function like an axis.
public abstract class AbstractModuleAxis : AbstractModule
{
    public abstract float GetAxis();
}
