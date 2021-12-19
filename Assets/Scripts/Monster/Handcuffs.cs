using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handcuffs : Singleton<Handcuffs>
{
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private int bulletCount;

    [SerializeField]
    private float attackMaxdistance;

    [SerializeField]
    private Monster monster;

    [HideInInspector]
    public List<Monster> monsterList = new List<Monster>();

    protected override void OnAwake()
    {
        StartCoroutine(Coroutine_Rotate());

        IEnumerator Coroutine_Rotate()
        {
            Func<bool> IsMonsterListCount = () =>
            {
                if (monsterList.Count > 0)
                    return true;
                else
                    return false;
            };
            float closestDistSqr = Mathf.Infinity;

            while(true)
            {
                // 몬스터 리스트가 1개이상일때까지 대기
                yield return new WaitUntil(IsMonsterListCount);

                if(monster == null)
                {
                    for (int i = 0; i < monsterList.Count; i++)
                    {
                        float distance = Vector3.Distance(monsterList[i].transform.position, transform.position);
                        if (distance <= attackMaxdistance && distance <= closestDistSqr)
                        {
                            closestDistSqr = distance;
                            monster = monsterList[i];
                        }
                    }
                }

                yield return null;
            }
        }

        // 몬스터 공격에대한 사정거리 체크
        StartCoroutine(Coroutine_MonsterAttackDistanceCheck());
        IEnumerator Coroutine_MonsterAttackDistanceCheck()
        {
            while(true)
            {
                yield return new WaitUntil(() =>
                {
                   return monster != null;
                });

                if(Vector2.Distance(monster.transform.position , transform.position) > attackMaxdistance)
                {
                    monster = null;
                    transform.rotation = Quaternion.identity;
                }

                yield return null;
            }
        }
    }

    private void Update()
    {
        if (monster != null)
            RotateToTarget();
    }

    //private IEnumerator Start()
    //{
    //    // 360f 공격
    //    float attackRate = 0.5f;
    //    float plusAngle = 0f;

    //    float initAngle = 360f / bulletCount;

    //    while (true)
    //    {
    //        yield return new WaitForSeconds(attackRate);

    //        for (int i = 0; i < bulletCount; i++)
    //        {
    //            GameObject obj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    //            float angle = plusAngle + (initAngle * i);
    //            float x = Mathf.Cos(angle * Mathf.PI / 180f);
    //            float y = Mathf.Sin(angle * Mathf.PI / 180f);

    //            Vector2 direction = new Vector2(x, y);
    //            Rigidbody2D bullet = obj.GetComponent<Rigidbody2D>();
    //            bullet.velocity = direction * bulletSpeed;
    //        }

    //        plusAngle += 1f;
    //    }

    //     //샷건 공격
    //    float initAngle = 30f;
    //    float gap = initAngle / bulletCount - 1;
    //    float startAngle = -initAngle / gap;

    //    while(true)
    //    {
    //        yield return new WaitForSeconds(0.2f);
    //        for(int i=0; i<bulletCount; i++)
    //        {
    //            float angle = startAngle + (gap * i);
    //            angle *= Mathf.Deg2Rad;

    //            GameObject obj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    //            Rigidbody2D bullet = obj.GetComponent<Rigidbody2D>();

    //            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    //            bullet.velocity = -direction * bulletSpeed;
    //        }
    //    }
    //}

    private void RotateToTarget()
    {
        float speed = 2f;
        float currentTime = 0f;
        float percent = 0f;

        float x = monster.transform.position.x - transform.position.x;
        float y = monster.transform.position.y - transform.position.y;

        float degree = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        Quaternion startAngle = transform.rotation;
        Quaternion angle = Quaternion.Euler(0, 0, degree);

        currentTime += Time.deltaTime;
        percent = currentTime / speed;
        transform.rotation = Quaternion.Lerp(startAngle, angle, percent);
    }
}
