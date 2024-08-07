using UnityEngine;
using UnityEngine.UI;
using TMPro;
//プレイヤーのHPを制御するスクリプト
public class PlayerHealth : MonoBehaviour
{
    public int playerHealth = 100;          // プレイヤーの初期体力
    public TMP_Text healthText;             // UIに表示する体力テキスト
    public GameObject gameOverUI;           // ゲームオーバーUI
    public StarterAssets.FirstPersonController firstPersonController;  // プレイヤーコントローラー
    private bool isGameStarted = false;     // ゲームが開始されているかどうかのフラグ

    private void Start()
    {
        isGameStarted = true;               // ゲーム開始を設定
        firstPersonController = GetComponent<StarterAssets.FirstPersonController>();  // プレイヤーコントローラーを取得
        UpdateUI();                         // UIを更新
        gameOverUI.SetActive(false);        // ゲームオーバーUIを非アクティブに設定
    }

    // ダメージを受けるメソッド
    public void TakeDamage(int damageAmount)
    {
        playerHealth -= damageAmount;       // ダメージを適用
        if (playerHealth < 0)
        {
            playerHealth = 0;               // 体力が0未満にならないように調整
        }
        UpdateUI();                         // UIを更新

        if (playerHealth == 0)
        {
            GameOver();                     // 体力が0になったらゲームオーバー処理を呼ぶ
            firstPersonController.enabled = false;  // プレイヤーコントローラーを無効にする
        }
    }

    // UIを更新するメソッド
    public void UpdateUI()
    {
        healthText.text = "Health: " + playerHealth;  // UIに現在の体力を表示
    }

    // ゲームオーバー時の処理
    void GameOver()
    {
        gameOverUI.SetActive(true);         // ゲームオーバーUIを表示
        Time.timeScale = 0;                 // ゲームの時間を停止
        UnlockCursor();                     // カーソルを解放
        isGameStarted = false;              // ゲームが開始されていない状態にする
        GameManager.instance.ResetGameTimer();  // GameManagerでゲームタイマーをリセットする
    }

    // 時間切れによるゲームオーバー処理
    public void GameOverByTime()
    {
        gameOverUI.SetActive(true);         // ゲームオーバーUIを表示
        Time.timeScale = 0;                 // ゲームの時間を停止
        firstPersonController.enabled = false;  // プレイヤーコントローラーを無効にする
        UnlockCursor();                     // カーソルを解放
        isGameStarted = false;              // ゲームが開始されていない状態にする
        GameManager.instance.ResetGameTimer();  // GameManagerでゲームタイマーをリセットする
    }

    // カーソルを解放するメソッド
    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;   // カーソルのロックを解除
        Cursor.visible = true;                    // カーソルを表示
    }

    // 体力を回復するメソッド
    public void RestoreHealth(int healthToRestore)
    {
        playerHealth += healthToRestore;        // 体力を回復
        if (playerHealth > 100)
        {
            playerHealth = 100;                // 体力が100を超えないように調整
        }
        UpdateUI();                            // UIを更新
    }
}
