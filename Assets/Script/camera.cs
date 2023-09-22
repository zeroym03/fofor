using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public Transform target;
    public Vector3 offest;
    void Update()
    {
        transform.position = target.position + offest;
    }
}
