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
    #region 数据
    [Header("炮塔攻击相关数据")]
    /// <summary>
    /// 攻击目标
    /// </summary>
    private Transform target;
    private Transform rotateTrans;
    /// <summary>
    /// 炮塔类型
    /// </summary>
    private TurrentType turrentType;
    private TurrentState turrentState = TurrentState.Rotate;
    /// <summary>
    /// 攻击间隔时间
    /// </summary>
    private float attackExecute;
    private GameObject ShellGame;
    private string shellPath = "";
    [Header("炮塔旋转相关数据")]
    private float rayRadius = 5;
    /// <summary>
    /// 炮塔旋转角度限制
    /// </summary>
    private float rotateAngle = 0f, rotateSpeed = 20f, useRotateNum = 0f;
    private Vector3 rotateImposeTrans = new Vector3(0, 135, 0);
    private Animator animator;
    private float rayRotateRange = 90;
    private int rayLineCount = 50;
    private float rayInterval = 0.0f;
    /// <summary>
    /// 是否正方向旋转
    /// </summary>
    private bool isRight = true;

    [Header("射线检测相关数据")]
    private Collider[] hitMonsters;
    private RaycastHit rayLineHit;
    private Vector3 rayVec;

    #endregion

    #region 初始化

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

    #region 更新
    private void Update()
    {
        TurrentIdle();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, rayRadius);
    }
    #endregion

    #region 逻辑代码
    /// <summary>
    /// 0:投石器
    /// 1：火箭筒
    /// </summary>
    /// <param name="_type"></param>
    public void SetTurrentType(int _type)
    {
        turrentType = (TurrentType)_type;
        Refresh();
    }
    /// <summary>
    /// 球形射线检测
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
    /// 发射射线进行检测
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
    /// 炮塔Idle状态旋转
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
