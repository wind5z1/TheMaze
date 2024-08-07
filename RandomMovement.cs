using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//あるAIがランダム移動させるスクリプト
public class RandomMovementAI : MonoBehaviour
{
    public float moveSpeed = 20f;              // 移動速度
    private bool isTurning = false;            // 回転中かどうか
    private bool shouldReverse = false;        // 逆向きに進むべきかどうか
    public int damageAmount;                   // プレイヤーに与えるダメージ量
    private bool canDamagePlayer = true;       // プレイヤーにダメージを与えることができるかどうか
    public float visibilityDistance = 30.0f;   // 可視距離
    private Renderer enemyRenderer;            // 敵のレンダラー
    private Transform player;                  // プレイヤーの位置
    public AudioClip ghostSound;               // ゴーストの音
    private AudioSource audioSource;           // オーディオソース

    private void Start()
    {
        enemyRenderer = GetComponent<MeshRenderer>();    // 自身のレンダラーを取得
        enemyRenderer.enabled = false;                   // 最初は非表示にする
        player = GameObject.FindGameObjectWithTag("Player").transform;   // タグが"Player"のオブジェクトの位置を取得
        audioSource = gameObject.AddComponent<AudioSource>();   // オーディオソースを追加
        audioSource.clip = ghostSound;               // ゴーストの音を設定
    }

    void Update()
    {
        if (!isTurning)
        {
            Vector3 currentPosition = transform.position;

            if (!shouldReverse)
            {
                currentPosition.x -= moveSpeed * Time.deltaTime;   // 進行方向に応じて移動
            }
            else
            {
                currentPosition.x += moveSpeed * Time.deltaTime;   // 逆向きに移動
            }

            transform.position = currentPosition;    // 位置を更新
        }

        RaycastHit hit;
        if (!isTurning && Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            if (hit.collider.CompareTag("wall"))
            {
                StartCoroutine(TurnAround());   // 壁に衝突したら反転する
            }
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);   // プレイヤーまでの距離を計算

        if (distanceToPlayer <= visibilityDistance)
        {
            enemyRenderer.enabled = true;    // プレイヤーが近くにいる場合は表示する
            PlayGhostSound();                // ゴーストの音を再生
        }
        else
        {
            enemyRenderer.enabled = false;   // プレイヤーが遠くにいる場合は非表示にする
            StopGhostSound();                // ゴーストの音を停止
        }
    }

    // 反転処理を実行するコルーチン
    IEnumerator TurnAround()
    {
        isTurning = true;

        float duration = 0.5f;   // 反転にかかる時間
        Quaternion startRotation = transform.rotation;                            // 初期の回転
        Quaternion targetRotation = Quaternion.Euler(0, 180, 0) * startRotation;   // 目標の回転

        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / duration);   // 回転を補間する
            yield return null;
        }

        isTurning = false;       // 回転終了
        shouldReverse = !shouldReverse;   // 向きを反転する
    }

    // ゴーストの音を再生する
    void PlayGhostSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // ゴーストの音を停止する
    void StopGhostSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // プレイヤーとの衝突時にダメージを与える
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();   // プレイヤーの健康状態を取得
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10);     // プレイヤーにダメージを与える
                canDamagePlayer = false;
            }
        }
    }

    // プレイヤーとの衝突から離れたらダメージを与えることができるようにする
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canDamagePlayer = true;
        }
    }

}
