using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject effectobj;
    public GameObject mesobj;
    public Rigidbody rigid;
    void Start()
    {
        StartCoroutine(Explosion());
    }
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        mesobj.SetActive(false);
        effectobj.SetActive(true);

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 
            15, Vector3.up, 0f,
            LayerMask.GetMask("enemy")); ;

        foreach (RaycastHit hit in hits)
        {
            hit.transform.GetComponent<Enemy>().HitByGranade(transform.position);
        }

        Destroy(gameObject,5);
    }
}
