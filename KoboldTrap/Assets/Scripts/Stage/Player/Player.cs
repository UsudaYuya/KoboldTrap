using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーに設定するデータ
/// </summary>
namespace KoboldTrap.Stage.Player
{
    public class Player : PlayerBehaviour
    {
        public int itemIndex;

        public override void UPDATE()
        {
            _hpGauge.transform.position = this.transform.position + (Vector3.up * 1.5f) + (Vector3.left * 0.5f);
            //カメラの挙動
            PlayerCamera();

            //攻撃
            if (Input.GetAxisRaw("Attak") == 0) Behave();
            //罠を置く
            if (Input.GetAxisRaw("Trap") == 0) ItemPut(itemIndex);

            //攻撃構え
            if (Input.GetAxisRaw("Attak") == 1) Hold();
            //プレイヤーの行動停止条件
            if (_rigid.constraints == RigidbodyConstraints2D.FreezeAll)
                return;

            //移動
            Movement(Input.GetAxis("Horizontal"));

            //設置状態
            IsGround();

            //ジャンプ
            if (Input.GetButtonDown("Jump")) Jump();

            //罠構え
            if (Input.GetAxisRaw("Trap") == 1) ItemHave(itemIndex);


            //アイテムの移動
            if (Input.GetButtonDown("RB"))//右移動
                ItemIndex(true);
            if (Input.GetButtonDown("LB"))//左移動
                ItemIndex(false);
    }

    /// <summary>
    /// アイテムのIndexの変更
    /// </summary>
    /// <param name="add">true + 1  false -1</param>
    public void ItemIndex(bool add)
    {
        if (add)
        {
            if (itemIndex != 2)
            {
                itemIndex++;
                UIManager.Instance.ItemCursor(itemIndex);
            }
        }
        else
        {
            if (itemIndex != 0)
            {
                itemIndex--;
                UIManager.Instance.ItemCursor(itemIndex);
            }
        }
    }
}
}
