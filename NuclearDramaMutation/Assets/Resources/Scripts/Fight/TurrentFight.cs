using UnityEngine;

public class TurrentFight : MonoBehaviour
{
    public enum TurrentType
    {
        sling,
        rocket,
    }
    public enum TurrentState
    {
        Attack,
        Rotate,
    }
    #region ����
    [Header("���������������")]
    /// <summary>
    /// ����Ŀ��
    /// </summary>
    private Transform target;
    private Transform rotateTrans;
    /// <summary>
    /// ��������
    /// </summary>
    private TurrentType turrentType;
    private TurrentState turrentState = TurrentState.Rotate;
    /// <summary>
    /// �������ʱ��
    /// </summary>
    private float attackExecute;
    private GameObject ShellGame;
    private string shellPath = "";
    [Header("������ת�������")]
    private float rayRadius = 5;
    /// <summary>
    /// ������ת�Ƕ�����
    /// </summary>
    private float rotateAngle = 0f, rotateSpeed = 20f, useRotateNum = 0f;
    private Vector3 rotateImposeTrans = new Vector3(0, 135, 0);
    private Animator animator;
    private float rayRotateRange = 90;
    private int rayLineCount = 50;
    private float rayInterval = 0.0f;
    /// <summary>
    /// �Ƿ���������ת
    /// </summary>
    private bool isRight = true;

    [Header("���߼���������")]
    private Collider[] hitMonsters;
    private RaycastHit rayLineHit;
    private Vector3 rayVec;

    #endregion

    #region ��ʼ��

    private void Awake()
    {

    }
    private void Start()
    {
    }
    public void Refresh()
    {
        Init();
    }
    public void Init()
    {
        switch (turrentType)
        {
            case TurrentType.sling:
                shellPath = TypeName.ResourcesTypeName.RTurrent + "Bullet_Missle";
                break;
            case TurrentType.rocket:
                shellPath = TypeName.ResourcesTypeName.RTurrent + "Bullet_Catapult";
                break;
        }
        SceneCore.Singletons.GetPrefab(shellPath, (_object) =>
        {
            if (_object != null)
                ShellGame = _object;
        });

        rayVec = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        rayInterval = rayRotateRange / rayLineCount / 2;
        rotateTrans = transform.GetChild(0);
        animator = transform.GetChild(0).GetComponent<Animator>();
    }
    #endregion

    #region ����
    private void Update()
    {
        TurrentIdle();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, rayRadius);
    }
    #endregion

    #region �߼�����
    /// <summary>
    /// 0:Ͷʯ��
    /// 1�����Ͳ
    /// </summary>
    /// <param name="_type"></param>
    public void SetTurrentType(int _type)
    {
        turrentType = (TurrentType)_type;
        Refresh();
    }
    /// <summary>
    /// �������߼��
    /// </summary>
    public void RayDetaction()
    {
        hitMonsters = Physics.OverlapSphere(transform.position, rayRadius, 1 << 3);
        if (hitMonsters != null)
            for (int i = 0; i < hitMonsters.Length; i++)
                turrentState = TurrentState.Attack;
    }
    public void LoopRayLine()
    {
        for (int i = 0; i < rayLineCount; i++)
        {
            target = RayLine(Quaternion.Euler(0, i * rayInterval, 0), 20f) == null ? RayLine(Quaternion.Euler(0, -i * rayInterval, 0), 20f) : RayLine(Quaternion.Euler(0, i * rayInterval, 0), 20f);
            if (target != null && target.CompareTag("Monster"))
                turrentState = TurrentState.Attack;
        }
    }
    /// <summary>
    /// �������߽��м��
    /// </summary>
    /// <param name="_quaternion"></param>
    /// <param name="_rayRange"></param>
    public Transform RayLine(Quaternion _quaternion, float _rayRange)
    {
        Debug.DrawRay(rayVec, _quaternion * rotateTrans.forward.normalized * rayRadius, Color.red);
        if (Physics.Raycast(rayVec, _quaternion * rotateTrans.forward, out rayLineHit, rayRadius))
            return rayLineHit.transform;
        return null;
    }
    public void TurrentIdle()
    {
        if (turrentState == TurrentState.Rotate)
        {
            LoopRayLine();
            IdleRotate();
            //RayDetaction();
        }
    }
    /// <summary>
    /// ����Idle״̬��ת
    /// </summary>
    public void IdleRotate()
    {
        if (isRight)
        {
            if (rotateTrans.rotation == Quaternion.Euler(rotateImposeTrans))
            {
                isRight = false;
                rotateImposeTrans.y *= -1;
            }
            rotateTrans.rotation = Quaternion.RotateTowards(rotateTrans.rotation, Quaternion.Euler(rotateImposeTrans), rotateSpeed * Time.deltaTime);
        }
        else
        {
            if (rotateTrans.rotation == Quaternion.Euler(Vector3.zero))
                isRight = true;
            rotateTrans.rotation = Quaternion.RotateTowards(rotateTrans.rotation, Quaternion.Euler(Vector3.zero), rotateSpeed * Time.deltaTime);
        }
    }

    public void FightTOMonster()
    {

    }
    #endregion
}
