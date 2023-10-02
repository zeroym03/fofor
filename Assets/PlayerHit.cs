using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerHit : MonoBehaviour
{
    PlayerAni playerAni;
    Rigidbody playerRigid;
    PlayerMob playerMob;
    public int ammo;
    public int coin;
    public int hasGreandes;
    public int score;
    public int maxammo;
    public int maxcoin;
    public int maxhasGreandes;
    bool isDamege = false;
    bool isDead;
       MeshRenderer[] meshes;
    private void Awake()
    {
        playerRigid = GetComponent<Rigidbody>();
        playerMob = GetComponent<PlayerMob>();
        playerMob. health = playerMob.maxhealth;
        playerAni = GetComponent<PlayerAni>();
        meshes = GetComponentsInChildren<MeshRenderer>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            ItemTypeCheck(item );
            Destroy(other.gameObject);
        }
        else if (other.tag == "EnemyBullet")
        {
            if (isDamege == false)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                playerMob.health -= enemyBullet.damage;
                bool isBossAtk = other.name == "Boss Melee Alea";
                Debug.Log(isBossAtk);
                StartCoroutine(OnDamege(isBossAtk));
            }
            if (other.GetComponent<Rigidbody>() != null) { Destroy(other.gameObject); }
        }
    }
    void ItemTypeCheck(Item item)
    {
        switch (item.type)
        {
            case Item.Type.Ammo:
                ammo += item.value;
                if (ammo > maxammo) { ammo = maxammo; }
                break;
            case Item.Type.Coin:
                coin += item.value;
                if (coin > maxcoin) { coin = maxcoin; }
                break;
            case Item.Type.Heart:
                playerMob. health += item.value;
                if (playerMob.health > playerMob.maxhealth) { playerMob.health = playerMob.maxhealth; }
                break;
            case Item.Type.Grenade:
                playerMob. grenades[hasGreandes].SetActive(true);
                hasGreandes += item.value;
                if (hasGreandes > maxhasGreandes) { hasGreandes = maxhasGreandes; }
                break;
        }
    }
    IEnumerator OnDamege(bool isBossAtk)
    {
        isDamege = true;
        if (playerMob.health <= 0 && !isDead)
        {
            OnDie();
        }
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.red;
        }
        if (isBossAtk)
        {
            playerRigid.AddForce(transform.forward * -25, ForceMode.Impulse);
        }
        yield return new WaitForSeconds(1);
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.white;
        }
        isDamege = false;
        if (isBossAtk)
        {
            playerRigid.velocity = Vector3.zero;
        }
    }
    void OnDie()
    {
        playerAni.PlayerDie();
        isDead = true;
      GenericSinglngton<GameManager>.Instance.GameOver();
    }
}
