using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KoboldTrap.Stage.Player
{

    /// <summary>
    /// アイテム,宝石,HP管理
    /// </summary>
    public class Status
    {
        public int[] items = { 0, 1, 7 };//保持しているアイテム
        public int coin = 10;//コイン
        public int hp = 10;//コイン

        public int crystal = 0;

        public float speed = 5;//移動速度
        public float jump = 5;//ジャンプ時の力

        //数値をリセットする
        public void Reset()
        {
            coin = 10;
            hp = 3;
            speed = 5;
            jump = 5;
        }

        //Noに設置されているアイテムの取得
        public Item.Item.Data ItemGet(int No) { return Item.ItemData.Instance._Data[items[No]]; }

        //アイテム番号の変更
        public void ItemChange(int itemNo, int playerNo) { items[playerNo] = itemNo; }

        //アイテムを置けるか
        public bool ItemPutChack(Item.Item.Data data)
        {
            if (coin <= data.cost)
                return false;
            coin -= data.cost;
            UIManager.Instance.Coin(coin, 10);
            return true;
        }
    }

    public class PlayerBehaviour : MonoBehaviour
    {
        public Status _status;

        [HideInInspector] public Rigidbody2D _rigid;
        [HideInInspector] public Animator _anima;
        [HideInInspector] public SpriteRenderer _hpGauge;
        [HideInInspector] public Animator _buffAnima;
        [HideInInspector] private SpriteRenderer _itemSprite;
        [HideInInspector] private AudioSource _audio;


        private void Start()
        {
            _status = new Status();//コンストラクタ
            _rigid = this.GetComponent<Rigidbody2D>();
            _anima = this.GetComponent<Animator>();
            _itemSprite = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
            _hpGauge = GameObject.Find("HPGauge").GetComponent<SpriteRenderer>();
            _buffAnima = transform.GetChild(1).GetComponent<Animator>();
            _audio = this.GetComponent<AudioSource>();

            _anima.SetBool("Frag", true);
            this.enabled = false;
        }

        /// <summary>
        /// 行動開始
        /// </summary>
        public void START()
        {
            ///-----------
            /// 位置の記録
            /// coin Hp UI のリセット
            /// 起きる動作の開始
            ///-----------

            Debug.Log("reset");
            //Hp Coin
            _status.Reset();

            //UIのリセット Item Coin Anima Hp Buff Item
            UIManager.Instance.ItemReset(_status.items);
            UIManager.Instance.Coin(_status.coin, 10);

            var size = _hpGauge.size;
            size.x = _status.hp;
            _hpGauge.size = size;

            _buffAnima.SetBool("SpeedUp", false);
            _buffAnima.SetBool("JumpUp", false);
            _anima.SetBool("Frag", false);

            //起きる動作
            StartCoroutine(GetUp());
        }

        /// <summary>
        /// 行動経過
        /// </summary>
        public virtual void UPDATE() { }

        /// 起きる動作　動作中は移動停止
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetUp()
        {
            _anima.SetBool("Frag", false);
            yield return null;
            _rigid.constraints = RigidbodyConstraints2D.FreezeAll;//移動の停止
            _anima.Rebind();//起きるアニメーションの開始

            yield return null;
            _anima.SetTrigger("GetUp");//起きるアニメーションの開始

            yield return new WaitForSeconds(0.1f);
            yield return null;
            //起きる動作のアニメーションのCord
            int AnimationCord = _anima.GetCurrentAnimatorStateInfo(0).tagHash;

            while (true)
            {
                //起きる動作のアニメーションの終了検知
                if (AnimationCord != _anima.GetCurrentAnimatorStateInfo(0).tagHash)
                {
                    //移動の固定の解除
                    _rigid.constraints = RigidbodyConstraints2D.None;
                    _rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                    yield break;
                }
                yield return null;
            }
        }

        /// <summary>
        /// カメラ
        /// </summary>
        public void PlayerCamera()
        {
            //カメラの位置
            Vector3 cameraPos = Camera.main.transform.position;

            cameraPos.x = this.transform.position.x;
            //最小値固定
            if (cameraPos.x < 0)
                cameraPos.x = 0;
            //最大値固定
            else if (cameraPos.x > 147)
                cameraPos.x = 147;

            Camera.main.transform.position = cameraPos;
        }

        /// <summary>
        /// 移動
        /// </summary>
        /// <param name="hor"></param>
        public void Movement(float hor)
        {
            //移動アニメーション
            _anima.SetFloat("Speed", Mathf.Abs(hor));

            //移動方向が入力されている場合
            if (hor != 0)
            {
                Vector2 angle = new Vector2(hor, 0);

                //移動方向にコライダーが存在しているかどうか
                RaycastHit2D hit = Physics2D.Raycast(this.transform.position, angle, 0.6f, LayerMask.GetMask("Stage"));

                if (!hit)
                {
                    //速度の適応
                    this.transform.position += Vector3.right * hor * _status.speed * Time.deltaTime;

                    //移動方向を向きます
                    if (hor > 0.1f)//右を向く
                    {
                        Quaternion rotation = this.transform.rotation;
                        rotation.y = 180;
                        this.transform.rotation = rotation;
                    }
                    else if (hor < -0.1f)//左を向く
                    {
                        Quaternion rotation = this.transform.rotation;
                        rotation.y = 0;
                        this.transform.rotation = rotation;
                    }
                }
            }
        }

        /// <summary>
        /// ジャンプ
        /// </summary>
        public void Jump()
        {
            if (IsGround())//接地判定
            {
                _rigid.velocity = Vector2.up * _status.jump;
            }
        }

        /// <summary>
        /// 接地判定
        /// </summary>
        /// <returns></returns>
        public bool IsGround()
        {
            //コライダーを設置する位置の取得
            Vector2 groundArea = (Vector2.left * 0.5f + Vector2.up * 0.1f);

            bool ground = Physics2D.OverlapArea(
                (Vector2)this.transform.position + groundArea,
                (Vector2)this.transform.position - groundArea,
                LayerMask.GetMask("Stage"));

            if (ground == _anima.GetBool("Sky"))
                Audio.Instance.Ground();//地面から離れた　接地したら音を鳴らす

            _anima.SetBool("Sky", !ground);

            //設置判定を返す
            return ground;
        }

        private float _attackCount;//構えている時間のカウント
        private bool _attack = false;
        /// <summary>
        /// 構える
        /// </summary>
        public void Hold()
        {
            if (IsGround())
            {
                _attack = true;//攻撃待機状態
                _anima.SetBool("Attack", true);//構えるアニメーション
                _attackCount += Time.deltaTime;//構えているカウント

                _rigid.constraints = RigidbodyConstraints2D.FreezeAll;//動きを止める
            }
        }

        /// <summary>
        /// 振り下ろす
        /// </summary>
        public void Behave()
        {
            if (_attackCount > 0.4f)
            {
                Collider2D[] col = Physics2D.OverlapCircleAll((Vector2)this.transform.position + Vector2.up, 1);
                foreach (Collider2D c in col)
                {
                    //接触がアイテムの場合接触判定
                    if (c.GetComponent<Item.ItemBehaviour>())
                        Destroy(c.gameObject);
                }
                Audio.Instance.Attack();//振り下ろす効果音
            }

            //攻撃のリセット
            if (_attack)
            {
                _attack = false;//攻撃未待機状態
                //動ける状態にする
                _rigid.constraints = RigidbodyConstraints2D.None;
                _rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

            _anima.SetBool("Attack", false);
            _attackCount = 0;
        }

        private bool hold = false;
        /// <summary>
        /// アイテム構え
        /// </summary>
        public void ItemHave(int index)
        {
            hold = true;
            //アイテムを構える
            Debug.Log(index);
            _itemSprite.sprite = _status.ItemGet(index).sprite();//構えるアイテム
            _anima.SetBool("Trap", true);
            _rigid.constraints = RigidbodyConstraints2D.FreezeAll;//移動の停止
        }

        /// <summary>
        /// アイテムを置く
        /// </summary>
        public void ItemPut(int index)
        {
            if (hold == false)
                return;

            hold = false;

            //動きの停止の解除
            _rigid.constraints = RigidbodyConstraints2D.None;
            _rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (_status.ItemPutChack(_status.ItemGet(index)))//アイテムを置けるかチェック
            {
                GameObject putItem = _status.ItemGet(index).prefab();
                //アイテムを設置したプレイヤーの記録
                putItem.GetComponent<Item.ItemBehaviour>().player = StageController.player;
                //アイテムの画像の変更
                putItem.GetComponent<SpriteRenderer>().sprite = _status.ItemGet(index).sprite();
                //アイテムを設置
                Instantiate(_status.ItemGet(index).prefab(), this.transform.GetChild(0)
                   .transform.position, Quaternion.identity);
            }

            //ホールドしているアイテムのリセット
            _itemSprite.sprite = null;
            _anima.SetBool("Trap", false);
        }

        /// <summary>
        /// 攻撃を受けた
        /// </summary>
        /// <param name="damage"></param>
        public void Damage(int damage)
        {
            if (StageController.state != StageController.STATE.Action)
                return;

            _status.hp -= damage;//HPを減らす

            //Hpの画像の更新
            var size = _hpGauge.size;
            size.x = _status.hp;
            _hpGauge.size = size;
            if (_status.hp <= 0) StageController.StateChange(StageController.STATE.ControllerChange);
        }

        //速度上昇
        public void SpeedUp() { _status.speed += 2; _buffAnima.SetBool("SpeedUp", true); }
        //ジャンプ力
        public void JumpUp() { _status.jump += 2; _buffAnima.SetBool("JumpUp", true); }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (StageController.state == StageController.STATE.Action && collision.gameObject.tag == "Goal")
            {
                //ゴール処理に変更
                StageController.StateChange(StageController.STATE.Goal);
                Audio.Instance.Goal();
                _anima.SetBool("Goal", true);

                //左を向く
                Quaternion rotation = this.transform.rotation;
                rotation.y = 0;
                this.transform.rotation = rotation;

                _status.crystal++;//宝石の数の追加
                UIManager.Instance.AddCrystal(_status.crystal);

                if (_status.crystal == 3)//宝石が最大になったらリザルトに変更
                    StageController.StateChange(StageController.STATE.Result);
            }
        }
    }
}

