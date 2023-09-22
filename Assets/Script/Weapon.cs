using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type{
        Melee,Range
    };
    public Type type;
    public int Damege;
    public int maxAmmo;
    public int curAmmo;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffact;

    public Transform bulletpos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;
    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine(Swing());
            StartCoroutine(Swing()); 
        }
        else if (type == Type.Range&& curAmmo > 0)
        {
            curAmmo--;
            StopCoroutine(Shot());
            StartCoroutine(Shot());
        }
    }
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailEffact.enabled = true;
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(0.1f);
        trailEffact.enabled = false;
    }
    IEnumerator Shot ()
    {
        GameObject intantBullet = Instantiate(bullet, bulletpos.position, bulletpos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletpos.forward*50;

        yield return null;
        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody CaseRigid = intantCase.GetComponent<Rigidbody>();
        Vector3 casevec = 
            bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(-3, -2);
        CaseRigid.AddForce(casevec, ForceMode.Impulse);
        CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
        
    }
}
