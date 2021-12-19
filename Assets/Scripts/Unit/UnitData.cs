using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitData : MonoBehaviour
{
    protected int level;
    protected Sprite sprite;

    public abstract void SetUnit(int _level, Sprite _sprite);
}
