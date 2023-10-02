using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAni : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public void DoJump()//Á¡ÇÁ½ÇÇà
    {
        animator.SetBool("isJump", true);
        animator.SetTrigger("doJump");
    }
   public void isFloor()//¶¥¿¡´ê¾Æ¼­ ÂøÁö¸ð¼Ç½ÇÇà
    {
        animator.SetBool("isJump", false);
    }
    public void PlayerDie()
    {
        animator.SetTrigger("doDie");
    }
    public void Reload()
    {
        animator.SetTrigger("doReload");
    }
    public void WeaponTypeCH(Weapon equipWeapon)
    {
        animator.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
    }
    public void Run(Vector3 moveVec ,bool walkDown)
    {
        animator.SetBool("isWalk", walkDown);
        animator.SetBool("isRun", moveVec != Vector3.zero);
    }
    public void DoDodge()
    {
        animator.SetTrigger("doDodge");
    }
    public void DoSwap()
    {
        animator.SetTrigger("DoSwap");
    }
}
