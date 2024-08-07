using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

//あるオブジェクトのフリッカーについてを制御するスクリプト
public class FlickerController : MonoBehaviour
{
    // 動画クリップ
    public VideoClip videoClip;
    // 動画を再生する距離
    public float activationRange = 5f;
    // 対象となるタグ
    public string[] targetTags;
    // ビデオプレーヤー
    private VideoPlayer videoPlayer;
    // プレイヤーのトランスフォーム
    private Transform playerTransform;
    // メインカメラ
    private Camera mainCamera;
    // フリッカーが発生しているかどうか
    private bool isFlickering = false;

    // 初期設定
    void Start()
    {
        // ビデオプレーヤーを追加し、クリップを設定
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.clip = videoClip;
        videoPlayer.playOnAwake = false;
        // プレイヤーのカメラのトランスフォームを取得
        playerTransform = CameraTransform();
        // メインカメラを取得
        mainCamera = Camera.main;
    }

    // 毎フレーム呼び出される
    void Update()
    {
        if (!isFlickering)
        {
            // 各ターゲットタグに対してチェック
            foreach (string targetTag in targetTags)
            {
                // 指定されたタグを持つすべてのオブジェクトを検索
                GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
                foreach (GameObject enemy in enemies)
                {
                    // プレイヤーとの距離を計算
                    float distanceToTarget = Vector3.Distance(enemy.transform.position, playerTransform.position);
                    // 指定された範囲内にターゲットがいる場合、動画再生を開始
                    if (distanceToTarget < activationRange)
                    {
                        Debug.Log("play video");
                        StartCoroutine(StartFlickerEffect());
                        break;
                    }
                }
            }
        }
    }

    // フリッカー効果を開始するコルーチン
    IEnumerator StartFlickerEffect()
    {
        isFlickering = true;
        // 動画をカメラの近くに表示
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        videoPlayer.Play();
        // 1秒待機
        yield return new WaitForSeconds(1.0f);
        videoPlayer.Stop();
        videoPlayer.time = 0f;
        // 動画をカメラの遠くに戻す
        videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }
        isFlickering = false;
    }

    // カメラのトランスフォームを取得
    Transform CameraTransform()
    {
        GameObject cameraRoot = GameObject.Find("Cameraroot");
        if (cameraRoot != null)
        {
            Camera playerCamera = cameraRoot.GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                return playerCamera.transform;
            }
        }
        return Camera.main.transform;
    }
}
