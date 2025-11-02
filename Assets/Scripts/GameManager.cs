using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public float gameTime = 120f;
    public Text timerText;

    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    public string[] horizontalAxes = { "Horizontal1", "Horizontal2", "Horizontal3", "Horizontal4" };
    public string[] verticalAxes = { "Vertical1", "Vertical2", "Vertical3", "Vertical4" };
    public Color[] playerColors = { Color.red, Color.blue, Color.green, Color.yellow };

    public List<Text> playerCoinTexts;  // 플레이어별 UI 텍스트 리스트 (인원수에 맞게 연결)
    private List<PlayerController> players = new List<PlayerController>();
    private bool gameStarted = false;

    public GameObject winPanel;
    public Text winnerText;

    void Start()
    {
        int numberOfPlayers = GameSettings.numberOfPlayers; // 씬 간 데이터 전달 값 읽기


        // 플레이어 수에 따라 UI 활성화
        for (int i = 0; i < playerCoinTexts.Count; i++)
        {
            playerCoinTexts[i].gameObject.SetActive(i < numberOfPlayers);
        }
        for (int i = 0; i < numberOfPlayers; i++)
        {
            GameObject playerObj = Instantiate(playerPrefab, spawnPoints[i].position, Quaternion.identity);

            PlayerController controller = playerObj.GetComponent<PlayerController>();
            controller.horizontalAxis = horizontalAxes[i];
            controller.verticalAxis = verticalAxes[i];

            SpriteRenderer sr = playerObj.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.color = playerColors[i];

            playerObj.name = "Player" + (i + 1);

            players.Add(controller);
        }

        winPanel.SetActive(false);
        gameStarted = true;
        Time.timeScale = 1f;
    }

    void UpdatePlayerCoinUI()
    {
        for (int i = 0; i < players.Count; i++)
        {
            playerCoinTexts[i].text = $"Player {i + 1}: {players[i].tailCoins.Count} 코인";
        }
    }
    void Update()
    {
        if (!gameStarted) return;

        if (gameTime > 0)
        {
            gameTime -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.CeilToInt(gameTime).ToString();

            UpdatePlayerCoinUI(); // 실시간으로 UI 갱신

            if (gameTime <= 0)
            {
                EndGame();
            }
        }
    }

    void EndGame()
    {
        int maxCoins = -1;
        List<PlayerController> winners = new List<PlayerController>();

        foreach (var player in players)
        {
            int coinCount = player.tailCoins.Count;

            if (coinCount > maxCoins)
            {
                maxCoins = coinCount;
                winners.Clear();
                winners.Add(player);
            }
            else if (coinCount == maxCoins)
            {
                winners.Add(player);
            }
        }

        winPanel.SetActive(true);

        if (winners.Count == 1)
        {
            winnerText.text = $"승리: {winners[0].name}\n{maxCoins} 코인";
        }
        else
        {
            winnerText.text = $"비겼습니다.\n{maxCoins} 코인";
        }

        Time.timeScale = 0f;
    }
}