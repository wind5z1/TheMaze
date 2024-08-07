using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//勝利パネルについて
public class WinningScript : MonoBehaviour
{
    public GameObject winPanel;                         // 勝利パネルの参照
    public StarterAssets.FirstPersonController firstPersonController;  // プレイヤーのファーストパーソンコントローラー

    // カーソルをロック解除する
    void UnlockCursor()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);  // 勝利パネルを表示する
        }

        firstPersonController.enabled = false;  // プレイヤーのコントロールを無効にする
        Cursor.lockState = CursorLockMode.None;  // カーソルロックを解除する
        Cursor.visible = true;  // カーソルを表示する
        GameManager.instance.PauseTimer();  // GameManagerでタイマーを一時停止する
    }

    // プレイヤーが衝突したときの処理
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            UnlockCursor();  // カーソルをロック解除する関数を呼び出す
        }
    }

    // ボタンがクリックされたときの処理
    public void OnButtonClick()
    {
        UnlockCursor();  // カーソルをロック解除する関数を呼び出す
    }
}
