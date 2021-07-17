using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KoboldTrap.Stage
{
    public class ItemChange : MonoBehaviour
    {
        private enum SELECT { Base, MyItem }
        private SELECT select = SELECT.Base;//現在の選択している状態

        [Header("アイテムの親"), SerializeField] private GameObject _itemImageParent = default;

        //変更するアイテムのデータ
        [System.Serializable]
        private class ChangeItem
        {
            [Tooltip("背景")] public RectTransform ground = default;    //背景
            [Tooltip("Image")] public Image image = default;          //アイテムの画像
            [Tooltip("名前")] public Text name = default;               //名前
            [Tooltip("コスト")] public Text cost = default;          //コスト
            [Tooltip("破壊までの攻撃回数")] public Text hp = default;  //破壊までのHp
            [Tooltip("説明")] public Text description = default;        //説明
        }
        [Header("変更するアイテムのUI"),SerializeField] private ChangeItem[] changeItemUI = default;

        [Header("プレイヤーのアイテムのUI"),SerializeField] private Image[] _myItemUI = new Image[3];

        //カーソル
        [Header("0:変更するアイテム 1:プレイヤーのアイテム"),SerializeField] private RectTransform[] _cursor = new RectTransform[2];
        private int _selectIndex = 0;//選択している番号
        private int _changeItemNo = 0;//変更するアイテムの番号

        //変更するアイテムの番号
        private int[] _changeNum = new int[3];

        StageController _stageController;
        Item.ItemData _itemData;
        private void Start()
        {
            _itemData = GameObject.FindObjectOfType<Item.ItemData>();
            _stageController = GameObject.FindObjectOfType<StageController>();
        }

        public void ItemChange_Start()
        {
            //交換するアイテムの番号の取得
            _changeNum[0] = Random.Range(0, _itemData._Data.Length);
            do
            {
                _changeNum[1] = Random.Range(0, _itemData._Data.Length);
            } while (_changeNum[0] == _changeNum[1]);
            do
            {
                _changeNum[2] = Random.Range(0, _itemData._Data.Length);
            } while (_changeNum[0] == _changeNum[2] || _changeNum[1] == _changeNum[2]);

            //変更するアイテムのUIの変更
            for (int i = 0; i < changeItemUI.Length; i++)
            {
                changeItemUI[i].image.sprite = _itemData._Data[_changeNum[i]].sprite();
                changeItemUI[i].name.text = _itemData._Data[_changeNum[i]].name;
                changeItemUI[i].cost.text = _itemData._Data[_changeNum[i]].cost.ToString();
                changeItemUI[i].hp.text = _itemData._Data[_changeNum[i]].hp.ToString();
                changeItemUI[i].description.text = _itemData._Data[_changeNum[i]].explanation;
            }

            //交換先のアイテムのUIの設定
            //プレイヤーのアイテムImageの変更
            for (int i = 0; i < 3; i++)
                _myItemUI[i].sprite = _stageController._actionBehaviours[(int)StageController.player]._status.ItemGet(i).sprite();
            //操作のリセット
            _selectIndex = 0;

            //プレイヤーのアイテムのカーソル
            _cursor[1].anchoredPosition = _myItemUI[0].GetComponent<RectTransform>().anchoredPosition;
        }

        public void ItemChange_Update()
        {
            /*選択*/
            _selectIndex += IndexChange(_selectIndex, 0, 2);

            CursorSet();

            //決定キー操作
            if (Input.GetButtonDown("Submit"))
            {
                Audio.Instance.Submit();

                //プレイヤーのあアイテム変更状態に変更
                if (select == SELECT.Base)
                {
                    select = SELECT.MyItem;
                    _changeItemNo = _selectIndex;
                }
                else if (select == SELECT.MyItem)
                {
                    _stageController._actionBehaviours[(int)StageController.player]._status.
                        ItemChange(_changeNum[_changeItemNo], _selectIndex);
                    StageController.StateChange(StageController.STATE.Action);
                    _itemImageParent.SetActive(false);
                }
                _selectIndex = 0;
            }
        }

        private bool operate = false;//操作を行うかどうか
        /// <summary>
        /// Indexの変更
        /// </summary>
        /// <param name="index">現在のIndex</param>
        /// <param name="min">最低値</param>
        /// <param name="max">最大値</param>
        /// <returns></returns>
        private int IndexChange(int index, int min, int max)
        {
            float key = Input.GetAxis("Horizontal");

            if (operate)
            {
                if (key > 0.4f && index != max)
                {
                    Audio.Instance.Select();
                    operate = false;
                    return 1;
                }
                else if (key < -0.4f && index != min)
                {
                    Audio.Instance.Select();
                    operate = false;
                    return -1;
                }
            }
            else if (key < 0.4f && key > -0.4f)
            {
                operate = true;
            }
            return 0;
        }

        /// <summary>
        /// カーソルの位置の設定
        /// </summary>
        private void CursorSet()
        {
            if (select == SELECT.Base)
                _cursor[0].anchoredPosition = changeItemUI[_selectIndex].ground.anchoredPosition;
            else
                _cursor[1].anchoredPosition = _myItemUI[_selectIndex].GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
