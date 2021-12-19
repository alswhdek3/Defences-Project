using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourcesType
{
    None=-1,
    Monster,
    Max
}

public class GameManager : Singleton<GameManager>
{
    [Header("WayPoints")]
    [SerializeField]
    private Transform[] wayPoints;

    [Header("Current State")]
    [SerializeField]
    private int currentStage;

    [Header("Monster Storage")]
    [SerializeField]
    private GameObject monsterStorage;

    [Header("CurrentScore")]
    [SerializeField]
    private int currentScore;

    [Header("GameOverBool")]
    [SerializeField]
    private bool isGameover;

    private IEnumerator Coroutine_CreateMonster()
    {
        while(true)
        {
            if (isGameover)
                yield break;

            yield return new WaitForSeconds(2f);

            // Monster Add
            Monster monster = MonsterManager.Instance.CreateMonster();
            SetTargetObjectParent(monster.gameObject, MonsterManager.Instance.gameObject);
            monster.transform.position = wayPoints[0].transform.position;
        }
    }

    #region 프로퍼티
    public int CurrentStage { get { return currentStage; } }

    public int CurrentScore { get { return currentScore; } }
    #endregion

    #region Init Methos
    private void InitStage()
    {
        // 몬스터 생성 시작
        StartCoroutine(Coroutine_CreateMonster());
    }
    #endregion

    #region Set Methods

    public void SetState(int _nextState)
    {
        currentStage = _nextState;
    }

    public void AddScore(int _plusScore)
    {
        currentScore += _plusScore;
    }
    #endregion

    #region WayPoint
    public int WayPointLength { get { return wayPoints.Length; } }

    public Vector3 GetWayPointPosition(int _wayPoint)
    {
        return wayPoints[_wayPoint].transform.position;
    }
    #endregion

    #region Action Methods
    public void SetTargetObjectParent(GameObject _object , GameObject _parentObject)
    {
        _object.transform.SetParent(_parentObject.transform);
        _object.transform.localScale = Vector3.one;
        _object.transform.localPosition = Vector3.zero;
    }
    #endregion

    protected override void OnAwake()
    {
        currentStage = 1;
    }
    protected override void OnStart()
    {
        InitStage();
    }
}
