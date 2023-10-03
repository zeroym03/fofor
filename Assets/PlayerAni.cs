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
    public void DoJump()//점프실행
    {
        animator.SetBool("isJump", true);
        animator.SetTrigger("doJump");
    }
   public void isFloor()//땅에닿아서 착지모션실행
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
    public void WeaponTypeCH(Weapon equipWeapon)//무기에 타입에따라 애니메이션 변경
    {
        animator.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
    }
    public void Run(Vector3 moveVec ,bool walkDown)//해야하나생각중
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
