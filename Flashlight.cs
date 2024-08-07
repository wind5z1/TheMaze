using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//懐中電灯のオブジェクトを制御するスクリプト
public class Flashlight : MonoBehaviour
{
    // 懐中電灯のメッシュレンダラー
    private MeshRenderer flashlightRenderer;
    // 懐中電灯が点灯しているかどうか
    private bool isFlashlightOn = false;
    // 懐中電灯の音
    private AudioSource flashlightSound;
    // 懐中電灯の音のクリップ
    public AudioClip flashlightclip;

    // 初期設定
    void Start()
    {
        // メッシュレンダラーを取得し、初期状態で非表示に設定
        flashlightRenderer = GetComponent<MeshRenderer>();
        flashlightRenderer.enabled = false;
        
        // オーディオソースを取得
        flashlightSound = GetComponent<AudioSource>();
        
        // オーディオソースが存在しない場合、新たに追加
        if(flashlightSound == null)
        {
            flashlightSound = gameObject.AddComponent<AudioSource>();
            flashlightSound.clip = flashlightclip;
        }
    }

    // 毎フレーム呼び出される
    void Update()
    {
        // 「F」キーが押された場合
        if (Input.GetKeyDown(KeyCode.F))
        {
            // 懐中電灯の状態を切り替え
            isFlashlightOn = !isFlashlightOn;
            flashlightRenderer.enabled = isFlashlightOn;
            
            // 懐中電灯が点灯した場合、音を再生
            if (isFlashlightOn)
            {
                flashlightSound.PlayOneShot(flashlightclip);
            }
        }
    }
}
