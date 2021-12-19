using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : UnitData
{
    private BatchRobotType type;
    private int attackValue;
    private float duration;
    public override void SetUnit(int _level, Sprite _sprite)
    {
        level = _level;
        sprite = _sprite;
    }

    public void SetRobot(BatchRobotType _type , int _attackValue , float _duration)
    {
        type = _type;
        attackValue = _attackValue;
        duration = _duration;

        // Sprite Setting
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
