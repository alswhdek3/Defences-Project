using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    protected Animator animator;

    protected int level;
    protected int hp;
    protected float speed;
    protected int currentWayPoint;

    public int Level { get { return level; } }
    public int Hp { get { return hp; } }
    public float Speed { get { return speed; } }

    private IEnumerator Coroutine_Move()
    {
        while(true)
        {
            // 몬스터가 활성화 될때까지 대기
            yield return new WaitUntil(() =>
            {
                return gameObject.activeInHierarchy;
            });

            if (currentWayPoint < GameManager.Instance.WayPointLength)
            {
                Vector3 wayPointPosition = GameManager.Instance.GetWayPointPosition(currentWayPoint);
                Vector3 dir = wayPointPosition - transform.position;
                transform.position += dir.normalized * speed * Time.deltaTime;

                if (dir.magnitude < 0.1f)
                {
                    transform.position = GameManager.Instance.GetWayPointPosition(currentWayPoint);
                    yield return new WaitForSeconds(0.01f);
                    currentWayPoint++;
                }

                animator.SetFloat("Horizontal", dir.x);
                animator.SetFloat("Vertical", dir.y);
            }

            // 몬스터 삭제
            else
            {
                animator.SetTrigger("Die");
                if (Handcuffs.Instance.monsterList.Contains(this))
                    Handcuffs.Instance.monsterList.Remove(this);

                yield break;
            }

            yield return null;
        }
    }

    private void AnimEvent_Die()
    {
        MonsterManager.Instance.RemoveMonster(this);
    }

    protected void InitAwake()
    {
        // GetComponent 초기화
        animator = GetComponent<Animator>();
    }
    public void SetMonster(int _level, int _hp, float _speed)
    {
        level = _level;
        hp = _hp;
        speed = _speed;
        currentWayPoint = 0; // WayPoint 시작지점 0으로 설정
    }

    private void Awake()
    {
        InitAwake();
    }
    private void OnEnable()
    {
        StartCoroutine(Coroutine_Move());
    }
}
