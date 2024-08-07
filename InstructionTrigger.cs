using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ゲーム中の説明のパネルをトリガーするスクリプト
public class InstructionTrigger : MonoBehaviour
{
    // トリガーが一度だけ作動するためのフラグ
    private bool isTriggered = false;
    // 表示するパネル
    public GameObject panel;

    // 他のオブジェクトがトリガー領域に入ったときに呼び出される
    private void OnTriggerEnter(Collider other)
    {
        // トリガーがまだ作動していなくて、かつプレイヤーがトリガー領域に入った場合
        if (!isTriggered && other.CompareTag("Player"))
        {
            // パネルを表示する
            panel.SetActive(true);
            // トリガーが作動したことを記録する
            isTriggered = true;
        }
    }

    // 毎フレーム呼び出される
    void Update()
    {
        // トリガーが作動していて、かつBキーが押された場合
        if (isTriggered && Input.GetKeyDown(KeyCode.B))
        {
            // パネルを非表示にする
            panel.SetActive(false);
        }
    }
}
