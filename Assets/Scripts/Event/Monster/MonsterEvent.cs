using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CreateMonsterEventHandler(object _sender, MonsterSettingEvent _e);
public delegate void RemoveMonsterEventHandler(object _sender, MonsterSettingEvent _e);
public class MonsterSettingEvent : EventArgs
{
    public MonsterSettingEvent(int _level , int _hp , float _speed)
    {
        Level = _level;
        HP = _hp;
        Speed = _speed;
    }


    public int Level { get; private set; }

    public int HP { get; private set; }

    public float Speed { get;  private set; }
}
