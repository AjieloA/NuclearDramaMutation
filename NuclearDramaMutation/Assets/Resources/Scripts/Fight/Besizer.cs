using DG.Tweening;
using UnityEngine;

public class Besizer : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public float angle;
    [SerializeField]
    private float velocity;
    public float timeStep;
    public Vector3[] vector3s;
    public float speed;
    public float speedSpeed;

    private void Start()
    {
        velocity = Vector3.Distance(start.position, end.position) *0.5f;
        Vector3[] _vectors = CalculateArcPoints(start.position, end.position, velocity, timeStep);
        speedSpeed=CalculateArcLength(_vectors)/(speed*1.5f);
        transform.DOPath(_vectors, speedSpeed,pathType:PathType.CatmullRom).SetEase(Ease.InSine);
    }
    public Vector3[] CalculateArcPoints(Vector3 initialPosition, Vector3 targetPosition, float height, float timeStep)
    {
        // ��������֮��ľ���
        float distance = Vector3.Distance(initialPosition, targetPosition);

        // ������Ƶ��λ��
        Vector3 controlPoint = (initialPosition + targetPosition) / 2f;
        controlPoint.y += height;

        // ������Ҫ�ĵ���
        int arcPointsCount = Mathf.RoundToInt(distance / timeStep);
        Vector3[] arcPoints = new Vector3[arcPointsCount];

        // ����ÿ��ʱ�䲽���ڵĻ����ϵĵ��λ��
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

}
