using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    public bool isDisabled;

    public abstract void Enable();
    public abstract void Disable();

    public abstract void Equip();
    public abstract void Unequip();
}
