using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーに設定するデータ
/// </summary>
namespace KoboldTrap.Stage.Player
{
    public class Enemy : PlayerBehaviour
    {
        private void Start()
        {
            this.enabled = false;
        }

        public override void UPDATE()
        {
            Debug.Log("ENEMY_UPDATE");
        }


    }


    //[System.Serializable]
    //public class PlayerStutas
    //{
    //    public float speed = 5.0f;//移動速度
    //    public float jump = 6.0f;//ジャンプ時の力
    //    public float dash = 5.0f;//ダッシュ時の力
    //    public int attak = 2;//攻撃速度

    //    //上昇する数値
    //    public float speedUp = 1.2f;//
    //    public float speedDown = 0.2f;
    //    public float jumpUp = 1.7f;

    //    //上昇している時間
    //    public float SpeedUpTime;
    //    public float SpeedDownTime;
    //    public float JumpUpCount;

    //    public Animator efectAnima;

    //    /// <summary>
    //    /// 移動速度を返す
    //    /// </summary>
    //    /// <returns></returns>
    //    public float Speed(PlayerStutas stutas)
    //    {
    //        float s = speed;

    //        if (stutas.SpeedUpTime <= 0)//速度未上昇中
    //        {
    //            stutas.SpeedUpTime = 0;
    //            efectAnima.SetBool("SpeedUp", false);
    //        }
    //        else//速度上昇中
    //        {
    //            efectAnima.SetBool("SpeedUp", true);
    //            stutas.SpeedUpTime -= Time.deltaTime;
    //            s *= speedUp;//速度上昇中のstatusの追加
    //        }

    //        if (stutas.SpeedDownTime <= 0)//速度未上昇中
    //        {
    //            stutas.SpeedDownTime = 0;
    //            efectAnima.SetBool("SpeedDown", false);
    //        }
    //        else//速度上昇中
    //        {
    //            efectAnima.SetBool("SpeedDown", true);
    //            stutas.SpeedDownTime -= Time.deltaTime;
    //            s *= speedDown;//速度上昇中のstatusの追加
    //        }

    //        return s;
    //    }
    //    /// <summary>
    //    /// ジャンプ力を返す
    //    /// </summary>
    //    /// <returns></returns>
    //    public float Jump(PlayerStutas stutas)
    //    {
    //        float j = jump;
    //        if (stutas.JumpUpCount <= 0)//速度未上昇中
    //        {
    //            stutas.JumpUpCount = 0;
    //            efectAnima.SetBool("JumpUp", false);
    //        }
    //        else//速度上昇中
    //        {
    //            efectAnima.SetBool("JumpUp", true);
    //            stutas.JumpUpCount--;
    //            j *= jumpUp;//速度上昇中のstatusの追加
    //        }

    //        return j;
    //    }
    //}

    //[System.Serializable]
    //public class Data
    //{
    //    public int[] item = { 0, 1, 2 };//アイテム番号
    //    [HideInInspector]
    //    public int itemIndex = 0;//現在選択しているアイテムの番号

    //    [HideInInspector]
    //    public Vector2 pos;//復活位置

    //    public int crystal = 0;//宝石の数
    //}
    //public class Player : MonoBehaviour
    //{
    //    [Header("プレイヤーのstatus")]
    //    [SerializeField] private PlayerStutas stutas = new PlayerStutas();
    //    [SerializeField] public Data data = new Data();

    //    [SerializeField] private SpriteRenderer itemSpriteRenderer;
    //    Rigidbody2D rigid;
    //    Animator anima;
    //    new AudioSource audio;

    //    Item.ItemData itemData;
    //    Item.ItemController itemController;
    //    PlayerController playerController;

    //    private void Awake()
    //    {
    //        rigid = this.GetComponent<Rigidbody2D>();
    //        anima = this.GetComponent<Animator>();
    //        audio = this.GetComponent<AudioSource>();

    //        itemData = GameObject.FindObjectOfType<Item.ItemData>();
    //        itemController = GameObject.FindObjectOfType<Item.ItemController>();
    //        playerController = GameObject.FindObjectOfType<PlayerController>();
    //    }


    //    private void Update()
    //    {
    //        Ground();//接地判定

