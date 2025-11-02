using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public float speed = 3f;
    public GameObject tailPrefab;
    public float tailSpacing = 0.5f;
    public List<GameObject> tailCoins = new List<GameObject>();

    void Update()
    {
        float h = Input.GetAxisRaw(horizontalAxis);
        float v = Input.GetAxisRaw(verticalAxis);
        Vector3 dir = new Vector3(h, v, 0).normalized;

        transform.Translate(dir * speed * Time.deltaTime);
        UpdateTail();
    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (!CompareTag("Player")) return; // 꼬리일 경우 무시

    if (other.CompareTag("Coin"))
    {
        AddTailCoin();
        Destroy(other.gameObject);
    }
}

    void AddTailCoin()
            {
            Vector3 tailPosition;

            if (tailCoins.Count == 0)
            {
            // 첫 꼬리는 플레이어 바로 뒤에서 tailSpacing 만큼 떨어진 위치에 생성
            tailPosition = transform.position - transform.right * tailSpacing;
            }
            else
            {
            // 이후 꼬리는 이전 꼬리 뒤에 생성
                GameObject lastTail = tailCoins[tailCoins.Count - 1];
                tailPosition = lastTail.transform.position - lastTail.transform.right * 0.3f;
            }

            GameObject newTail = Instantiate(tailPrefab, tailPosition, Quaternion.identity);

        // 꼬리 소유자 지정
        TailCoin tailCoinComp = newTail.GetComponent<TailCoin>();
        if (tailCoinComp != null)
            tailCoinComp.ownerPlayer = this;

        // 꼬리가 플레이어보다 위에 렌더링되도록 SpriteRenderer 설정
        SpriteRenderer sr = newTail.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = 1; // 플레이어보다 높게 설정
        }

        tailCoins.Add(newTail);


        UpdateTailEndTag(); // 꼬리 추가 후 마지막 꼬리 태그 갱신
    }

    void UpdateTail()
    {
        for (int i = 0; i < tailCoins.Count; i++)
        {
            Vector3 target = (i == 0) ? transform.position : tailCoins[i - 1].transform.position;
            Vector3 direction = (target - tailCoins[i].transform.position).normalized;
            float distance = Vector3.Distance(tailCoins[i].transform.position, target);

            if (distance > tailSpacing)
            {
                Vector3 newPos = target - direction * tailSpacing;
                tailCoins[i].transform.position = Vector3.Lerp(
                    tailCoins[i].transform.position,
                    newPos,
                    Time.deltaTime * 10f
                    );
            }
        }
    }
    public void ForceTailRearrange()
    {
        for (int i = 0; i < tailCoins.Count; i++)
        {
            Vector3 pos = (i == 0) ? transform.position - transform.up * tailSpacing
                                   : tailCoins[i - 1].transform.position - transform.up * tailSpacing;

            tailCoins[i].transform.position = pos;
        }
    }
    public void UpdateTailEndTag()
    {
        // 모든 꼬리 조각의 TailEnd 태그 제거
        foreach (var tail in tailCoins)
        {
            tail.tag = "Untagged";
        }

        if (tailCoins.Count > 0)
        {
            // 마지막 꼬리에만 TailEnd 태그 설정
            tailCoins[tailCoins.Count - 1].tag = "TailEnd";
        }
    }
}