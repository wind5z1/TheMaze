using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//プレイヤーの鍵を管理するスクリプト
public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;   // シングルトンインスタンス

    public List<Key> key = new List<Key>();   // 鍵のリスト
    public int numberOfKeys = 0;              // 鍵の数

    private void Awake()
    {
        instance = this;  // シングルトンインスタンスを設定する
    }

    // アイテムをインベントリに追加する
    public void AddItem(Key item)
    {
        key.Add(item);          // アイテムをリストに追加する
        numberOfKeys++;         // アイテムの数を増やす
    }

    // アイテムを使用する
    public void UseItem(Key item)
    {
        if (key.Contains(item))  // アイテムがリストに含まれているか確認する
        {
            key.Remove(item);    // リストからアイテムを削除する
            numberOfKeys--;      // アイテムの数を減らす
        }
    }


}
