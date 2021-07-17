using System.Collections;
using UnityEngine;

namespace KoboldTrap.Stage.Item
{
    public class Landmines : ItemBehaviour
    {
        ////とらばさみ　プレイヤーと接触でダメージ
        private float _time;

        Animator _anima;

        private void Start()
        {
            this.GetComponent<AudioSource>().PlayOneShot(putAudio);
            _anima = this.GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            if (player != StageController.player)
            {
                if (!Physics2D.Raycast(this.transform.position, Vector2.down, this.transform.localScale.y / 2 + 0.1f, LayerMask.GetMask("Stage")))
                {
                    //重力加算用時間の適応
                    _time += Time.deltaTime;
                    //重力を加える
                    this.transform.position += (((Vector3)Physics2D.gravity * 0.8f) * _time) * Time.deltaTime;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (player != StageController.player)
            {
                //接触したオブジェクトがプレイヤーの場合
                if (collision.GetComponent<Player.Player>())
                {
                    Audio.Instance.Damage();
                    //効果を適応するプレイヤーを送る
                    collision.GetComponent<Player.PlayerBehaviour>().Damage(1);
                    _anima.SetTrigger("play");//閉まるアニメーションの開始
                }
            }
        }

        /// <summary>
        /// 破壊処理
        /// </summary>
        public void Ravage()
        {
            Destroy(this.gameObject);//破壊
        }

        //private float time;//重力処理用の時間経過

        //Animator anima;
        //Item item;
        //private void Start()
        //{
        //    item = this.GetComponent<Item>();
        //    anima = this.GetComponent<Animator>();
        //}

        //private void FixedUpdate()
        //{
        //    if (item.data.player != (int)StageController.player)
        //    {
        //        if (!Physics2D.Raycast(this.transform.position, Vector2.down, this.transform.localScale.y / 2 + 0.1f, LayerMask.GetMask("Stage")))
        //        {
        //            //重力加算用時間の適応
        //            time += Time.deltaTime;
        //            //重力を加える
        //            this.transform.position += (((Vector3)Physics2D.gravity * 0.8f) * time) * Time.deltaTime;
        //        }
        //    }
        //}

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (item.data.player != (int)StageController.player)
        //    {
        //        if (StageController.state != StageController.STATE.Action)//プレイ中以外は接触判定を行わない
        //            return;

        //        //接触したオブジェクトがプレイヤーの場合
        //        if (collision.GetComponent<Player.Player>())
        //        {
        //            //効果を適応するプレイヤーを送る
        //            item.ItemEfect(collision.GetComponent<Player.Player>());
        //            anima.SetTrigger("play");//閉まるアニメーションの開始
        //        }
        //    }
        //}

        ///// <summary>
        ///// 破壊処理
        ///// </summary>
        //public void Ravage()
        //{
        //    Destroy(this.gameObject);//破壊
        //}
    }
}
