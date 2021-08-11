using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public Rigidbody2D rigid;
    public Animator anim;
    [SerializeField] Transform sensorGround;
    [SerializeField] Transform sensorWall;
    [SerializeField] Transform sensorAttack;

    [Header("Sensors")]
    [SerializeField] bool isTouchGround;
    [SerializeField] bool isTouchWall;
    [SerializeField] bool isPlayerRange;
    [SerializeField] bool isRangeAttack;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask playerMask;
    [SerializeField] float groundRadius;
    [SerializeField] float wallRadius;
    [SerializeField] float chaseDistance;
    [SerializeField] float attackRadius;

    [Header("")]
    public bool GizmosGround;
    public bool GizmosWall;
    public bool GizmosChase;
    public bool GizmosAttack;

    [Header("Patrol System")]
    [SerializeField] bool canPatrol;
    [SerializeField] bool isPatrol;
    [SerializeField] bool isFlip;
    [SerializeField] float speedPatrol;
    [SerializeField] float delayPatrol;
    float delayPatrolCur;

    [Header("Chase System")]
    [SerializeField] bool canChase;
    [SerializeField] bool isChase;
    [SerializeField] float speedChase;
    [SerializeField] float delayChase;
    float delayChaseCur;

    [Header("Combat System")]
    [SerializeField] bool canAttack;
    [SerializeField] bool isAttack;
    [SerializeField] float delayAttack;
    [SerializeField] bool anyDamage;
    float delayAttackCur;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ReadSensors();
        ReadPatrol();
        ReadChase();
        anim.SetBool("patrol", isPatrol);
        anim.SetBool("chase", isChase);
        anim.SetBool("attack", isAttack);
    }
    private void ReadSensors()
    {
        isTouchGround = Physics2D.OverlapCircle(sensorGround.position, groundRadius);
        isTouchWall = Physics2D.OverlapCircle(sensorWall.position, wallRadius);
        isPlayerRange = Physics2D.Raycast(transform.position, (isFlip) ? Vector2.right : Vector2.left, chaseDistance, playerMask);
        if(isPlayerRange)
            isRangeAttack = Physics2D.OverlapCircle(sensorAttack.position, attackRadius);
    }
    private void ReadPatrol()
    {
        if (canPatrol && isPatrol)
        {
            rigid.velocity = new Vector2((isFlip) ? speedPatrol : -speedPatrol, rigid.velocity.y);
            if (isTouchWall || !isTouchGround)
            {
                delayPatrolCur = delayPatrol;
                isPatrol = false;
                rigid.velocity = Vector2.zero;
            }
            if (isPlayerRange)
            {
                delayChaseCur = delayChase;
                isPatrol = false;
                rigid.velocity = Vector2.zero;
            }
        }

        if (delayPatrolCur > 0)
        {
            delayPatrolCur -= Time.deltaTime;
            if (delayPatrolCur <= 0)
            {
                isPatrol = true;
                if (!isPlayerRange)
                    ChangeDirection();
                delayPatrolCur = 0;
            }
        }
    }
    private void ReadChase()
    {
        if (canChase && isChase)
        {
            rigid.velocity = new Vector2((isFlip) ? speedChase : -speedChase, rigid.velocity.y);
            if (isRangeAttack && delayAttackCur <= 0)
            {
                isRangeAttack = false;
                isChase = false;
                isAttack = true;
                rigid.velocity = Vector2.zero;
                delayAttackCur = delayAttack;
            }
            if (isTouchWall)
            {
                delayPatrolCur = delayPatrol;
                isChase = false;
                rigid.velocity = Vector2.zero;
            }
        }
        if (delayChaseCur > 0)
        {
            delayChaseCur -= Time.deltaTime;
            if (delayChaseCur <= 0)
            {
                isChase = true;
                delayChaseCur = 0;
            }
        }
        if (delayAttackCur > 0)
        {
            delayAttackCur -= Time.deltaTime;
            if (delayAttackCur <= 0)
            {
                delayAttackCur = 0;
                isAttack = false;
                delayChaseCur = delayChase;
            }
        }
    }
    private void ChangeDirection()
    {
        if (transform.eulerAngles.y == 0)
        {
            //Debug.Log("giro izquierda");
            transform.eulerAngles = new Vector3(0, 180, 0);
            isFlip = false;
        }
        else
        {
            //Debug.Log("giro derecha");
            transform.eulerAngles = new Vector3(0, 0, 0);
            isFlip = true;
        }
    }
    private void OnDrawGizmos()
    {
        if (GizmosGround)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(sensorGround.position, groundRadius);
        }
        if (GizmosWall)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(sensorWall.position, wallRadius);
        }
        if (GizmosChase)
        {
            Debug.DrawRay(transform.position, (isFlip) ? Vector2.right * chaseDistance : Vector2.left * chaseDistance, Color.green);
        }
        if (GizmosAttack)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(sensorAttack.position, attackRadius);
        }
    }
}
