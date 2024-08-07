using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//ゲーム主要のシステムを制御するスクリプト
public class GameManager : MonoBehaviour
{
    // インスタンス
    public static GameManager instance;
    // プレイヤースコア
    public int playerScore = 0;
    // プレイヤーのコントローラー
    private StarterAssets.FirstPersonController firstPersonController;
    // スタートパネル
    public GameObject startPanel;
    // 表示するテキスト
    public TextMeshProUGUI textToDisplay;
    // 時間表示
    public TextMeshProUGUI timeDisplay;
    // ゲームタイマー
    private float gametimer = 600f;
    // ゲームが開始されたかどうか
    private bool isGameStarted = false;
    // プレイヤーのヘルス
    private PlayerHealth playerHealth;

    // 初期設定
    private void Start()
    {
        // フレームレートを設定
        Application.targetFrameRate = 60;
        // カーソルの状態を設定
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // プレイヤーコントローラーを取得
        firstPersonController = GetComponent<StarterAssets.FirstPersonController>();
        // プレイヤーヘルスを取得
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    // インスタンスを設定
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // テキスト表示を無効にする
        textToDisplay.enabled = false;
        timeDisplay.enabled = false;
    }

    // 毎フレーム呼び出される
    private void Update()
    {
        if (isGameStarted)　//ゲームが始まったら、タイマーが始める
            gametimer -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(gametimer / 60);
        int seconds = Mathf.FloorToInt(gametimer % 60);
        timeDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (gametimer <= 0 && playerHealth != null)  //タイマーがゼロになったらゲーム終わり
        {
            gametimer = 0;
            playerHealth.GameOverByTime();
        }
    }

    // ゲームが開始されたかどうかを返す
    public bool IsGameStarted()
    {
        return isGameStarted;
    }

    // スコアを追加する
    public void AddScore(int amount)
    {
        playerScore += amount;
    }

    // 現在のスコアを返す
    public int GetScore()
    {
        return playerScore;
    }

    // 現在のゲームタイマーを返す
    public float GetGameTimer()
    {
        return gametimer;
    }

    // スコアをリセットする
    public void ResetScore()
    {
        playerScore = 0;
    }

    // オブジェクトが破棄されるときに呼ばれる
    private void OnDestroy()
    {
        instance = null;
    }

    // ゲームを再スタートする
    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");

        if (firstPersonController != null)
        {
            firstPersonController.enabled = true;
        }
        Time.timeScale = 1;
    }

    // ゲームを開始し、パネルを閉じる
    public void StartGameAndClosePanel()
    {
        startPanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        textToDisplay.enabled = true;
        timeDisplay.enabled = true;
        isGameStarted = true;
    }

    // ゲームを終了する
    public void QuitGame()
    {
        Application.Quit();
    }

    // ゲームタイマーをリセットする
    public void ResetGameTimer()
    {
        gametimer = 600f;
    }

    // タイマーを一時停止する
    public void PauseTimer()
    {
        isGameStarted = false;
        timeDisplay.enabled = false;
    }
}
