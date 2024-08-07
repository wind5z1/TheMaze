using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ゲーム中のパネル（一時停止、ゲーム再開）を制御するスクリプト
public class Ingamepanel : MonoBehaviour
{
    // インゲームパネル
    public GameObject InGamePanel;
    // プレイヤーコントローラー
    public StarterAssets.FirstPersonController firstPersonController;
    // スタートパネル
    public GameObject startPanel;

    // 初期設定
    void Start()
    {
        // ゲーム開始時はインゲームパネルを非表示にする
        InGamePanel.SetActive(false);
    }

    // 毎フレーム呼び出される
    void Update()
    {
        // スタートパネルがアクティブな場合は何もしない
        if (startPanel != null && startPanel.activeSelf)
        {
            return;
        }

        // Escapeキーが押された場合の処理
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (InGamePanel.activeSelf)
            {
                // インゲームパネルが表示されている場合は非表示にする
                InGamePanel.SetActive(false);
                // カーソルの状態を設定
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                // 時間の流れを通常に戻す
                Time.timeScale = 1f;
                // プレイヤーコントローラーを有効にする
                firstPersonController.enabled = true;
            }
            else
            {
                // インゲームパネルが非表示の場合は表示する
                InGamePanel.SetActive(true);
                // カーソルの状態を設定
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                // 時間を停止する
                Time.timeScale = 0f;
                // プレイヤーコントローラーを無効にする
                firstPersonController.enabled = false;
            }
        }
    }
}
