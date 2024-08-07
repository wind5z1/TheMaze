using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//ステージ１のドアを制御するスクリプト
public class DoorControllerlvl1 : MonoBehaviour
{
    // 必要なスコア
    public int requiredScore = 100;
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
    // ドアの閉じた位置
    private Vector3 closedPosition;
    // ドアの開いた位置
    private Vector3 openPosition;
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
        // ドアの閉じた位置を取得
        closedPosition = transform.position;
        // ドアの開いた位置を設定
        openPosition = closedPosition + Vector3.up * 10.0f;
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
        if(currentScore >= requiredScore && inventory.numberOfKeys >= 1 && Input.GetKeyDown(KeyCode.O))
        {
            OpenDoor();
        }

        // プレイヤーが近くにいる場合
        if (isPlayerNear)
        {
            // スコアとキーの数が条件を満たしている場合
            if(currentScore >= requiredScore && inventory.numberOfKeys >= 1 && key.isPickedUp)
            {
                if (!isDoorOpen)
                {
                    textMeshPro.text = "Press 'O' to open";
                }
                else
                {
                    textMeshPro.text = "";
                    GameManager.instance.ResetScore();
                    inventory.numberOfKeys -= 1;
                }
            }
            else
            {
                // 条件を満たしていない場合のメッセージを表示
                string message = "Need " + " 1 key " + " and " + " 100 points " + " to open this gate.";
                message += "\nNow you have " + (inventory.numberOfKeys > 0 ? inventory.numberOfKeys + " key " : " 0 key ") + " and " + currentScore + " points.";
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
            textMeshPro.gameObject.SetActive(false);
            doorAudioSource.PlayOneShot(doorOpenSound);
            StartCoroutine(MoveDoor(openPosition));
        }
    }

    // ドアを動かすコルーチン
    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        float startTime = Time.time;
        while(Time.time - startTime < 1.0f)
        {
            float t = (Time.time - startTime) * openSpeed;
            transform.position = Vector3.Lerp(closedPosition, targetPosition, t);
            yield return null;
        }
        transform.position = targetPosition;
        textMeshPro.gameObject.SetActive(true);
    }
}
