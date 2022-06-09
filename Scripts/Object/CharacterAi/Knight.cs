using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
==============================
 * ÃÖÁ¾¼öÁ¤ÀÏ : 2022-06-06
 * ÀÛ¼ºÀÚ : JSJ
 * ÆÄÀÏ¸í : Knight.cs
==============================
*/

public class Knight : MonoBehaviour
{
    [Header("Type")]
    [SerializeField] private int team = 0;
    public int Team { get { return team; } set { team = value; } }

    [Header("Status")]
    [SerializeField] 
    private float reactionRate = 0.5f;
    [SerializeField] 
    private float seeDistance = 300f;
    [SerializeField]
    private float defendProbability = 30f;
    [SerializeField]
    private float counterProbability = 30f;
    [SerializeField]
    private float attackProbability = 30f;
    [SerializeField] 
    private float tauntrobability = 10f;
    [SerializeField] 
    private float attackRange = 1.5f;
    [SerializeField]
    private float combatRange = 5f;
    [SerializeField]
    private float runSpeed = 3.5f;
    [SerializeField]
    private float walkSpeed = 1.5f;
    [SerializeField] 
    private int maxHp = 3;

    [Header("Reference Component")]
    [SerializeField] private KnightInformation informaton = null;
    public KnightInformation Information { get { return informaton; } }
    [SerializeField]
    private Transform attackPos = null;
    [SerializeField]
    private Transform cameraHolder = null;
    [SerializeField] 
    private TextMesh stateText = null;
    [SerializeField]
    private Collider ownWeapon = null;
    [SerializeField]
    private Collider ownShield = null;
    [SerializeField] 
    private Transform rightHandTransfrom = null;

    [Header("Debug")]
    [SerializeField]
    private bool isControllerable = false;
    [SerializeField] 
    private bool isRun = false;
    [SerializeField] 
    private bool debugMode = false;
    [SerializeField] 
    private EAIState state = EAIState.Idle;
    [SerializeField]
    private bool doSomething = false;
    [SerializeField]
    private bool isJump = false;
    [SerializeField]
    private Knight target = null;

    [SerializeField]
    private int hp;
    [SerializeField] 
    private int threat = 0;
    [SerializeField]
    private int killCount = 0;
    public int KillCount { get { return killCount; } }

    private Stack<Knight> targets = new Stack<Knight>();
    private Knight closestTarget = null;
    private Arrow currentArrow = null;
    private ObjectPool arrowPool = null;
    private float distFromClosetTarget = 100f;


