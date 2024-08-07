using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//鍵についてを制御するスクリプト
public class Key : MonoBehaviour
{
    // プレイヤーが近くにいるかどうかのフラグ
    private bool isPlayerNear = false;
    // 情報表示用テキスト
    public TextMeshProUGUI notificationText;
    // 鍵が回収されたかどうかのフラグ
    public bool isPickedUp = false;
    // 鍵を回収するためのテキスト
    public TextMeshProUGUI pickUpText;
    // 効果音のためのオーディオソース
    private AudioSource audioSource;
    // 鍵を回収する際の効果音
    public AudioClip pickUpSound;

    private void Start()
    {
        // オーディオソースの取得または追加
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 効果音の設定
        audioSource.clip = pickUpSound;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // プレイヤーが衝突した場合、プレイヤーが近くにいるとフラグを立てる
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // プレイヤーが離れた場合、フラグを下げる
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }

    // 毎フレーム呼び出される
    void Update()
    {
        // プレイヤーが近くにいて、鍵がまだ回収されていない場合
        if (isPlayerNear && !isPickedUp)
        {
            // 鍵を回収するためのテキストを表示する
            pickUpText.gameObject.SetActive(true);
            pickUpText.text = "Press E To Pick Up Key.";
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUpKey(); // Eキーが押されたら鍵を回収する
            }
        }
        else
        {
            // テキストを非表示にする
            pickUpText.text = "";
            pickUpText.gameObject.SetActive(false);
        }
    }

    // 鍵を回収する処理
    private void PickUpKey()
    {
        // すでに鍵が回収されている場合は何もしない
        if (isPickedUp)
        {
            return;
        }

        // 効果音が設定されている場合は再生する
        if (audioSource != null && pickUpSound != null)
        {
            audioSource.PlayOneShot(pickUpSound);
        }

        // プレイヤーのインベントリを取得
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();

        // インベントリが見つかった場合
        if (inventory != null)
        {
            // 通知テキストを表示して一定時間後に非表示にする
            if (notificationText != null)
            {
                notificationText.text = "You Obtained a Key.";
                StartCoroutine(HideNotification());
            }

            // 鍵のメッシュレンダラーを非表示にする
            MeshRenderer[] childRenderer = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer childRenderers in childRenderer)
            {
                childRenderers.enabled = false;
            }

            // カプセルコライダーを無効にする
            CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
            if (capsuleCollider != null)
            {
                capsuleCollider.enabled = false;
            }

            // プレイヤーのインベントリに鍵の数を加算する
            if (inventory != null)
            {
                inventory.numberOfKeys++;
            }

            // 鍵が回収されたことを記録する
            isPickedUp = true;
        }
    }

    // 通知テキストを一定時間後に非表示にするコルーチン
    private IEnumerator HideNotification()
    {
        yield return new WaitForSeconds(1f);

        if (notificationText != null)
        {
            notificationText.text = " ";
        }
    }
}