    //        EfectReset();//効果のリセット
    //    }


    //    /// <summary>
    //    /// 起きる動作　動作中は移動停止
    //    /// </summary>
    //    /// <returns></returns>
    //    public IEnumerator GetUp()
    //    {
    //        this.gameObject.SetActive(true);//プレイヤーの表示
    //        rigid.constraints = RigidbodyConstraints2D.FreezeAll;//移動の停止
    //        anima.SetTrigger("GetUp");//起きるアニメーションの開始

    //        yield return new WaitForSeconds(0.1f);
    //        yield return null;
    //        //起きる動作のアニメーションのCord
    //        int AnimationCord = anima.GetCurrentAnimatorStateInfo(0).tagHash;

    //        while (true)
    //        {
    //            //起きる動作のアニメーションの終了検知
    //            if (AnimationCord != anima.GetCurrentAnimatorStateInfo(0).tagHash)
    //            {
    //                //移動の固定の解除
    //                rigid.constraints = RigidbodyConstraints2D.None;
    //                rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

    //                yield break;
    //            }
    //            yield return null;
    //        }
    //    }


    //    /// <summary>
    //    /// 移動処理
    //    /// </summary>
    //    /// <param name="ver">左右の入力</param>
    //    public void Movement(float ver)
    //    {
    //        if (rigid.constraints == RigidbodyConstraints2D.FreezeAll)
    //            return;
    //        //移動アニメーション
    //        anima.SetFloat("Speed", Mathf.Abs(ver));
    //        //移動速度
    //        float speed = stutas.Speed(stutas);

    //        //移動方向が入力されている場合
    //        if (ver != 0)
    //        {
    //            Vector2 angle = new Vector2(ver, 0);

    //            //移動方向にコライダーが存在しているかどうか
    //            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, angle, 0.6f, LayerMask.GetMask("Stage"));

    //            if (!hit)
    //            {
    //                //速度の適応
    //                this.transform.position += Vector3.right * ver * speed * Time.deltaTime;

    //                //移動方向を向きます
    //                if (ver > 0.1f)//右を向く
    //                {
    //                    Quaternion rotation = this.transform.rotation;
    //                    rotation.y = 180;
    //                    this.transform.rotation = rotation;
    //                }
    //                else if (ver < -0.1f)//左を向く
    //                {
    //                    Quaternion rotation = this.transform.rotation;
    //                    rotation.y = 0;
    //                    this.transform.rotation = rotation;
    //                }
    //            }
    //        }
    //    }

    //    [SerializeField] private AudioClip jump = null;
    //    /// <summary>
    //    /// ジャンプ
    //    /// </summary>
    //    public void Jump()
    //    {
    //        if (Ground())//接地判定
    //        {
    //            audio.PlayOneShot(jump);
    //            float jumpForce = stutas.Jump(stutas);
    //            rigid.velocity = Vector2.up * jumpForce;
    //        }
    //    }

    //    private bool attackHold = false;
    //    /// <summary>
    //    /// 攻撃を構える
    //    /// </summary>
    //    public void Attack_Hold()
    //    {
    //        anima.SetBool("Attack", true);
    //        attackHold = true;
    //    }

    //    [SerializeField] private AudioClip attack = null;//攻撃効果音
    //    /// <summary>
    //    /// つるはしを振り下ろす
    //    /// </summary>
    //    public void Attack_behave()
    //    {
    //        if (attackHold)
    //        {
    //            attackHold = false;

    //            audio.PlayOneShot(attack);
    //            anima.SetBool("Attack", false);
    //        }
    //    }

    //    [SerializeField] private AudioClip itemChange;

    //    /// <summary>
    //    /// アイテムのIndexの変更
    //    /// </summary>
    //    /// <param name="add">true + 1  false -1</param>
    //    public void ItemIndex(bool add)
    //    {
    //        if (add)
    //        {
    //            if (data.itemIndex != data.item.Length - 1)
    //            {
    //                audio.PlayOneShot(itemChange);
    //                data.itemIndex++;
    //                playerController.ActionUIReset(data.itemIndex);
    //            }
    //        }
    //        else
    //        {
    //            if (data.itemIndex != 0)
    //            {
    //                audio.PlayOneShot(itemChange);
    //                data.itemIndex--;
    //                playerController.ActionUIReset(data.itemIndex);
    //            }
    //        }
    //    }


