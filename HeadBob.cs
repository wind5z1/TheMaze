using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//プレイヤーの画面をちょっと揺れるするスクリプト
public class HeadBob : MonoBehaviour
{
    // ヘッドボブの振幅
    [Range(0.001f, 0.01f)]
    public float Amount = 0.002f;
    // ヘッドボブの周波数
    [Range(1f, 30f)]
    public float Frequency = 10.0f;
    // ヘッドボブのスムーズさ
    [Range(10f, 100f)]
    public float Smooth = 10.0f;

    // 初期位置
    Vector3 StartPos;

    // 初期設定
    void Start()
    {
        StartPos = transform.localPosition;
    }

    // 毎フレーム呼び出される
    void Update()
    {
        CheckForHeadbobTrigger();
        StopHeadbob();
    }

    // ヘッドボブのトリガーをチェックする
    private void CheckForHeadbobTrigger()
    {
        // 入力の大きさを計算
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

        // 入力がある場合、ヘッドボブを開始
        if (inputMagnitude > 0)
        {
            StartHeadBob();
        }
    }

    // ヘッドボブを開始する
    private Vector3 StartHeadBob()
    {
        Vector3 pos = Vector3.zero;
        // y方向のヘッドボブ
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * Frequency) * Amount * 1.4f, Smooth * Time.deltaTime);
        // x方向のヘッドボブ
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * Frequency / 2f) * Amount * 1.6f, Smooth * Time.deltaTime);
        // ローカルポジションに反映
        transform.localPosition += pos;
        return pos;
    }

    // ヘッドボブを停止する
    private void StopHeadbob()
    {
        // 現在の位置が初期位置と同じ場合、何もしない
        if (transform.localPosition == StartPos) return;
        // 現在の位置を初期位置にリセット
        transform.localPosition = Vector3.Lerp(transform.localPosition, StartPos, 1 * Time.deltaTime);
    }
}
