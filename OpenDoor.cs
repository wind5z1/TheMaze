using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//最初のドアの動きを制御するスクリプト
public class OpenDoor : MonoBehaviour
{
    public float openAngle;                 // ドアを開く角度
    private bool isOpen = false;            // ドアが開いているかどうかのフラグ
    private bool isClosing = false;         // ドアが閉じている最中かどうかのフラグ
    public float rotationSpeed = 90.0f;     // ドアの回転速度
    private Quaternion initialRotation;     // 初期の回転
    private Quaternion targetRotation;      // 目標の回転
    private bool canToggleDoor = false;     // ドアのトグルが可能かどうかのフラグ
    public TextMeshProUGUI interactionText; // ドアの操作時に表示するテキスト
    private string openText = "Press O To Open.";   // 開くためのテキスト
    private string closeText = "Press O To Close."; // 閉じるためのテキスト
    public AudioClip doorOpenSound;         // ドアが開く音
    public AudioSource doorSound;           // オーディオソース

    private void Start()
    {
        initialRotation = transform.rotation;                             // 初期の回転を設定
        targetRotation = initialRotation * Quaternion.Euler(0, openAngle, 0); // 目標の回転を設定
        doorSound = GetComponent<AudioSource>();
        doorSound.clip = doorOpenSound;                                   // オーディオソースにサウンドを設定
    }

    private void Update()
    {
        if (canToggleDoor && Input.GetKeyDown(KeyCode.O))  // ドアのトグルが可能で、Oキーが押されたら
        {
            if (isOpen)                                     // ドアが開いている場合
            {
                StartCoroutine(CloseDoor());               // ドアを閉じるコルーチンを開始
                isOpen = false;                             // ドアが閉じている状態にする
            }
            else                                            // ドアが閉じている場合
            {
                StartCoroutine(OpenDoors());               // ドアを開くコルーチンを開始
                isOpen = true;                              // ドアが開いている状態にする
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))        // 衝突したオブジェクトがプレイヤーの場合
        {
            canToggleDoor = true;                              // ドアのトグルを許可する
            interactionText.text = isOpen ? closeText : openText;  // 適切な操作テキストを表示する
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))        // 衝突したオブジェクトがプレイヤーの場合
        {
            canToggleDoor = false;                             // ドアのトグルを禁止する
            interactionText.text = "";                         // 操作テキストを空にする
        }
    }

    // ドアを開くコルーチン
    IEnumerator OpenDoors()
    {
        float elapsedTime = 0;

        while (elapsedTime < 1)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime); // 回転を補間して設定
            elapsedTime += Time.deltaTime * rotationSpeed; // 経過時間を更新

            if (elapsedTime >= 0.5f && isOpen)  // 半分以上開いた時、ドアが開いている場合
            {
                doorSound.Play();             // ドアの音を再生
            }

            yield return null;                // 1フレーム待つ
        }
    }

    // ドアを閉じるコルーチン
    IEnumerator CloseDoor()
    {
        float elapsedTime = 0;

        while (elapsedTime < 1)
        {
            transform.rotation = Quaternion.Slerp(targetRotation, initialRotation, elapsedTime); // 回転を補間して設定
            elapsedTime += Time.deltaTime * rotationSpeed; // 経過時間を更新

            if (elapsedTime >= 0.5f && !isOpen) // 半分以上閉じた時、ドアが閉じている場合
            {
                doorSound.Play();             // ドアの音を再生
            }

            yield return null;                // 1フレーム待つ
        }
    }
}
