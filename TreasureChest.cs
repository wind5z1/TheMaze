using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//チェストの拾いについてのスクリプト
public class TreasureChest : MonoBehaviour
{
    private bool isPlayerNear = false;      // プレイヤーが近くにいるかどうかのフラグ
    private bool isPickedUp = false;       // すでに拾われたかどうかのフラグ
    public int scoreValue = 50;           // チェストから得られるスコア値
    public TextMeshProUGUI infoText;       // 操作の指示を表示するテキスト
    private AudioSource audioSource;       // オーディオソース
    public AudioClip pickUpSound;          // チェストを拾ったときの効果音

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();  // オーディオソースを取得する
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();  // オーディオソースがなければ追加する
        }

        audioSource.clip = pickUpSound;  // 効果音を設定する
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;  // プレイヤーが近くにいる場合、フラグを立てる
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNear = false;  // プレイヤーが離れた場合、フラグを下げる
        }
    }

    void Update()
    {
        if (isPlayerNear && !isPickedUp)
        {
            infoText.text = "Press E To Pick Up 50 Points.";  // プレイヤーが近くにいて未拾収の場合、操作の指示を表示する
            infoText.gameObject.SetActive(true);  // 操作の指示を表示する
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUpTreasureChest();  // Eキーが押されたらチェストを拾う処理を呼び出す
            }
        }
        else
        {
            infoText.gameObject.SetActive(false);  // それ以外の場合、操作の指示を非表示にする
        }
    }

    private void PickUpTreasureChest()
    {
        if (isPickedUp)
        {
            return;  // すでに拾収済みの場合、処理を終了する
        }

        if (audioSource != null)
        {
            audioSource.PlayOneShot(pickUpSound);  // 効果音を再生する
        }

        GameManager.instance.AddScore(scoreValue);  // スコアをGameManagerに追加する

        if (infoText != null)
        {
            infoText.gameObject.SetActive(false);  // 操作の指示を非表示にする
        }

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;  // チェストのメッシュレンダラーを非表示にする
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;  // チェストのコライダーを無効にする
        }

        isPickedUp = true;  // フラグを立てる
    }
}
