using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KoboldTrap.Stage
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        private void Awake() { Instance = this; }

        Item.ItemData itemData;
        private void Start()
        {
            itemData = GameObject.FindObjectOfType<Item.ItemData>();
        }

        //--------------------------
        //　スタミナ
        //--------------------------

        [Header("スタミナゲージ"), SerializeField] private Image _stamina = default;
        [Header("スタミナテキスト"), SerializeField] private Text _timeText = default;
        public void Stamina(float time, float maxTime)
        {
            _stamina.fillAmount = time / maxTime;
            _timeText.text = (int)time + "/" + (int)maxTime;
        }


        //--------------------------
        //　コイン
        //--------------------------
        [Header("コインゲージ"), SerializeField] private Image _coin = default;
        public void Coin(float coin, float maxCoin)
        {
            _coin.fillAmount = coin / maxCoin;
        }

        //--------------------------
        //　アイテム
        //--------------------------

        [Header("アイテムカーソル"), SerializeField] private RectTransform _itemCursor = default;
        [Header("アイテム画像"), SerializeField] private Image[] _itemImages = default;
        public void ItemReset(int[] items)
        {
            _itemCursor.anchoredPosition = _itemImages[0].rectTransform.anchoredPosition;

            for (int i = 0; i < 3; i++) _itemImages[i].sprite = itemData._Data[items[i]].sprite();
        }

        public void ItemCursor(int No)
        {
            _itemCursor.anchoredPosition = _itemImages[No].rectTransform.anchoredPosition;
        }

        //--------------------------
        //　ゴールcrystal
        //--------------------------
        [Header("宝石_プレイヤーA"), SerializeField] private Image[] _crystalA = default;
        [Header("宝石_プレイヤーB"), SerializeField] private Image[] _crystalB = default;

        public void AddCrystal(int crystal)
        {
            if (StageController.player == StageController.PLAYER.A)
                _crystalA[crystal - 1].gameObject.SetActive(true);
            else if (StageController.player == StageController.PLAYER.B)
                _crystalB[crystal - 1].gameObject.SetActive(true);
        }


    }
}
