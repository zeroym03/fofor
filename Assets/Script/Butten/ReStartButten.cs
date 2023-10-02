using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public void ReStart()
    {
        SceneManager.LoadScene(0);
    }//정보초기화 추가 아 필요없내
}
