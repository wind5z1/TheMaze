using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//ステージ２のドアを制御するスクリプト
public class DoorControllerLvl2 : MonoBehaviour
{
    // 必要なスコア
    public int requiredScore = 200;
    // キーを持っているかどうか
    public bool hasKey = false;
    // ドアが開いているかどうか
    private bool isDoorOpen = false;
    // Keyクラスのインスタンス
    public Key key;
    // UI表示用のTextMeshPro
    public TextMeshProUGUI textMeshPro;
    // プレイヤーがドアの近くにいるかどうか
    private bool isPlayerNear = false;
    // ドアの閉じた回転
    private Quaternion closedRotation;
    // ドアの開いた回転
    private Quaternion openRotation;
    // ドアの開く速度
    private float openSpeed = 2.0f;
    // 必要なキーの数
    public int numberOfKeys = 0;
    // ドアが開く時の音
    public AudioClip doorOpenSound;
    // オーディオソース
    public AudioSource doorAudioSource;

    // 初期設定
    private void Start()
    {
        // ドアの閉じた回転を取得
        closedRotation = transform.rotation;
        // ドアの開いた回転を設定
        openRotation = Quaternion.Euler(0, 1, 0);
        // オーディオソースを取得
        doorAudioSource = GetComponent<AudioSource>();
    }

    // 衝突時に呼び出される
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // プレイヤーが近くにいることを示す
            isPlayerNear = true;
        }
    }

    // 衝突解除時に呼び出される
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // プレイヤーが近くにいないことを示す
            isPlayerNear = false;
        }
    }

    // 毎フレーム呼び出される
    void Update()
    {
        // 現在のスコアを取得
        int currentScore = GameManager.instance.GetScore();
        // プレイヤーのインベントリを取得
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();

        // スコアとキーの数が条件を満たし、かつ「O」キーが押された場合、ドアを開ける
        if (currentScore >= requiredScore && inventory.numberOfKeys >= 2 && Input.GetKeyDown(KeyCode.O))
        {
            OpenDoor();
        }

        // プレイヤーが近くにいる場合
        if (isPlayerNear)
        {
            // スコアとキーの数が条件を満たしている場合
            if (currentScore >= requiredScore && inventory.numberOfKeys >= 2 && key.isPickedUp)
            {
                if (!isDoorOpen)
                {
                    textMeshPro.text = "Press 'O' to open";
                }
                else
                {
                    textMeshPro.text = "";
                    GameManager.instance.ResetScore();
                    inventory.numberOfKeys -= 2;
                }
            }
            else
            {
                // 条件を満たしていない場合のメッセージを表示
                string message = "This door requires " + " 2 keys " + " and " + " 300 points. ";
                message += "\nNow you have " + (inventory.numberOfKeys > 0 ? inventory.numberOfKeys + " keys " : " no keys ") + " and " + currentScore + " points.";
                textMeshPro.text = message;
            }
        }
        else
        {
            // プレイヤーが近くにいない場合
            textMeshPro.text = " ";
        }
    }

    // ドアを開ける
    private void OpenDoor()
    {
        if (!isDoorOpen)
        {
            isDoorOpen = true;
            doorAudioSource.PlayOneShot(doorOpenSound);
            StartCoroutine(RotateDoor(openRotation));
        }
    }

    // ドアを回転させるコルーチン
    private IEnumerator RotateDoor(Quaternion targetRotation)
    {
        float startTime = Time.time;
        while (Time.time - startTime < 1.0f)
        {
            float t = (Time.time - startTime) * openSpeed;
            transform.rotation = Quaternion.Slerp(closedRotation, targetRotation, t);
            yield return null;
        }
        transform.rotation = targetRotation;
        textMeshPro.gameObject.SetActive(false);
    }
}
