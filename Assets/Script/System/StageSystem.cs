using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StageSystem : MonoBehaviour
{
     List<int> enemyList;
    private void Awake()
    {
        enemyList = new List<int>();
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
        while (enemyCntA + enemyCntB + enemyCntC + enemyCntD > 0)
        {
            yield return null;
        }
        yield return new WaitForSeconds(5);
        boss = null;
        StageEnd();
    }
    public void GameOver()
    {
        gamePanal.SetActive(false);
        gameOverPanal.SetActive(true);
        int maxScore = PlayerPrefs.GetInt("MaxScore");
        if (player.score > maxScore)
        {
            bestScoreText.gameObject.SetActive(true);
            PlayerPrefs.SetInt("MaxScore", player.score);
        }
    }
}
