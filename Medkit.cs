using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//救急キットを制御するスクリプト
public class Medkit : MonoBehaviour
{
    private bool isPlayerNear = false;  // プレイヤーが近くにいるかどうかのフラグ
    private Collision playerCollision;  // プレイヤーとの衝突情報
    public int healthToRestore = 20;    // 回復する体力量
    public Animator medkitAnimator;     // アニメーター
    private bool isPickedUp = false;    // アイテムが回収されたかどうかのフラグ
    public TextMeshProUGUI infoText;    // 情報表示用テキスト
    private AudioSource audioSource;    // 効果音用オーディオソース
    public AudioClip pickUpSound;       // 回収音

    private void Start()
    {
        medkitAnimator = GetComponent<Animator>();
        medkitAnimator.enabled = false;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = pickUpSound;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 衝突したオブジェクトがプレイヤーであれば、プレイヤーが近くにいるとフラグを立てる
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;
            playerCollision = collision;
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
        // プレイヤーが近くにいて、アイテムがまだ回収されていない場合
        if (isPlayerNear && !isPickedUp)
        {
            // 回収するための情報テキストを表示する
            infoText.text = "Press E To Pick Up Medkit.";
            infoText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUpMedkit();  // Eキーが押されたらアイテムを回収する
            }
        }
        else
        {
            infoText.gameObject.SetActive(false);
        }
    }

    // アイテムを回収する処理
    private void PickUpMedkit()
    {
        Debug.Log("Picked up");
        // 効果音が設定されている場合は再生する
        if (audioSource != null)
        {
            audioSource.PlayOneShot(pickUpSound);
        }

        // 衝突情報があれば
        if (playerCollision != null)
        {
            infoText.text = " ";
            infoText.gameObject.SetActive(true);
            medkitAnimator.enabled = true;
            medkitAnimator.Play("ammo_box_anim");
            isPickedUp = true;

            StartCoroutine(DestroyAfterAnimation());
        }
    }

    // アニメーション再生後、一定時間が経過した後にアイテムを破棄する
    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(medkitAnimator.GetCurrentAnimatorClipInfo(0).Length);

        // プレイヤーの体力を回復し、UIを更新する
        PlayerHealth playerHealth = playerCollision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.RestoreHealth(healthToRestore);
            playerHealth.UpdateUI();
        }

        // アニメーションを無効にし、情報テキストを非表示にしてアイテムを破棄する
        medkitAnimator.enabled = false;
        infoText.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
