using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject menuCam;
    public GameObject gameCam;
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
    public List<int> enemyList;
    public Text bestScoreText;
    public Text CurScoreText;
    private void Awake()
    {
        enemyList = new List<int>();
        maxScore.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));
        if (PlayerPrefs.HasKey("MaxScore"))
        {
            PlayerPrefs.SetInt("MaxScore", 0);
        }
    }
    public void GameStart()
    {
        menuCam.SetActive(false);
        gameCam.SetActive(true);
        menuPanal.SetActive(false);
        gamePanal.SetActive(true);
        player.gameObject.SetActive(true);
    }
    public void GameOver()
    {
        gamePanal.SetActive(false);
        gameOverPanal.SetActive(true);
        int maxScore = PlayerPrefs.GetInt("MaxScore");
        if(player.score > maxScore)
        {
            bestScoreText.gameObject.SetActive(true);
            PlayerPrefs.SetInt("MaxScore", player.score);
        }
    }
    public void ReStart()
    {
        SceneManager.LoadScene(0);
    }
    private void Update()
    {
        if (isBattel)
        {
            PlayTime += Time.deltaTime;
        }
    }
    public void StageStart()
    {
        ItemShop.SetActive(false);
        WeaponShop.SetActive(false);
        StartZon.SetActive(false);

        foreach (Transform zone in enemyZones)
        {
            zone.gameObject.SetActive(true);
        }
        isBattel = true;
        StartCoroutine(inBattel());
    }
    IEnumerator inBattel()
    {
        if (stage % 5 == 0)
        {
            enemyCntD++;
            GameObject instantenemy = Instantiate(enemies[3], enemyZones[0].position, enemyZones[0].rotation);
            Enemy enemy = instantenemy.GetComponent<Enemy>();
            enemy.target = player.transform;
            enemy.manager = this;
            boss = instantenemy.GetComponent<BossMob>();
        }
        else
        {
            for (int i = 0; i < stage; i++)
            {
                int ran = Random.Range(0, 3);
                enemyList.Add(ran);
                switch (ran)
                {
                    case 0:
                        enemyCntA++;
                        break;
                    case 1:
                        enemyCntB++;
                        break;
                    case 2:
                        enemyCntC++;
                        break;
                }
            }
            while (enemyList.Count > 0)
            {
                int ranZone = Random.Range(0, 4);
                GameObject instantenemy = Instantiate(enemies[enemyList[0]], enemyZones[ranZone].position, enemyZones[ranZone].rotation);
                Enemy enemy = instantenemy.GetComponent<Enemy>();
                enemy.target = player.transform;
                enemy.manager = this;
                enemyList.RemoveAt(0);
                yield return new WaitForSeconds(5);
            }
        }
        while (enemyCntA + enemyCntB+ enemyCntC+ enemyCntD>0)
        {
            yield return null;
        }
                yield return new WaitForSeconds(5);
        boss = null;
        StageEnd();
    }
    public void StageEnd()
    {
        player.transform.position = Vector3.zero;
        stage++;

        isBattel = false;
        ItemShop.SetActive(true);
        WeaponShop.SetActive(true);
        StartZon.SetActive(true);
        foreach (Transform zone in enemyZones)
        {
            zone.gameObject.SetActive(false);
        }

    }
    private void LateUpdate()
    {
        ScoreText.text = string.Format("{0:n0}", player.score);
        stageText.text = "Stage" + stage;

        int hour = (int)(PlayTime / 3600);
        int Min = (int)((PlayTime - hour * 3600) / 60);
        int second = (int)PlayTime % 60;
        playTimeText.text =
            string.Format("{0:00}", hour) + ":" +
            string.Format("{0:00}", Min) + ":" +
            string.Format("{0:00}", second);

        playHealtText.text = player.health + " / " + player.maxhealth;
        playCoinText.text = string.Format("{0:n0}", player.coin);
        if (player.equipWeapon == null)
        {
            playAmmoText.text = "- / " + player.ammo;
        }
        else if (player.equipWeapon.type == Weapon.Type.Melee)
        {
            playAmmoText.text = "- / " + player.ammo;
        }
        else
        {
            playAmmoText.text = player.equipWeapon.curAmmo + " / " + player.ammo;
        }
        weaponImage1.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weaponImage2.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weaponImage3.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weaponImageR.color = new Color(1, 1, 1, player.hasGreandes > 0 ? 1 : 0);
        enemyTextA.text = enemyCntA.ToString();
        enemyTextB.text = enemyCntB.ToString();
        enemyTextC.text = enemyCntC.ToString();

        if (boss != null)
        {
            BossHPGroup.anchoredPosition =  Vector3.down*30;
            BossHPBar.localScale = new Vector3((float)boss.CurHP / (float)boss.MaxHP, 1, 1);
        }
        else
        {
            BossHPGroup.anchoredPosition = Vector3.up * 200;

        }
    }
}
