using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    [Header("Monster Prefab")]
    [SerializeField]
    private GameObject monsterPrefab;

    // Gameobject Pool
    private GameobjectPool<Monster> monsterPool;

    //EventHandler
    private event CreateMonsterEventHandler CreateMonsterEventHandler;
    private event RemoveMonsterEventHandler RemoveMonsterEventHandler;

    protected override void OnStart()
    {
        // EventHandler Add
        CreateMonsterEventHandler += CreateMonsterEvent;
        RemoveMonsterEventHandler += RemoveMonsterEvent;

        // Monster Pool Setting
        monsterPool = new GameobjectPool<Monster>(5, () =>
        {
            // 초기 스테이지에 해당하는 몬스터 오브젝트풀 세팅
            monsterPrefab = GetMonster(GameManager.Instance.CurrentStage);

            var obj = Instantiate(monsterPrefab);
            obj.transform.SetParent(transform);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;

            Monster monster = obj.GetComponent<Monster>();
            monster.gameObject.SetActive(false);
            return monster;
        });
    }

    public void CreateMonsterEvent(object _sender, MonsterSettingEvent _e)
    {
        Debug.Log($"Create Monster(Level : {_e.Level} / HP : {_e.HP} / Speed : {_e.Speed})");
    }
    public void RemoveMonsterEvent(object _sender, MonsterSettingEvent _e)
    {
        Debug.Log($"Remove Monster(Level : {_e.Level} / HP : {_e.HP} / Speed : {_e.Speed})");

        // 추후 경험치 추가 이벤트 구현 예정
        GameManager.Instance.AddScore(_e.Level * 10);
        Debug.Log($"Current Score : {GameManager.Instance.CurrentScore}");
    }

    #region Monster Create / Monster
    public Monster CreateMonster()
    {
        // Queue에서 Monster을 꺼낸다
        var newMonster = monsterPool.Get();
        newMonster.SetMonster(GameManager.Instance.CurrentStage, GameManager.Instance.CurrentStage * 100, GameManager.Instance.CurrentStage * 2.5f);

        //EventHandler Show
        if (CreateMonsterEventHandler != null)
            CreateMonsterEventHandler(this, new MonsterSettingEvent(newMonster.Level, newMonster.Hp, newMonster.Speed));

        newMonster.gameObject.SetActive(true);
        if (!Handcuffs.Instance.monsterList.Contains(newMonster))
        {
            Handcuffs.Instance.monsterList.Add(newMonster);
            Debug.Log($"MonsterListCount : {Handcuffs.Instance.monsterList.Count}");
        }

        return newMonster;
    }

    public void RemoveMonster(Monster _monster)
    {
        _monster.gameObject.SetActive(false);
        monsterPool.Set(_monster);
        RemoveMonsterEventHandler(this, new MonsterSettingEvent(_monster.Level, _monster.Hp, _monster.Speed));
    }
    #endregion

    public GameObject GetMonster(int _stage)
    {
        return Resources.Load(ResourcesType.Monster.ToString() + "/Monster_" + _stage) as GameObject;
    }
}
