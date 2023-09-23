using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : Bullet
{
    Rigidbody body;
    float angularPawer; 
    float scalevalue;
    bool isShoot = false;
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        StartCoroutine(GainPowerTimer());
        StartCoroutine(GainPower());
    }
    IEnumerator GainPowerTimer()
    {
yield return new WaitForSeconds(2.2f);
        isShoot = true;
    }
    IEnumerator GainPower()
    {
        while (!isShoot)
        {
            angularPawer += 0.004f;
            scalevalue += 0.0005f;
            transform.localScale = Vector3.one * scalevalue;
            body.AddTorque(transform.right * angularPawer, ForceMode.Acceleration);
            yield return null;
        }
    }
}
