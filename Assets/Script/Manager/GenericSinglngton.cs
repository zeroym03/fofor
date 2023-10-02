using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSinglngton<T> where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject temp = new GameObject();
                _instance = temp.AddComponent<T>();
                Object.DontDestroyOnLoad(temp);
            }
            return _instance;
        }
    }
}
