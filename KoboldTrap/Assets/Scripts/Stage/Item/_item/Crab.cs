using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KoboldTrap.Stage.Item
{
    public class Crab : ItemBehaviour
    {
        //こうせきがに　地面を左右移動する　接触で移動速度の低下

        private float time;//重力計算用の時間
        private Vector2 angle;

        private Item item;
        [SerializeField] private float speed = 2.0f;

        //攻撃終了時のsprite
        [SerializeField] private Sprite attackSprite = default;
        private void Start()
        {
            this.GetComponent<AudioSource>().PlayOneShot(putAudio);
            item = this.GetComponent<Item>();
            angle = Vector2.right;
        }

        private void FixedUpdate()
        {
            if (player != StageController.player)
            {
                /*重力計算*/
                if (!Physics2D.Raycast(this.transform.position, Vector2.down, this.transform.localScale.y / 2 + 0.1f, LayerMask.GetMask("Stage")))
                {
                    time += Time.deltaTime;
                    //重力を加える
                    this.transform.position += ((Vector3)Physics2D.gravity * time) * Time.deltaTime;
                }

                //左右移動を行います
                var rayStartPosition = this.transform.position;
                if (transform.localScale.x > 1)
                {
                    this.transform.position += (Vector3)angle * speed * Time.deltaTime;
                    rayStartPosition = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
                }
                else
                {
                    this.transform.position += (Vector3)angle * 2.0f * Time.deltaTime;
                }
                if (Physics2D.Raycast(rayStartPosition, angle, 0.6f, LayerMask.GetMask("Stage")))
                {
                    angle = angle * -1;//移動する方向を変えます
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (player != StageController.player)
            {
                if (StageController.state != StageController.STATE.Action)//プレイ中以外は接触判定を行わない
                    return;

                //接触したオブジェクトがプレイヤーの場合
                if (collision.GetComponent<Player.Player>())
                {
                    Audio.Instance.Damage();
                    //効果を適応するプレイヤーを送る
                    collision.GetComponent<Player.PlayerBehaviour>().Damage(1);
                    //攻撃したSpriteに変更
                    this.GetComponent<SpriteRenderer>().sprite = attackSprite;
                    Invoke("Ravage", 0.1f);
                }
            }
        }

        /// <summary>
        /// オブジェクトの破壊
        /// </summary>
        private void Ravage()
        {
            Destroy(this.gameObject);
        }

    
    }
}
