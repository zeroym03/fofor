using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type
    {
        A,B,C,D
    }
    public Type type;

 
    public int MaxHP;
    public int CurHP;
    public Transform target;
    public BoxCollider meleeArea;
    public GameObject bullet;
    public bool isAttack;
    public bool isChase;

    MeshRenderer[] meshs;
    Rigidbody rb;
    BoxCollider boxCollider;
    NavMeshAgent agent;
    Animator animator;
    private void Awake()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        if(type != Type.D)
        Invoke("CheseStart", 2);
    }
    void Targetting()
    {
        if(type != Type.D)
        {
            float targetRadiuse = 0f;
            float targetRange = 0f;

            switch (type)
            {
                case Type.A:
                    targetRadiuse = 1.5f;
                    targetRange = 2.0f;
                    break;
                case Type.B:
                    targetRadiuse = 1.5f;
                    targetRange = 4.5f;
                    break;
                case Type.C:
                    targetRadiuse = 0.5f;
                    targetRange = 25f;
                    break;
            }
            RaycastHit[] hits = Physics.SphereCastAll(transform.position,
               targetRadiuse, transform.forward, targetRange,
               LayerMask.GetMask("Player"));
            if (hits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }
    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        animator.SetBool("isAttack", true);
        switch (type)
        {
            case Type.A:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;
                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;
                yield return new WaitForSeconds(1f);

                break;
            case Type.B:
                yield return new WaitForSeconds(0.1f);
                rb.AddForce(transform.forward * 20, ForceMode.Impulse);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rb.velocity = Vector3.zero;
                meleeArea.enabled = false;

                yield return new WaitForSeconds(3f);
                break;
            case Type.C:
                yield return new WaitForSeconds(0.5f);
                GameObject bulletObject = Instantiate(bullet,transform.position,transform.rotation);
                Rigidbody rigidbody = bulletObject.GetComponent<Rigidbody>();
                rigidbody.velocity = transform.forward * 20;
                yield return new WaitForSeconds(3f);
                break;
        }
        isChase = true;
        isAttack = false;
        animator.SetBool("isAttack", false);
    }
    void FreezeVelocity()
    {
        if(isChase)
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero; 
    }
    void CheseStart()
    {
        isChase = true;
        animator.SetBool("isWalk", true);
    }
    private void Update()
    {
        if (agent.enabled)
        {
            agent.SetDestination(target.position);
            agent.isStopped = !isChase;
            Targetting();
        }
    }
    private void FixedUpdate()
    {
        FreezeVelocity();
    }
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            CurHP -= weapon.Damege;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamege(reactVec, false));
        }
        else if (other.tag == "Bullet")
        {
            Bullet weapon = other.GetComponent<Bullet>();
            CurHP -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            if(type != Type.D)Destroy(other.gameObject);

            StartCoroutine(OnDamege(reactVec, false));
        }
      
    }
  public  void HitByGranade(Vector3 explosionPos)
    {
        CurHP -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamege(reactVec,true));
    }
    IEnumerator OnDamege(Vector3 reactVec, bool isgranade)
    {foreach(MeshRenderer Mat in meshs )
        Mat.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
      if(CurHP >0)
            foreach(MeshRenderer Mat in meshs )
                Mat.material.color = Color.white;
        else
        {
            foreach (MeshRenderer Mat in meshs)
                Mat.material.color = Color.black;
            gameObject.layer = 14;
            isChase = false;
            agent.enabled = false;
            animator.SetTrigger("Die");
            if (isgranade)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up*3;
                rb.freezeRotation = false;
                rb.AddForce(reactVec * 5, ForceMode.Impulse);
                rb.AddTorque(reactVec * 15, ForceMode.Impulse);
                yield return new WaitForSeconds(0.5f);
                Destroy(gameObject);
            }
            else
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                rb.AddForce(reactVec * 5, ForceMode.Impulse);
                yield return new WaitForSeconds(0.5f);
                Destroy(gameObject);
            }
          
        }
    }
}
