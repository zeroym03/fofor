using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMob : MonoBehaviour
{
    public float speed = 3;
    public GameObject[] weapons;
    public GameObject[] grenades;
    public GameObject granadeobj;
    public Camera follwouCamera;
    public bool[] hasWeapons;
    public GameManager gameManager;


    public int ammo;
    public int coin;
    public int health;
    public int hasGreandes;
    public int score;

    public int maxammo;
    public int maxcoin;
    public int maxhealth;
    public int maxhasGreandes;

    float axisHorx;
    float axisVerz;
    float fireDelay;

    bool isShop;
    bool isJump;
    bool isDodge;
    bool isSwap;
    bool isFireReady = true;
    bool isBorder;
    bool isDamege = false;

    bool walkDown;
    bool JumpDown;
    bool iDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool fDown;
    bool gDown;
    bool rDown;

    bool isDead;
    bool isReload;


    Vector3 moveVec;
    Vector3 DodgeVec;
    Animator animator;
    Rigidbody plrigidbody;
    MeshRenderer[] meshes;
    GameObject nearobjeact;
  public  Weapon equipWeapon;
    int equipWeaponIndex = -1;
    private void Awake()
    {
        plrigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        meshes = GetComponentsInChildren<MeshRenderer>();
        PlayerPrefs.SetInt("MaxScore",1000);
    }
    void Start()
    {
        Debug.Log("asd");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor") {
            animator.SetBool("isJump", false);
            isJump = false;
               }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon"|| other.tag == "Shop")
        {
            nearobjeact = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Weapon")
        {
            nearobjeact = null;
        }
        else if (other.tag == "Shop")
        {
            Shop shop = other.gameObject.GetComponent<Shop>();
            shop.Exit();
            nearobjeact = null;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
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
                    health += item.value;
                    if (health > maxhealth) { health = maxhealth; }
                    break;
                case Item.Type.Grenade:
                    grenades[hasGreandes].SetActive(true);
                    hasGreandes += item.value;
                    if (hasGreandes > maxhasGreandes) { hasGreandes = maxhasGreandes; }
                    break;
            }
            Destroy(other.gameObject);
        }
      else  if (other.tag == "EnemyBullet" )
        {
            if (isDamege == false)
            {
                Debug.Log("EnemyBullet");
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;
                bool isBossAtk = other.name == "Boss Melee Alea";
                StartCoroutine(OnDamege(isBossAtk));
            }
            if(other.GetComponent<Rigidbody>() != null) { Destroy(other.gameObject); }
        }
    }
    IEnumerator OnDamege(bool isBossAtk)
    {
        if (health <= 0 && !isDead)
        {
            OnDie();
        }
        isDamege = true;
        foreach(MeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.red;
        }
        if (isBossAtk)
        {
            plrigidbody.AddForce(transform.forward * -25, ForceMode.Impulse);
        }
        yield return new WaitForSeconds(1);
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.white;
        }
        isDamege = false;
        if (isBossAtk)
        {
            plrigidbody.velocity = Vector3.zero;
        }
    
    }

    void OnDie()
    {
        animator.SetTrigger("doDie");
        isDead = true;
        gameManager.GameOver();
    }
    void FreezeRotatoin()
    {
        plrigidbody.angularVelocity = Vector3.zero;
    }
    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.red);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }
    private void FixedUpdate()
    {
        PlayerMove();
        FreezeRotatoin();
        StopToWall();
    }
    void Update()
    {
        if(!isDead)
        {
            Dodge();
            Jump();
            GetInput();
            Anime();
            Attack();
            Reload();
            Swap();
            Interation();
            Granade();
        }
      //  OnDie();
    }
    void Granade()
    {
        if (hasGreandes == 0)
        { return; }
        if (gDown && !isReload && !isSwap)
        {
            Ray ray = follwouCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Vector3 nextvec = hit.point - transform.position;
                nextvec.y = 13;
                GameObject instantGranade = Instantiate(granadeobj, transform.position, transform.rotation);
                Rigidbody rigidGranade = instantGranade.GetComponent<Rigidbody>();
                rigidGranade.AddForce(nextvec, ForceMode.Impulse);
                rigidGranade.AddTorque(Vector3.back * 10, ForceMode.Impulse);

                hasGreandes--;
                grenades[hasGreandes].SetActive(false);
            }
        }
    }
    void GetInput()
    {
        axisHorx = Input.GetAxisRaw("Horizontal");
        axisVerz = Input.GetAxisRaw("Vertical");
        walkDown = Input.GetButton("Walk");
        JumpDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButton("Fire1");
        gDown = Input.GetButtonDown("Fire2");
        rDown = Input.GetButtonDown("Reload");
        iDown = Input.GetButtonDown("Interation");
        sDown1 = Input.GetButtonDown("sDown1");
        sDown2 = Input.GetButtonDown("sDown2");
        sDown3 = Input.GetButtonDown("sDown3");
    }
    void Reload()
    {
        if (equipWeapon == null) { return; }
        if (equipWeapon.type == Weapon.Type.Melee) { return; }
        if (ammo <= 0) { return; }

        if (rDown && isFireReady && isDodge == false && isSwap == false && isJump == false)
        {
            animator.SetTrigger("doReload");
            isReload = true;
            Invoke("ReloadOut", 1f);
        }
    }
    void ReloadOut()
    {
        int reAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        equipWeapon.curAmmo = equipWeapon.maxAmmo;
        ammo -= reAmmo;
        isReload = false;
    }
    void Turn()
    {
        if (fDown)
        {
            Ray ray = follwouCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Vector3 nex = hit.point - transform.position;
                nex.y = 0;
                transform.LookAt(transform.position + nex);
            }
        }
        transform.LookAt(transform.position + moveVec);

    }
    void Attack()
    {
        if (equipWeapon == null) return;
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;
        if (fDown && isFireReady && isDodge == false && isSwap == false&&!isShop)
        {
            equipWeapon.Use();
            animator.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0;
        }
    }
    void PlayerMove()
    {
        moveVec = new Vector3(axisHorx, 0, axisVerz).normalized;
        if (isDodge == true) { moveVec = DodgeVec; }
        if (isSwap == true || isReload || isFireReady == false) { moveVec = Vector3.zero; }
        if (isBorder == false) { transform.position += moveVec * speed * Time.deltaTime; }

        Turn();
    }
    void Anime()
    {
        animator.SetBool("isRun", moveVec != Vector3.zero);
        animator.SetBool("isWalk", walkDown);
    }
    void Jump()
    {
        if (JumpDown && moveVec == Vector3.zero && isJump == false && !isSwap)
        {
            plrigidbody.AddForce(Vector3.up * 10, ForceMode.Impulse);
            animator.SetBool("isJump", true);
            animator.SetTrigger("doJump");
            isJump = true;
        }
    }
    void Dodge()
    {
        if (JumpDown && moveVec != Vector3.zero && isJump == false && !isSwap)
        {
            DodgeVec = moveVec;
            speed *= 2;
            animator.SetTrigger("doDodge");
            isDodge = true;
            Invoke("DodgeOut", 0.5f);
        }
    }
    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }
    void Swap()
    {
        if (sDown1 && (hasWeapons[0] == false || equipWeaponIndex == 0)) { return; }
        if (sDown2 && (hasWeapons[1] == false || equipWeaponIndex == 1)) { return; }
        if (sDown3 && (hasWeapons[2] == false || equipWeaponIndex == 2)) { return; }

        int weaponIndex = -1;
        if (sDown1) { weaponIndex = 0; }
        if (sDown2) { weaponIndex = 1; }
        if (sDown3) { weaponIndex = 2; }

        if ((sDown1 || sDown2 || sDown3) && isJump == false && isDodge == false)//
        {
            if (equipWeapon != null) { equipWeapon.gameObject.SetActive(false); }
            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            weapons[weaponIndex].SetActive(true);

            animator.SetTrigger("DoSwap");
            isSwap = true;
            Invoke("SwapOut", 0.5f);
        }
    }
    void SwapOut()
    {
        isSwap = false;
    }
    void Interation()
    {
        if (iDown == true && nearobjeact != null && isJump == false && isDodge == false)
        {
            if (nearobjeact.tag == "Weapon")
            {
                Item item = nearobjeact.GetComponent<Item>();
                int weaponindex = item.value;
                Debug.Log(item.value);
                hasWeapons[weaponindex] = true;
                Destroy(nearobjeact);

            }
            else if (nearobjeact.tag == "Shop")
            {
                Shop shop = nearobjeact.GetComponent<Shop>();
                shop.Enter(this);
                isShop = false;
                nearobjeact = null;
            }
        }
    }
}
