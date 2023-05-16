using DG.Tweening;
using UnityEngine;
public class ShellFight : MonoBehaviour
{
    #region Data
    enum ShellType
    {
        sling,
        rocket,
    }
    private Transform target;
    private ShellType type;
    private bool isTest = true;
    #endregion

    #region Refresh
    public void GlobalRefresh(Transform _target, int _enum)
    {
        target = _target;
        type = (ShellType)_enum;
        LocalRefresh();
    }
    public void LocalRefresh()
    {

    }
    private void Update()
    {
        if (type == ShellType.rocket && isTest)
        {
            RocketMove();

            isTest = false;
        }
    }
    #endregion

    #region Logic
    public void RocketMove()
    {
        if (target == null)
            return;
        Vector3 _targetVec = target.position;
        Vector3 _transVec = transform.position;
        Vector3[] _pathVec = CalculateArcPoints(_transVec, _targetVec, Vector3.Distance(_transVec, _targetVec) * 0.5f, 0.1f);
        transform.DOPath(_pathVec, CalculateArcLength(_pathVec) / (10f * 1.5f), pathType: PathType.CatmullRom).SetEase(Ease.InSine).OnComplete(() =>
        {
            SceneCore.Singletons.GetVFXToScene("CFXR2 WW Enemy Explosion", _targetVec, (_game) =>
            {
                DestroyImmediate(gameObject);
            });

        });

    }
    public Vector3[] CalculateArcPoints(Vector3 initialPosition, Vector3 targetPosition, float height, float timeStep)
    {
        // 计算两点之间的距离
        float distance = Vector3.Distance(initialPosition, targetPosition);

        // 计算控制点的位置
        Vector3 controlPoint = (initialPosition + targetPosition) / 2f;
        controlPoint.y += height;

        // 计算需要的点数
        int arcPointsCount = Mathf.RoundToInt(distance / timeStep);
        Vector3[] arcPoints = new Vector3[arcPointsCount];

        // 计算每个时间步长内的弧线上的点的位置
        for (int i = 0; i < arcPointsCount; i++)
        {
            float t = (float)i / (arcPointsCount - 1);
            Vector3 point = Mathf.Pow(1 - t, 2) * initialPosition + 2 * (1 - t) * t * controlPoint + Mathf.Pow(t, 2) * targetPosition;
            arcPoints[i] = point;
        }

        return arcPoints;
    }
    public float CalculateArcLength(Vector3[] arcPoints)
    {
        float totalLength = 0f;
        Vector3 lastPoint = arcPoints[0];
        for (int i = 1; i < arcPoints.Length; i++)
        {
            float segmentDistance = Vector3.Distance(lastPoint, arcPoints[i]);
            totalLength += segmentDistance;
            lastPoint = arcPoints[i];
        }

        return totalLength;
    }
    #endregion

}
