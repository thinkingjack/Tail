using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailStealer : MonoBehaviour
{
    public PlayerController ownerPlayer;

    void Start()
    {
        if (ownerPlayer == null)
            ownerPlayer = GetComponent<PlayerController>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TailEnd"))
        {
            
            TailCoin tailCoin = other.GetComponent<TailCoin>();

            if (tailCoin != null)
            {
                PlayerController victim = tailCoin.ownerPlayer;

                if (victim != null && victim != ownerPlayer && victim.tailCoins.Count > 0)
                {
                    Debug.Log("Stealing from: " + victim.name);
                    StealCoins(victim);
                }
            }
        }
    }

    void StealCoins(PlayerController victim)
    {
        int stolenCount = victim.tailCoins.Count;

        // 꼬리 넘기기
        foreach (var tailCoin in victim.tailCoins)
        {
            //부모를 끊어 꼬리 오브젝트가 고정되지 않도록
            tailCoin.transform.SetParent(null);

            // 꼬리를 새 플레이어에 등록
            ownerPlayer.tailCoins.Add(tailCoin);

            // 꼬리의 주인을 새로 설정
            TailCoin coinComp = tailCoin.GetComponent<TailCoin>();
            if (coinComp != null)
            {
                coinComp.ownerPlayer = ownerPlayer;
            }
        }

        victim.tailCoins.Clear();

        // 꼬리 태그 갱신
        ownerPlayer.UpdateTailEndTag();

        // 꼬리 위치 재정렬
        ownerPlayer.ForceTailRearrange();

    }
}