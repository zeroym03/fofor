using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public RectTransform UIGroup;
    public Animator Animator;
    PlayerMob enterPlayer;

    public Text talkText;

    public int[] itemPrice;
  public  Transform[] itemPos;
    public GameObject[] itemObj;
    public string[] TalkData;
    public void Enter(PlayerMob playerMob)
    {
        enterPlayer = playerMob;
        UIGroup.anchoredPosition = Vector3.zero;
    }
    public void Exit()
    {
        UIGroup.anchoredPosition = Vector3.down * 1000;
        Animator.SetTrigger("doHello");
    }
    public void Buy(int index)
    {
        int price = itemPrice[index];
        if(price > enterPlayer.coin)
        {
            StopCoroutine(Talk());
            StartCoroutine(Talk());
            return;
        }
        enterPlayer.coin -= price;
        Vector3 renVec = Vector3.right * Random.Range(-3, 3)+Vector3.forward * Random.Range(-3,3);
        Instantiate(itemObj[index], itemPos[index].position + renVec, itemPos[index].rotation);
    }
    IEnumerator Talk()
    {
        talkText.text = TalkData[1];
        yield return new WaitForSeconds(1f);
        talkText.text = TalkData[0];
    }
}
