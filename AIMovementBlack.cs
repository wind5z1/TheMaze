using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 黒い色のAIのゴーストの動きを制御するスクリプト
public class AIMovementBlack : MonoBehaviour
{
    // 移動速度
    public float moveSpeed = 20f;
    // 回転中かどうかのフラグ
    private bool isTurning = false;
    // 逆方向に移動するかどうかのフラグ
    private bool shouldReverse = false;
    // ダメージ量
    public int damageAmount;
    // プレイヤーにダメージを与えることができるかどうかのフラグ
    private bool canDamagePlayer = true;
    // 視認距離
    public float visibilityDistance = 30.0f;
    // 敵のレンダラー
    private Renderer enemyRenderer;
    // プレイヤーへの参照
    private Transform player;
    // ゴーストの音
    public AudioClip ghostSound;
    // オーディオソース
    private AudioSource audioSource;

    // 初期化メソッド
    private void Start()
    {
        // レンダラーを取得し、非表示に設定
        enemyRenderer = GetComponent<MeshRenderer>();
        enemyRenderer.enabled = false;
        // プレイヤーをタグで検索
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // オーディオソースを追加し、クリップを設定
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = ghostSound;
    }

    // 更新メソッド
    void Update()
    {
        // 回転中でない場合
        if (!isTurning)
        {
            // 現在の位置を取得
            Vector3 currentPosition = transform.position;

            // 逆方向に移動する必要がない場合
            if (!shouldReverse)
            {
                // 前方に移動
                currentPosition.z += moveSpeed * Time.deltaTime;
            }
            else
            {
                // 後方に移動
                currentPosition.z -= moveSpeed * Time.deltaTime;
            }
            
            // 位置を更新
            transform.position = currentPosition;
        }
       
        // レイキャストで壁に当たった場合、回転を開始
        RaycastHit hit;
        if (!isTurning && Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            if (hit.collider.CompareTag("wall"))
            {
                StartCoroutine(TurnAround());
            }
        }

        // プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // プレイヤーが視認距離内にいる場合
        if (distanceToPlayer <= visibilityDistance)
        {
            // 敵を表示し、ゴーストの音を再生
            enemyRenderer.enabled = true;
            PlayGhostSound();
        }
        else
        {
            // 敵を非表示にし、ゴーストの音を停止
            enemyRenderer.enabled = false;
            StopGhostSound();
        }

        // 回転するコルーチン
        IEnumerator TurnAround()
        {
            // 回転中フラグを立てる
            isTurning = true;

            // 回転の時間を設定
            float duration = 0.5f;
            // 開始時の回転を取得
            Quaternion startRotation = transform.rotation;
            // 目標の回転を計算
            Quaternion targetRotation = Quaternion.Euler(0, 180, 0) * startRotation;

            // 経過時間を初期化
            float timeElapsed = 0f;
            // 回転が完了するまでループ
            while (timeElapsed < duration)
            {
                // 経過時間を更新
                timeElapsed += Time.deltaTime;
                // 現在の回転を開始時の回転と目標の回転の間で補間
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / duration);
                // 次のフレームまで待機
                yield return null;
            }

            // 回転中フラグを下ろす
            isTurning = false;
            // 逆方向に移動するフラグを反転
            shouldReverse = !shouldReverse;
        }
    }

    // ゴーストの音を再生するメソッド
    void PlayGhostSound()
    {
        // オーディオソースが再生中でない場合
        if (!audioSource.isPlaying)
        {
            // オーディオソースを再生
            audioSource.Play();
        }
    }

    // ゴーストの音を停止するメソッド
    void StopGhostSound()
    {
        // オーディオソースが再生中の場合
        if (audioSource.isPlaying)
        {
            // オーディオソースを停止
            audioSource.Stop();
        }
    }

    // 他のコライダーと接触したときのメソッド
    private void OnTriggerEnter(Collider other)
    {
        // 接触したオブジェクトがプレイヤーの場合
        if (other.CompareTag("Player"))
        {
            // プレイヤーの健康状態を取得
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            // プレイヤーの健康状態が取得できた場合
            if (playerHealth != null)
            {
                // プレイヤーにダメージを与える
                playerHealth.TakeDamage(50);
                // プレイヤーにダメージを与えることができない状態に設定
                canDamagePlayer = false;
            }
        }
    }

    // 他のコライダーとの接触が終了したときのメソッド
    private void OnTriggerExit(Collider other)
    {
        // 接触が終了したオブジェクトがプレイヤーの場合
        if (other.CompareTag("Player"))
        {
            // プレイヤーにダメージを与えることができる状態に設定
            canDamagePlayer = true;
        }
    }
}
