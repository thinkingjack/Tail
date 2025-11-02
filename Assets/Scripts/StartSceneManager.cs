using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class GameSettings
{
    public static int numberOfPlayers;
}

public class StartSceneManager : MonoBehaviour
{
    // 버튼에서 호출하는 함수들 (2,3,4인 플레이어 선택)

    public void SelectTwoPlayers()
    {
        GameSettings.numberOfPlayers = 2;
        LoadGameScene();
    }

    public void SelectThreePlayers()
    {
        GameSettings.numberOfPlayers = 3;
        LoadGameScene();
    }

    public void SelectFourPlayers()
    {
        GameSettings.numberOfPlayers = 4;
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene"); // 게임 씬 이름으로 변경
    }
}