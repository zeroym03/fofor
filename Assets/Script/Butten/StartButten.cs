using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButten : MonoBehaviour
{

     Camera menuCam;
     MenuCamera gameCam;
     GameObject menuPanal;
     GameObject gamePanal;
     PlayerMob player;
    public void GameStart()
    {
       menuCam.gameObject.SetActive(false);
        gameCam.gameObject.SetActive(true);
        menuPanal.SetActive(false);
        gamePanal.SetActive(true);
        player.gameObject.SetActive(true);
    }
    private void Awake()
    {
        menuCam = GetComponent<Camera>();
    }
}