    //    /// <summary>
    //    ///　アイテムを持つアニメーション
    //    /// </summary>
    //    public void ItemHaveAnima()
    //    {
    //        //アイテムを構える
    //        anima.SetBool("Trap", true);
    //    }

    //    /// <summary>
    //    /// アイテムを持つ
    //    /// </summary>
    //    public void ItemHave()
    //    {
    //        itemSpriteRenderer.sprite = itemData._Data[data.item[data.itemIndex]].sprite[(int)StageController.player];
    //        Debug.Log(itemData._Data[data.item[data.itemIndex]].sprite[(int)StageController.player]);
    //    }

    //    /// <summary>
    //    /// アイテムを設置するアニメーション
    //    /// </summary>
    //    public void ItemPutAnima()
    //    {
    //        itemSpriteRenderer.sprite = null;
    //        anima.SetBool("Trap", false);
    //    }

    //    /// <summary>
    //    /// アイテムを設置する
    //    /// </summary>
    //    public void ItemPut()
    //    {
    //        if (playerController.playData.coin >= itemData._Data[data.item[data.itemIndex]].cost)
    //        {
    //            playerController.playData.coin -= itemData._Data[data.item[data.itemIndex]].cost;//コストの削減
    //            itemController.ItemPut(itemSpriteRenderer.transform.position, data.item[data.itemIndex]);//アイテムを設置する
    //        }
    //    }



    //    /// <summary>
    //    /// 設置判定
    //    /// </summary>
    //    private bool Ground()
    //    {
    //        //コライダーを設置する位置の取得
    //        Vector2 groundArea = (Vector2.left * 0.5f + Vector2.up * 0.1f);

    //        bool ground = Physics2D.OverlapArea(
    //            (Vector2)this.transform.position + groundArea,
    //            (Vector2)this.transform.position - groundArea,
    //            LayerMask.GetMask("Stage"));

    //        anima.SetBool("Sky", !ground);

    //        //設置判定を返す
    //        return ground;
    //    }

    //    /// <summary>
    //    /// アイテムのアニメーションを行わない時
    //    /// </summary>
    //    public void EfectReset()
    //    {
    //        //効果エフェクトの終了検知
    //        if (stutas.SpeedUpTime == 0 && stutas.SpeedDownTime == 0 && stutas.JumpUpCount == 0)
    //            stutas.efectAnima.gameObject.SetActive(false);
    //        else
    //            stutas.efectAnima.gameObject.SetActive(true);
    //    }

    //    [SerializeField] private AudioClip damage;
    //    /// <summary>
    //    /// 速度上昇の時間の追加
    //    /// </summary>
    //    /// <param name="time"></param>
    //    public void SpeedUp(float time)
    //    {
    //        audio.PlayOneShot(damage);//接触時の音
    //        stutas.SpeedUpTime += time;
    //    }
    //    /// <summary>
    //    /// 速度低下の時間の追加
    //    /// </summary>
    //    /// <param name="time"></param>
    //    public void SpeedDown(float time)
    //    {
    //        audio.PlayOneShot(damage);//接触時の音
    //        stutas.SpeedDownTime += time;
    //    }
    //    /// <summary>
    //    /// ジャンプ力上昇のカウントの追加
    //    /// </summary>
    //    /// <param name="count"></param>
    //    public void JumpUp(float count)
    //    {
    //        audio.PlayOneShot(damage);//接触時の音
    //        stutas.JumpUpCount += count;
    //    }

    //    [SerializeField] private AudioClip goal;
    //    private void OnTriggerEnter2D(Collider2D collision)
    //    {
    //        if (collision.gameObject.tag == "Goal")
    //        {
    //            audio.PlayOneShot(goal);
    //            GameObject.FindObjectOfType<StageController>().GoalStart(this.gameObject);
    //            anima.SetBool("Goal", true);
    //        }
    //    }

    //    /// <summary>
    //    /// animationを初期状態に戻す
    //    /// </summary>
    //    public void AnimatorReset()
    //    {
    //        anima.SetBool("Goal",false);
    //        anima.SetTrigger("Reset");
    //    }
}
