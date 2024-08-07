using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//アセットのジャンプスケアの音についてのスクリプト
public class SuddenJumpscare : MonoBehaviour
{
    public float visibilityDistance = 5f;       // ジャンプスケアが発生する距離
    private Renderer enemyRenderer;             // エネミーのレンダラー
    private Transform player;                   // プレイヤーのトランスフォーム
    private bool hasJumpscareOccurred = false;  // ジャンプスケアが発生したかどうかのフラグ
    public bool disableAfterJumpscare = false;  // ジャンプスケアの後にエフェクトを無効にするかどうかのオプション
    public float disableDelay = 1f;             // ジャンプスケアのエフェクトを無効にするまでの遅延時間
    public AudioClip jumpscareSound;            // ジャンプスケアの音声クリップ
    private AudioSource audioSource;            // AudioSource コンポーネント

    void Start()
    {
        enemyRenderer = GetComponent<MeshRenderer>(); // メッシュレンダラーの取得
        enemyRenderer.enabled = false;                // レンダラーを初期状態では非表示にする

        player = GameObject.FindGameObjectWithTag("Player").transform; // プレイヤーのトランスフォームを取得
        audioSource = gameObject.AddComponent<AudioSource>();         // AudioSource コンポーネントを追加
        audioSource.playOnAwake = false;                               // 起動時に音声を再生しないように設定
        audioSource.clip = jumpscareSound;                             // ジャンプスケアの音声クリップを設定
    }


    void Update()
    {
        if (hasJumpscareOccurred)
            return; // ジャンプスケアが発生した場合、何もしない

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= visibilityDistance)
        {
            audioSource.Play();         // ジャンプスケアの音声を再生
            enemyRenderer.enabled = true;  // レンダラーを表示してジャンプスケアを表示する

            hasJumpscareOccurred = true; // ジャンプスケアが発生したフラグを設定
            if (disableAfterJumpscare)
            {
                Invoke("DisableJumpscare", disableDelay); // 遅延時間後にジャンプスケアを無効化するメソッドを呼び出す
            }
        }
        else
        {
            enemyRenderer.enabled = false; // プレイヤーが表示距離外にいる場合、レンダラーを非表示にする
        }
    }

    // ジャンプスケアのエフェクトを無効化するメソッド
    private void DisableJumpscare()
    {
        audioSource.Stop();              // ジャンプスケアの音声を停止
        hasJumpscareOccurred = false;    // ジャンプスケア発生フラグをリセット
        enemyRenderer.enabled = false;   // ジャンプスケアのエフェクトを非表示にする
    }

    // レンダラーを無効化するメソッド（外部から使用可能）
    public void DisableRenderer()
    {
        enemyRenderer.enabled = false; // ジャンプスケアのエフェクトを非表示にする
    }
}
