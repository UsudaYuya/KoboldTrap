using UnityEngine;
using UnityEngine.UI;

namespace KoboldTrap.Stage
{
    public class StageController : MonoBehaviour
    {
        //現在の状態
        public enum STATE
        {
            Before,              //プレイ前
            Action,              //playerの行動中
            ControllerChange,    //コントローラーの変更
            ItemChange,          //アイテムの変更
            Goal,                //ゴール演出
            Result               //リザルト
        }
        public static STATE state = STATE.Before;

        //プレイしている方
        public enum PLAYER { A = 0, B = 1 }
        public static PLAYER player = PLAYER.A;

        [Header("プレイ前")]
        [SerializeField, Tooltip("プレイ前説明Image")] private GameObject[] _beforeImages = default;
        private int _beforeImageCount = 0;

        [Header("プレイ中")]
        [SerializeField, Tooltip("行動するキャラ")] public Player.PlayerBehaviour[] _actionBehaviours = new Player.PlayerBehaviour[2];
        [SerializeField, Tooltip("最大時間")] private float _maxTime = 5.0f;
        private float _time = 0;

        [Header("コントローラー変更")]
        [SerializeField, Tooltip("コントローラー変更")] private GameObject _controllerChangeUI = default;
        [SerializeField, Tooltip("ゲージ")] private Image _gauge = default;
        [SerializeField, Tooltip("ゲージ背景")] private Image _gaugeBackGround = default;
        [SerializeField, Tooltip("UISprite")] private Sprite[] sprite = new Sprite[2];

        [Header("アイテム変更")]
        [SerializeField, Tooltip("アイテム変更背景")] private GameObject _itemChangeBackGround = default;

        [Header("リザルト")]
        [SerializeField, Tooltip("リザルトUI")] private GameObject _resultUI = default;

        [Header("効果音")]
        [SerializeField] private AudioClip _submit = default;
        [SerializeField] private AudioClip _controllerChange = default;


        ResultController _resultController;
        ItemChange _itemChange;
        AudioSource _audio;
        private void Start()
        {
            state = STATE.Before;
            player = PLAYER.A;

            _resultController = GameObject.FindObjectOfType<ResultController>();
            _itemChange = GameObject.FindObjectOfType<ItemChange>();
            _audio = Camera.main.GetComponent<AudioSource>();
        }

        private void Update()
        {
            //ポーズ中は動かないようにする
            if (Time.timeScale == 0)
                return;

            //State固有処理
            switch (state)
            {
                case STATE.Before: { Before(); } break;
                case STATE.Action: { Action(); } break;
                case STATE.ControllerChange: { ControllerChange(); } break;
                case STATE.ItemChange: { ItemChange(); } break;
                case STATE.Goal: { Goal(); } break;
                case STATE.Result: { Result(); } break;
            }
        }

