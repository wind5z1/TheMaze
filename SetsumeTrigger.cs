using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ステージ２に入る前にの説明パネル
public class SetsumeiTrigger : MonoBehaviour
{
    private bool isTriggered = false;   // トリガーが発動したかどうかのフラグ
    public GameObject panel;            // 表示するパネル

    // プレイヤーがトリガーに入ったときに呼び出される
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            panel.SetActive(true);    // パネルを表示する
            isTriggered = true;       // トリガーを発動済みにする
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered && Input.GetKeyDown(KeyCode.B))
        {
            panel.SetActive(false);   // Bキーが押されたらパネルを非表示にする
        }
    }
}
