using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ステージ２入ったあとの提示パネル
public class Tip : MonoBehaviour
{
    private bool isTriggered = false;  // トリガーが発生したかどうかのフラグ
    public GameObject panel;           // 表示するパネル

    // プレイヤーがトリガーエリアに入ったときに呼ばれるメソッド
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            panel.SetActive(true);  // パネルを表示する
            isTriggered = true;     // トリガーが発生したフラグを設定する
        }
    }

    void Update()
    {
        // トリガーが発生しており、キーBが押された場合
        if (isTriggered && Input.GetKeyDown(KeyCode.B))
        {
            panel.SetActive(false); // パネルを非表示にする
        }
    }
}