        /// <summary>
        /// 事前処理
        /// </summary>
        private void Before()
        {
            //--------------------------------------
            //　決定キーでページを進める
            //　すべてのページを見たらプレイ状態に変更する
            //--------------------------------------

            //ページ変更キー
            if (Input.GetButtonDown("Submit"))
            {
                _audio.PlayOneShot(_submit);//ページ変更の音声
                _beforeImageCount++;//開くページのカウント
                if (_beforeImages.Length <= _beforeImageCount)
                    state = STATE.Action;
            }

            //選択されているImageの表示
            for (int i = 0; i < _beforeImages.Length; i++)
            {
                if (i == _beforeImageCount)
                    _beforeImages[i].gameObject.SetActive(true);
                else
                    _beforeImages[i].gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// プレイ中処理
        /// </summary>
        private void Action()
        {
            //--------------------------------------
            //　プレイヤー開始処理
            //　プレイヤーの行動
            //　時間経過のカウント
            //--------------------------------------

            //プレイヤーがActiveかどうか
            if (_actionBehaviours[(int)player].enabled == false)
            {
                _actionBehaviours[(int)player].enabled = true;//プレイヤーの行動開始
                _actionBehaviours[(int)player].START();//開始処理
                _time = _maxTime;//時間のリセット
            }

            //プレイヤーの行動
            _actionBehaviours[(int)player].UPDATE();
            //時間経過
            _time -= Time.deltaTime;
            //経過表示
            UIManager.Instance.Stamina(_time, _maxTime);

            if (_time < 0)
            {
                //コントローラー変更状態に変更
                state = STATE.ControllerChange;
            }
        }

        private float count = 0;//コントローラー変更のカウント
        /// <summary>
        /// コントローラー変更
        /// </summary>
        private void ControllerChange()
        {
            //--------------------------------------
            //　決定キーカウント
            //　二秒押したらアイテム変更状態にする
            //--------------------------------------

            if (_controllerChangeUI.activeSelf == false)
            {
                _actionBehaviours[(int)player].enabled = false;
                _actionBehaviours[(int)player]._anima.SetBool("Frag", true);
                _actionBehaviours[(int)player]._buffAnima.SetBool("SpeedUp", false);
                _actionBehaviours[(int)player]._buffAnima.SetBool("JumpUp", false);
                //UIの表示
                _controllerChangeUI.SetActive(true);

                _gauge.sprite = sprite[(int)player];//ゲージを今プレイしているキャラの色にする
                //ゲージの背景を相手の色に変更
                _gaugeBackGround.sprite = sprite[(int)(player == PLAYER.A ? PLAYER.B : PLAYER.A)];

                count = 2;//ゲージのリセット
            }

            if (Input.GetButton("Submit"))
            {
                _audio.PlayOneShot(_controllerChange);
                _gauge.fillAmount = count / 2;
                count -= Time.deltaTime;
                if (count < 0)
                {
                    //UIの非表示
                    _controllerChangeUI.SetActive(false);
                    //プレイヤー変更
                    player = player == PLAYER.A ? PLAYER.B : PLAYER.A;
                    state = STATE.ItemChange;//アイテム変更状態にする
                }
                return;
            }
            count = 2;//カウントのリセット
        }

        /// <summary>
        /// アイテム変更
        /// </summary>
        private void ItemChange()
        {
            //--------------------------------------
            //　アイテム変更UIの表示
            //　アイテム変更状態の開始
            //　アイテム変更状態の継続処理
            //--------------------------------------
            if (!_itemChangeBackGround.activeSelf)
            {
                _itemChangeBackGround.SetActive(true);//アイテム変更UIの表示
                _itemChange.ItemChange_Start();//アイテム変更状態の開始
            }
            _itemChange.ItemChange_Update();//アイテム変更状態の継続処理
        }

        /// <summary>
        /// ゴール
        /// </summary>
        private void Goal()
        {
            //--------------------------------------
            //　左に移動
            //　画面外に移動したらコントローラー変更状態に
            //--------------------------------------

            //プレイヤーを左に移動させる
            _actionBehaviours[(int)player].transform.position += (Vector3)Vector2.left * Time.deltaTime * 4.0f;

            //画面外に移動した場合
            if (!_actionBehaviours[(int)player].GetComponent<SpriteRenderer>().isVisible)
            {
                _actionBehaviours[(int)player]._anima.SetBool("Goal", false);
                _actionBehaviours[(int)player]._anima.SetBool("Frag", true);
                state = STATE.ControllerChange;
            }
        }

        /// <summary>
        /// リザルト
        /// </summary>
        private void Result()
        {
            //--------------------------------------
            //　リザルトUIの表示
            //　リザルトの開始処理
            //　リザルトの継続処理
            //--------------------------------------
            if (!_resultUI.activeSelf)//リザルトUIの表示状態
            {
                _resultUI.SetActive(true);//リザルトUIの表示
                _resultController.ResultStart((int)player);//リザルト処理の開始
            }
            _resultController.ResultUpdate();
        }

        /// <summary>
        /// Stateの変更
        /// </summary>
        /// <param name="s">変更する状態</param>
        public static void StateChange(STATE s)
        {
            state = s;
        }
    }
}
