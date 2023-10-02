using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    private void Update()
    {
        if (isBattel)
        {
            PlayTime += Time.deltaTime;
        }
    }
    private void LateUpdate()
    {
        ImegeUI();
        WeaponImageUI();
        BossHPBarUI();
    }
    void ImegeUI()
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



    }
    void WeaponImageUI()
    {
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

    }
    void BossHPBarUI()
    {
        if (boss != null)
        {
            BossHPGroup.anchoredPosition = Vector3.down * 30;
            BossHPBar.localScale = new Vector3((float)boss.CurHP / (float)boss.MaxHP, 1, 1);
        }
        else
        {
            BossHPGroup.anchoredPosition = Vector3.up * 200;

        }
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