    public event System.Action onDie = null;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        informaton = GetComponent<KnightInformation>();
        nav = GetComponentInParent<NavMeshAgent>();
        rb = GetComponentInParent<Rigidbody>();
        layerMask = 1 << LayerMask.NameToLayer("Targetable");
    }

    private void OnEnable()
    {
        onDie = null;

        state = EAIState.Idle;
        doSomething = false;
        isControllerable = false;
        target = null;
        threat = 0;
        killCount = 0;
        distFromClosetTarget = 10000f;

        reactionRate += Random.Range(-0.05f, 0.05f);
        waitReaction = new WaitForSeconds(reactionRate);
        arrowPool = GameObject.FindObjectOfType<ArrowPool>();
        hp = maxHp;
        killCount = 0;
        nav.enabled = false;
        anim.enabled = true;

        if (anim.runtimeAnimatorController != null)
            anim.Play("NoCombat", 0);

        informaton.KnightType = EKnightType.Default;

        rb.isKinematic = true;
        foreach (Collider collider in GetComponentsInParent<Collider>())
            collider.enabled = true;
    }




    private void Update()
    {
        if (isControllerable)
        {
            MoveControl();
            AttackControl();
            DefendControl();
            JumpControl();
        }
        //Debug
        if (Input.GetKeyDown(KeyCode.Tab))
            debugMode = !debugMode;


        if (state == EAIState.Die) return;
        if (debugMode && target != null)
            Debug.DrawLine(transform.position, target.transform.position, Color.blue);
    }


    public void InitializeKnightInfomation(EKnightType _type)
    {
        informaton.KnightType = _type;


        string animFilePath = "Animations\\Knight_" + informaton.KnightType.ToString() + "_Controller";
        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(animFilePath);

        if (informaton.KnightType != EKnightType.Bow)
        {
            ownWeapon = GetComponentInChildren<WeaponHolder>()
                .GetActiveObject().GetComponent<Collider>();
        }

        anim.SetBool("Combat", true);

        switch (informaton.KnightType)
        {
            case EKnightType.Sword:
                combatRange = 5f;
                attackRange = 2f;
                break;
            case EKnightType.Spear:
                combatRange = 7.5f;
                attackRange = 2.5f;
                break;
            case EKnightType.Bow:
                combatRange = 32f;
                attackRange = 30f;
                break;
        }
    }

    public void StartBattle()
    {
        state = EAIState.CombatIdle;
        rb.isKinematic = false;
        nav.enabled = true;
        anim.SetBool("Combat", true);
        StartCoroutine("StateCoroutine");
    }



    #region "User Controll"
    public void ToggleControll()
    {
        StopAllCoroutines();
        isControllerable = !isControllerable;
        nav.enabled = !isControllerable;
        anim.SetInteger("State", 1);
        doSomething = false;
        rb.isKinematic = false;

        if (!isControllerable)
        {
            state = EAIState.CombatIdle;
            StartCoroutine("StateCoroutine");
        }
    }


    public Transform GetCamHolder()
    {
        return cameraHolder;
    }

    private void FixedUpdate()
    {
        if (!isControllerable) return;

        //TODO : ½ºÇÇ¾îÄ³½ºÆ®?
        Debug.DrawRay(transform.position + Vector3.up, transform.forward * 10f, Color.blue);
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 100f, layerMask))
        {
            target = hit.collider.GetComponentInChildren<Knight>();
        }
    }

    private void MoveControl()
    {
        if (doSomething) return;

        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
            isRun = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            isRun = false;

        float gravityY = rb.velocity.y;
        rb.velocity = (transform.parent.forward * ((isRun) ? 3.5f : 1.5f) * vAxis)
            + (transform.parent.right * ((isRun) ? 3.5f : 1.5f) * hAxis);

        rb.velocity = new Vector3(rb.velocity.x, gravityY, rb.velocity.z);

        Debug.DrawRay(transform.position + Vector3.right * 0.1f, rb.velocity * 5f, Color.red);

        float yRot = Input.GetAxis("Mouse X") * 2f;
        Vector3 curRot = transform.parent.localRotation.eulerAngles;
        float y = curRot.y + yRot;

        y = Mathf.Clamp(y, -360f, 360f);
        transform.parent.localRotation = Quaternion.Euler(curRot.x, y, curRot.z);

        state = EAIState.Forward;
        anim.SetFloat("Speed", rb.velocity.magnitude);
    }

    private void AttackControl()
    {
        if (doSomething) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            doSomething = true;


            if (target != null && target.hp <= 1)
            {
                anim.SetInteger("AttackNum", 0);
                target.GetFinishAttack(this);
            }
            else
            {
                OnAttackCollider();
                int num = Random.Range(1, 3);
                anim.SetInteger("AttackNum", num);

                if (target != null && informaton.KnightType != EKnightType.Bow)
                    target.Defend(attackPos, transform);
            }


            anim.SetInteger("State", 5);
            state = EAIState.Attack;
            Debug.DrawRay(transform.position + Vector3.right * 0.2f, Vector3.up * 5f, Color.blue, 1f);
        }
    }

    private void DefendControl()
    {
        if (doSomething) return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            doSomething = true;
            anim.SetInteger("State", 6);
            anim.SetTrigger("Defend");
            state = EAIState.Defend;
        }
    }

    private void JumpControl()
    {
        if (doSomething || isJump) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            doSomething = true;
            anim.SetInteger("State", (int)EAIState.Jump);
            rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        }


    }

    public void Landing()
    {
        isJump = false;
        anim.SetInteger("State", (int)EAIState.CombatIdle);
    }
    #endregion


    #region "AI"
    private IEnumerator StateCoroutine()
    {
        while (true)
        {
            yield return waitReaction;
            if (doSomething)
            {
                yield return waitReaction;
                continue;
            }

            curTarget = target;
            target = See();
            if (target == null)
            {
                yield return null;
                continue;
            }

            if (curTarget == null && target != null)
            {
                target.threat++;
            }
            else if (curTarget != null && curTarget != target)
            {
                target.threat++;
                curTarget.threat--;
            }
            state = target.GetNextState(this);

            switch (state)
            {
                case EAIState.Idle:
                    break;
                case EAIState.CombatIdle:
                    StopMove();
                    break;
                case EAIState.Sprint:
                    Move(target.transform, runSpeed, attackRange);
                    break;
                case EAIState.Forward:
                    Move(target.transform, walkSpeed, attackRange);
                    break;
                case EAIState.Strafe:
                    doSomething = true;
                    StopMove();
                    Strafe();
                    break;
                case EAIState.Attack:
                    doSomething = true;
                    StopMove();
                    Attack();
                    break;
                case EAIState.Defend:
                    break;
                case EAIState.Eavade:
                    doSomething = true;
                    StopMove();
                    break;
                case EAIState.Counter:
                    doSomething = true;
                    StopMove();
                    break;
                case EAIState.Taunt:
                    doSomething = true;
                    StopMove();
                    Taunt();
                    break;
                case EAIState.FinishAttack:
                    doSomething = true;
                    StopMove();
                    break;
                case EAIState.Run:
                    doSomething = true;
                    break;
                case EAIState.GetHit:
                    break;
                case EAIState.Die:
                    break;
            }
            
        }
    }

    private Knight See()
    {
        targetInViewRadius = Physics.OverlapSphere(transform.position, seeDistance, layerMask);

        if (targetInViewRadius.Length == 0) return null;

        for (int i = 0; i < targetInViewRadius.Length; i++)
        {
            Transform targetCandidate = targetInViewRadius[i].transform;
            Vector3 dirToTarget = (targetCandidate.position - transform.position).normalized;

            Knight seenTarget = targetCandidate.GetComponentInChildren<Knight>();
            if (seenTarget != null && seenTarget.team != team && seenTarget.state != EAIState.Die)
            {
                distFromClosetTarget = GetClosetTargetDistance();

                float distance = Vector3.Distance(transform.position, seenTarget.transform.position) + seenTarget.threat;
                if (!targets.Contains(seenTarget) && distance < distFromClosetTarget)
                    targets.Push(seenTarget);
            }
        }

        if (targets.Count > 0)
            return targets.Peek();
        else
            return null;
    }

    private void Move(Transform _tr, float _speed, float _stopDistance)
    {
        rb.isKinematic = true;

        if (nav.isOnNavMesh)
        {
            nav.SetDestination(_tr.position);
            nav.isStopped = false;
            nav.stoppingDistance = _stopDistance;

            float distance = Vector3.Distance(_tr.position, transform.parent.position);
            if (distance >= _stopDistance * 2f)
                nav.speed = _speed;
            else
                nav.speed = _speed * ((distance - _stopDistance) / _stopDistance);

            StopCoroutine("InterpolationSpeed");
            StartCoroutine("InterpolationSpeed", nav.speed);
        }
        anim.SetInteger("State", 2);
    }

    private void Strafe()
    {
        transform.parent.LookAt(target.transform);
        anim.SetInteger("State", 2);
        StopCoroutine("InterpolationSpeed");
        StartCoroutine("InterpolationSpeed", -1.5f);
        StartCoroutine("StrafeCoroutine");
    }

    private IEnumerator StrafeCoroutine()
    {
        rb.isKinematic = false;
        float rand = Random.Range(0.5f, 1f);
        float elpased = 0f;

        while (elpased < rand)
        {
            elpased += Time.deltaTime;
            rb.velocity = new Vector3(
                -transform.forward.x * walkSpeed,
                rb.velocity.y,
                -transform.forward.z * walkSpeed);

            yield return null;
        }
        doSomething = false;
        StopMove();
    }

    private void Attack()
    {
        transform.parent.LookAt(target.transform);

        if (target.hp <= 1 && informaton.KnightType != EKnightType.Bow)
        {
            anim.SetInteger("AttackNum", 0);
            target.GetFinishAttack(this);
        }
        else
        {
            OnAttackCollider();
            int num = Random.Range(1, 4);
            anim.SetInteger("AttackNum", num);
            

            if (informaton.KnightType != EKnightType.Bow)
                target.Defend(attackPos, transform);
        }
        anim.SetInteger("State", 5);
    }

    public void Defend(Transform _defendPos, Transform _attacker)
    {
        OffAttackCollider();

        if (isControllerable) return;
        if (!CalcProbability(defendProbability)) return;

        transform.parent.LookAt(new Vector3(_attacker.position.x, transform.position.y, _attacker.position.z));
        StopMove();
        doSomething = true;

        anim.SetTrigger("Defend");

        state = EAIState.Defend;
    }


    private void Taunt()
    {
        transform.parent.LookAt(target.transform);

        int num = Random.Range(1, 8);
        anim.SetInteger("TauntNum", num);
    }

    private void StopMove()
    {
        if (nav.isOnNavMesh)
        {
            nav.isStopped = true;
            nav.stoppingDistance = 0f;
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
        }

        StopCoroutine("InterpolationSpeed");
        if (gameObject.activeInHierarchy)
            StartCoroutine("InterpolationSpeed", 0f);
    }
    #endregion

    public void Die()
    {
        StopAllCoroutines();
        if (target != null) target.threat--;

        onDie?.Invoke();

        rb.isKinematic = true;

        Invoke("OffAnimator", 2.5f);

        nav.enabled = false;
        foreach (Collider collider in GetComponentsInParent<Collider>())
            collider.enabled = false;

        state = EAIState.Die;
    }

    private void OffAnimator()
    {
        anim.enabled = false;
    }

    public void ShootArrow()
    {
        GameObject arrowGo = arrowPool.GetObject();
        arrowGo.transform.position = rightHandTransfrom.position;
        arrowGo.transform.rotation = transform.parent.rotation;
        currentArrow = arrowGo.GetComponent<Arrow>();
        float dist = Vector3.Distance(transform.position, target.transform.position);
        currentArrow.Shoot(-transform.right, dist, this);
    }


    private EAIState GetNextState(Knight _other)
    {
        if (_other.state == EAIState.Die) return EAIState.Die;

        if (InRange(transform, _other.transform, _other.combatRange))
        {
            switch (state)
            {
                case EAIState.Idle:
                    return EAIState.Idle;
                case EAIState.CombatIdle:
                case EAIState.Sprint:
                case EAIState.Forward:
                case EAIState.Strafe:
                case EAIState.Taunt:
                case EAIState.Run:
                case EAIState.Attack:
                    if (InRange(transform, _other.transform, _other.attackRange))
                    {
                        if (IsOpposite(_other.transform.forward))
                        {
                            if (CalcProbability(_other.attackProbability))
                                return EAIState.Attack;
                            else if (CalcProbability(10f * threat))
                                return EAIState.Strafe;
                            else
                                return EAIState.CombatIdle;
                        }
                        else
                        {
                            if (CalcProbability(_other.attackProbability * 2f))
                                return EAIState.Attack;
                            else
                                return EAIState.Forward;
                        }
                    }
                    else
                    {
                        return EAIState.Forward;
                    }

                case EAIState.Defend:
                case EAIState.Eavade:
                case EAIState.Counter:
                case EAIState.FinishAttack:
                case EAIState.GetHit:
                    return EAIState.CombatIdle;
                case EAIState.Die:
                    return EAIState.CombatIdle;
            }
        }
        else
            return EAIState.Sprint;
        return EAIState.CombatIdle;
    }

    public void DoneSomething()
    {
        OffAttackCollider();

        if (state == EAIState.Die) return;

        state = EAIState.CombatIdle;
        anim.SetInteger("AttackNum", 0);
        doSomething = false;
        anim.SetInteger("State", 1);
    }

    public void EndBattle()
    {
        StopMove();
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (state == EAIState.Die || state == EAIState.GetHit || state == EAIState.Defend) return;
        if (_other == ownWeapon || _other == ownShield) return;
        if (_other.CompareTag("Attackable"))
        {
            GetHit(0, _other.GetComponentInParent<Knight>());
        }

        if (_other.CompareTag("Arrow"))
        {
            Arrow arrow = _other.GetComponent<Arrow>();
            if (arrow != currentArrow)
                GetHitArrow(arrow);
        }
    }

    private void GetHit(int _num, Knight _attacker)
    {
        if (_attacker.team == team) return;

        doSomething = true;
        StopMove();

        hp--;

        //Todo : ±¦Âú´×
        if (hp <= 0)
        {
            anim.SetInteger("HitNum", 5);
            anim.SetTrigger("GetHit");
            Die();
            _attacker.killCount++;
        }
        else
        {
            _attacker.OffAttackCollider();
            transform.parent.LookAt(_attacker.transform);

            anim.SetInteger("HitNum", _num);
            anim.SetTrigger("GetHit");
            state = EAIState.GetHit;
        }
    }

    public void GetFinishAttack(Knight _attacker)
    {
        doSomething = true;

        transform.parent.LookAt(_attacker.transform);
        anim.SetInteger("HitNum", 5);
        anim.SetTrigger("GetHit");
        stateText.transform.position += Vector3.down * 5f;

        hp = 0;
        Die();
        _attacker.killCount++;
    }

    private void GetHitArrow(Arrow _arrow)
    {
        StopMove();
        doSomething = true;
        hp--;

        //Todo : ±¦Âú´×
        if (hp <= 0)
        {
            anim.SetInteger("HitNum", 5);
            anim.SetTrigger("GetHit");
            Die();
            _arrow.GetShooter().killCount++;
        }
        else
        {
            transform.parent.LookAt(new Vector3(_arrow.transform.position.x, transform.position.y, _arrow.transform.position.z));
            anim.SetInteger("HitNum", 0);
            anim.SetTrigger("GetHit");
            state = EAIState.GetHit;
        }
    }

    private float GetClosetTargetDistance()
    {
        if (targets.Count > 0)
            while ((closestTarget = targets.Peek()) == null || closestTarget.state == EAIState.Die)
            {
                targets.Pop();
                if (targets.Count == 0)
                    return 10000f;
            }
        else
            return 10000f;

        return Vector3.Distance(transform.position, closestTarget.transform.position) + (closestTarget.threat);
    }

    private bool IsOpposite(Vector3 _otherFace)
    {
        float angle = Vector3.Dot(transform.forward, _otherFace);
        return (angle < 0f);
    }

    private bool InRange(Transform _lhs, Transform _rhs, float _range)
    {
        float distance = Vector3.Distance(_lhs.position, _rhs.position);
        return (distance <= _range);
    }

    private IEnumerator InterpolationSpeed(float _speed)
    {
        float elpased = 0f;

        while ((elpased * 4f) <= 1f)
        {
            elpased += Time.deltaTime;

            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), _speed, elpased * 4f));
            yield return null;
        }
    }

    private bool CalcProbability(float _prob)
    {
        float rand = Random.Range(0f, 100f);
        return (rand <= _prob);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnAttackCollider() { if (ownWeapon) ownWeapon.enabled = true; }
    private void OffAttackCollider() { if (ownWeapon) ownWeapon.enabled = false; }



    #region "Component"
    private Animator anim = null;
    private NavMeshAgent nav = null;
    private Rigidbody rb = null;
    #endregion

    #region "For Performance"
    private Knight curTarget = null;
    private WaitForSeconds waitReaction = null;
    private int layerMask = 0;
    private Collider[] targetInViewRadius;
    private RaycastHit hit;
    #endregion
}
