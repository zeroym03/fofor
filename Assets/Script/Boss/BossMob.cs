using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMob : Enemy
{
    public GameObject missaile;
    public Transform missailePortA;
    public Transform missailePortB;
    Vector3 lookVec;
    Vector3 tountVec;
  public  bool isLook ;

    void Awake()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        agent.isStopped = true;
        StartCoroutine(Think());
    }
    
    void Update()
    {
        if (isDead)
        {
            StopAllCoroutines();
            animator.SetTrigger("doDie");
            Destroy(this.gameObject);
            return;
        }
        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.position + lookVec);
        }
        else
        {
            agent.SetDestination(tountVec);
        }
    }
    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);
        int ranAction = Random.Range(0, 5);
        switch (ranAction)
        {
            case 0:
            case 1:
                StartCoroutine(MissaileShot());
                break;
            case 2:
            case 3:
                StartCoroutine(RockShot());
                break;
            case 4:
                StartCoroutine(Taunt());
                break;
            case 5:
                break;
        }
    }
    IEnumerator MissaileShot()
    {
        animator.SetTrigger("doShot");
        yield return new WaitForSeconds(0.2f);
        GameObject instantMissailA = Instantiate(missaile, missailePortA.position, missailePortA.rotation);
        BossMisail bossMisailA = instantMissailA.GetComponent<BossMisail>();
        bossMisailA.target = target;

        yield return new WaitForSeconds(0.5f);
        GameObject instantMissailB = Instantiate(missaile, missailePortB.position, missailePortB.rotation);
        BossMisail bossMisailB = instantMissailB.GetComponent<BossMisail>();
        bossMisailB.target = target;
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(Think());
    }
    IEnumerator RockShot()
    {
        animator.SetTrigger("doBigShot");
        isLook = false;
        Instantiate(bullet,transform.position,transform.rotation);
        yield return new WaitForSeconds(3f);
        isLook = true;
        StartCoroutine(Think());
    }
    IEnumerator Taunt()
    {
        animator.SetTrigger("doTaunt");
        tountVec = target.position + lookVec;
        isLook = false;
        agent.isStopped = false;
        boxCollider.enabled = false;
        yield return new WaitForSeconds(1.5f);

        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;
        isLook = true;
        boxCollider.enabled = true;
        yield return new WaitForSeconds(3f);
        agent.isStopped = true;

        StartCoroutine(Think());
    }
}
