using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BatchRobotType
{
    None=-1,
    Robot_0,
    Max
}
public class BatchManager : Singleton<BatchManager>
{
    [Header("RobotPrefab")]
    [SerializeField]
    private GameObject robotPrefab;

    private GameobjectPool<Robot> robotPool;

    // EventHandler
    public event RobotSettingEventHandler RobotSettingEventHandler;

    public void SetObjectSetParent(GameObject _obj)
    {
        _obj.transform.SetParent(transform);
        _obj.transform.localScale = Vector3.one;
        _obj.transform.localPosition = Vector3.zero;       
    }

    public Sprite GetSpriteLoad(string _path , BatchRobotType _type)
    {
        return Resources.Load<Sprite>($"{_path}/{_type.ToString()}");
    }

    private void InitCreateRobot()
    {
        robotPool = new GameobjectPool<Robot>(2, () =>
        {
            GameObject obj = Instantiate(robotPrefab);
            SetObjectSetParent(obj);

            // RobotPrefab ComponetConnect
            Robot robot = obj.GetComponent<Robot>();
            robot.gameObject.SetActive(false);

            return robot;
        });
    }

    public Robot CreateRobot(BatchRobotType _type)
    {
        // RobotSetting Value
        int level = (int)_type + 1;
        int attackValue = (int)_type + 1 * 100;
        Sprite robotSprite = GetSpriteLoad("Robot", BatchRobotType.Robot_0);
        float duration = (int)_type + 1 * 20;

        // Robot Pool Get Push
        Robot robot = robotPool.Get();

        // RobotSetting
        RobotSettingEventArgs robotSettingEventArgs = new RobotSettingEventArgs(_type, level, attackValue, duration);
        robot.SetUnit(level, robotSprite);
        robot.SetRobot(_type, attackValue, duration);
        robot.gameObject.SetActive(true);

        // EventHandler Call
        RobotSettingEventHandler(this, robotSettingEventArgs);

        return robot;
    }

    public void RemoveRobot(Robot _robot)
    {
        _robot.gameObject.SetActive(false);
        robotPool.Set(_robot);
    }

    private void RobotSettingShowLog(object _sender, RobotSettingEventArgs _e)
    {
        Debug.Log($"RobotType:{_e.Type}/Level:{_e.Level}/AttackValue{_e.AttackValue}/Duration{_e.Duration}");
    }

    protected override void OnStart()
    {
        // EventHandler Add
        RobotSettingEventHandler += RobotSettingShowLog;

        // InitRobotPool Setting
        InitCreateRobot();

        
    }
}
