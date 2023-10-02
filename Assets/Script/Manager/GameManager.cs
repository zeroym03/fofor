using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerMob player;
    public BossMob boss;
    public int stage;
    public float PlayTime;
    public bool isBattel;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;
    public int enemyCntD;

    public GameObject menuPanal;
    public GameObject gamePanal;
    public GameObject gameOverPanal;

    public Text maxScore;

    public Text ScoreText;

    public Text stageText;
    public Text playTimeText;

    public Text playHealtText;
    public Text playAmmoText;
    public Text playCoinText;

    public Image weaponImage1;
    public Image weaponImage2;
    public Image weaponImage3;
    public Image weaponImageR;

    public Text enemyTextA;
    public Text enemyTextB;
    public Text enemyTextC;

    public RectTransform BossHPGroup;
    public RectTransform BossHPBar;

    public GameObject ItemShop;
    public GameObject WeaponShop;
    public GameObject StartZon;

    public Transform[] enemyZones;
    public GameObject[] enemies;
    public Text bestScoreText;
    public Text CurScoreText;
    private void Awake()
    {
        DeforteScoreSet();
    }
    void DeforteScoreSet()
    {
        if (PlayerPrefs.HasKey("MaxScore"))
        {
            PlayerPrefs.SetInt("MaxScore", 0);
        }
        maxScore.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));
    }
}
