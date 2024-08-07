using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//懐中電灯のライトを制御するスクリプト
public class FlashlightController : MonoBehaviour
{
    // ライトコンポーネント
    Light light;

    // 初期設定
    private void Start()
    {
        // ライトコンポーネントを取得し、初期状態でオフに設定
        light = GetComponent<Light>();
        light.enabled = false;
    }

    // 毎フレーム呼び出される
    private void Update()
    {
        // 「F」キーが押された場合
        if (Input.GetKeyDown(KeyCode.F))
        {
            // ライトの状態を切り替え
            light.enabled = !light.enabled;
        }
    }
}
