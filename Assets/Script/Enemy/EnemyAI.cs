using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    #region private variable
    private float nextWaypointDis = 3f;
    private float disBetweenEnemyAndPlayer;
    private float RetreatDis, DetectRange, AttackDis, speed, defaultSpeed, JumpForce, CheckPointRadius;
    private int currentWaypoint;
    private bool reachEndOfWaypoint, startWalk, isPlayerinRange, isGoToPlayer, isGetAwayFromPlayer, isUseLowFly, isFlyAble, isGroundCheck, isWallCheck, isJump;
    private Seeker seeker;
    private Rigidbody2D rb2d;
    private Vector2 direc;
    private Vector2 force;
    #endregion

    #region SerializeField variable
    [SerializeField] private Path path;
    [SerializeField] private GameObject EnemyGraphic;
    [SerializeField] private Transform GroundCheckPoint,WallCheckPoint;
    [SerializeField] private Enemy Enemy;
    [SerializeField] private LayerMask LayerCheck;
    #endregion

    #region Class Property
    
    #region Get Property
    public bool GetPlayerInRange
    {
        get
        {
            return isPlayerinRange;
        }
    }
    public bool GetIsGoToPlayer
    {
        get
        {
            return isGoToPlayer;
        }
    }
    public bool GetisGetAwayFromPlayer
    {
        get
        {
            return isGetAwayFromPlayer;
        }
    }
    public float GetAttackDis
    {
        get
        {
            return AttackDis;
        }
    }
    public float GetDistanceBetweenEnemyAndPlayer
    {
        get
        {
            return disBetweenEnemyAndPlayer;
        }
    }
    #endregion
    #region Set Property
    public float SetRetreatDis
    {
        set
        {
            RetreatDis = value;
        }
    }
    public float SetDetectRange
    {
        set
        {
            DetectRange = value;
        }
    }
    public float SetAttackDis
    {
        set
        {
            AttackDis = value;
        }
    }
    public float SetSpeed
    {
        set
        {
            speed = value;
            defaultSpeed = speed;
        }
    }
    public bool SetIsUseLowFly
    {
        set
        {
            isUseLowFly = value;
        }
    }
    public bool SetIsFlyAble
    {
        set
        {
            isFlyAble = value;
        }
    }
    public float SetJumpForce
    {
        set
        {
            JumpForce = value;
        }
    }
    #endregion

    #endregion
    
    //Get all component
    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    //Prepare all setting
    void Start()
    {
        //Make path and update it
        seeker.StartPath(rb2d.position, Enemy.GetPlayerGameObject.transform.position, OnPathComplete);
        InvokeRepeating("UpdatePath", 0f, 0.5f);

        if(isFlyAble == true)
        {
            startWalk = true;
        }
    }

    void OnPathComplete(Path p)
    {
        //I don't know what the fuck is this
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    void UpdatePath()
    {
        //yeah Update Pathfinding
        if (seeker.IsDone())
        {
            seeker.StartPath(rb2d.position, Enemy.GetPlayerGameObject.transform.position, OnPathComplete);
        }
    }
    private void Update()
    {
        if(Enemy.GetisDeath == true)
        {
            this.enabled = false;
        }

        if(isFlyAble == false)
        {
            isGroundCheck = Physics2D.OverlapCircle(GroundCheckPoint.position,CheckPointRadius,LayerCheck);
            isWallCheck = Physics2D.OverlapCircle(WallCheckPoint.position,CheckPointRadius,LayerCheck);
        }
    }
    void FixedUpdate()
    {
        //I don't know what the fuck is this
        if (path == null)
        {
                return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
                reachEndOfWaypoint = true;
                return;
        }
        else
        {
            reachEndOfWaypoint = false;
        }

        //check path enemy and path waypoint & make force
        direc = ((Vector2)path.vectorPath[currentWaypoint] - rb2d.position).normalized;

        force = new Vector2(direc.x * speed ,direc.y * speed);
        

        //check distance between enemy & player
        disBetweenEnemyAndPlayer = Vector2.Distance(rb2d.position, Enemy.GetPlayerGameObject.transform.position);

        //is player in detect range
        if(disBetweenEnemyAndPlayer < DetectRange && isPlayerinRange == false)
        {
            StartFollowPlayer();
        }

        //if enemy hit ground and player in detectrage
        else if (startWalk == true && isPlayerinRange == true)
        {
            //check if it have any waypoint left
            float dis = Vector2.Distance(rb2d.position, path.vectorPath[currentWaypoint]);
            if (dis < nextWaypointDis)
            {
                currentWaypoint++;
            }

            if(isFlyAble == true && isUseLowFly == true && TimeManager.GetTimeStop() == false)
            {
                if(transform.position.y < 0f)
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x,2f);
                }
            }
            else if(isFlyAble == false)
            {
                if(isGroundCheck == false && isJump == false)
                {
                    isJump = true;
                    print("No Ground");
                    rb2d.velocity = Vector2.up * JumpForce;
                }
                else if(isWallCheck == true && isJump == false)
                {
                    isJump = true;
                    print("Have Wall");
                    rb2d.velocity = Vector2.up * JumpForce;
                }
            }

            //Rotate Graphic
            if (Enemy.GetPlayerGameObject.transform.position.x > transform.position.x && TimeManager.GetTimeStop() == false)
            {
                EnemyGraphic.transform.localRotation = Quaternion.Euler(0f,180f,0f);
            }
            else if (Enemy.GetPlayerGameObject.transform.position.x < transform.position.x && TimeManager.GetTimeStop() == false)
            {
                EnemyGraphic.transform.localRotation = Quaternion.Euler(0f,0f,0f);
            }
        }
    }

    //player in detectrange
    public void StartFollowPlayer()
    {
        isPlayerinRange = true;
    }

    //follow player
    public void GoToPlayer()
    {
        if(TimeManager.GetTimeStop() == false && Enemy.GetisDamage == false)
        {
            if(disBetweenEnemyAndPlayer > AttackDis)
            {
                isGoToPlayer = true;
                isGetAwayFromPlayer = false;
                //rb2d.AddForce(force * Time.deltaTime, ForceMode2D.Force);

                if(isFlyAble == false)
                {
                    rb2d.velocity = new Vector2(force.x, rb2d.velocity.y);
                }
                else if(isFlyAble == true)
                {
                    rb2d.velocity = new Vector2(force.x, force.y);
                }
            }
        }
    }
    public void MoveToWardToPlayer()
    {
        if(TimeManager.GetTimeStop() == false && Enemy.GetisDamage == false)
        {
            rb2d.velocity = force * 1.5f;
        }
    }
    public void StandStill()
    {
        if(disBetweenEnemyAndPlayer > DetectRange)
        {
                GoToPlayer();
        }
        else
        {
            isGoToPlayer = false;
            isGetAwayFromPlayer = false;
            rb2d.velocity = new Vector2(0f,rb2d.velocity.y);
        }
    }

    //get away from player
    public void GetAwayFromPlayer()
    {
        if(TimeManager.GetTimeStop() == false && Enemy.GetisDamage == false)
        {
            if(disBetweenEnemyAndPlayer < RetreatDis && disBetweenEnemyAndPlayer < DetectRange)
            {
                isGoToPlayer = false;
                isGetAwayFromPlayer = true;
                //rb2d.AddForce(-force * Time.deltaTime, ForceMode2D.Force);
                if(isFlyAble == false)
                {
                    rb2d.velocity = new Vector2(-force.x, rb2d.velocity.y);
                }
                else if(isFlyAble == true)
                {
                    rb2d.velocity = new Vector2(-force.x, -force.y);
                }
            }
            else if(disBetweenEnemyAndPlayer > DetectRange)
            {
                GoToPlayer();
            }
            else
            {
                StandStill();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            if(startWalk == false)
            {
                startWalk = true;
            }

            for(int i = 0; i < other.contacts.Length; i++)
            {
                if(Vector2.Angle(other.contacts[i].normal,Vector2.up) <= 30f)
                {
                    isJump = false;
                }
            }
        }
    }

    //draw all detect range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,DetectRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,RetreatDis);

        if(GroundCheckPoint != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(GroundCheckPoint.position,CheckPointRadius);
        }
        if(WallCheckPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(WallCheckPoint.position,CheckPointRadius);
        }
    }
}
