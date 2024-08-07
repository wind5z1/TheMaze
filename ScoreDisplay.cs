using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//スコアの表示するためのスクリプト
public class ScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;    // テキストメッシュプロUGUIコンポーネント
    private GameManager gameManager;        // ゲームマネージャー

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();   // テキストメッシュプロUGUIコンポーネントを取得
        gameManager = GameManager.instance;               // ゲームマネージャーを取得
        textMeshPro.alpha = 0;                           // 初期状態ではテキストを非表示にする
    }

    void Update()
    {
        if (gameManager != null)
        {
            textMeshPro.text = "Score: " + gameManager.GetScore().ToString();   // スコアを表示

            if (gameManager.GetScore() > 0)
            {
                textMeshPro.alpha = 1;   // スコアが0より大きい場合はテキストを表示する
            }
        }
    }
}
