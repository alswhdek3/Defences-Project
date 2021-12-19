using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void RobotSettingEventHandler(object _sender , RobotSettingEventArgs _e);
public class RobotSettingEventArgs : EventArgs
{
    public RobotSettingEventArgs(BatchRobotType _type , int _level , int _attackValue , float _duration)
    {
        Type = _type;
        Level = _level;
        AttackValue = _attackValue;
        Duration = _duration;
    }

    public BatchRobotType Type { get; private set; }

    public int Level { get; private set; }

    public int AttackValue { get; private set; }

    public float Duration { get; private set; }

}
