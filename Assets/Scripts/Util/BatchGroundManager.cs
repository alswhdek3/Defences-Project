using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WayPointType
{
    None = -1,
    WayPoint_0, WayPoint_1,
    Max
}

public class BatchGroundManager : Singleton<BatchGroundManager>
{
    // 마우스 왼쪽 버튼 클릭시 자식으로 로봇 생성
    private GameObject selectWayPoint;

    [SerializeField]
    private GameObject dragShowObject;
    private SpriteRenderer wayPointSprite;

    private Sprite GetSprite(WayPointType _wayPointtype)
    {
        return Resources.Load<Sprite>($"WayPoint/{_wayPointtype}");
    }

    private GameObject GetBatchGround(Vector3 _position)
    {
        Ray2D ray = new Ray2D(_position, Vector3.zero);

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100f, 1 << LayerMask.NameToLayer("BatchWayPoint"));

        if(hit.collider != null)
            return hit.collider.gameObject;

        return null;
    }

    protected override void OnStart()
    {
        wayPointSprite = dragShowObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        dragShowObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject targetGround = GetBatchGround(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (targetGround != null)
        {
            // Draging GroundChildren Search Robot
            Robot searchRobot = targetGround.GetComponentInChildren<Robot>();
            if(searchRobot == null)
            {
                // 배치할수있는 스프라이트 이미지로 변경
                wayPointSprite.sprite = GetSprite(WayPointType.WayPoint_0);

                // 로봇 생성
                if(Input.GetMouseButtonDown(0))
                {
                    Robot robot = BatchManager.Instance.CreateRobot(BatchRobotType.Robot_0);
                    robot.transform.SetParent(targetGround.transform);
                    robot.transform.localScale = Vector2.one;
                    robot.transform.localPosition = Vector2.zero;
                }
            }
        }
        else
        {
            // 배치할수없는 스프라이트 이미지로 변경
            wayPointSprite.sprite = GetSprite(WayPointType.WayPoint_1);
        }
    }
}
