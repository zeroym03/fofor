using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int MaxHP;
    public int CurHP;
    Material Mat;
    Rigidbody rb;
    BoxCollider boxCollider;
    private void Awake()
    {
        Mat = GetComponent<MeshRenderer>().material;
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
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
            Destroy(other.gameObject);
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
    {
        Mat.color = Color.red;
        yield return new WaitForSeconds(0.2f);
      if(CurHP >0)  Mat.color = Color.white;
        else
        {
            Mat.color = Color.black;
            gameObject.layer = 14;
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
