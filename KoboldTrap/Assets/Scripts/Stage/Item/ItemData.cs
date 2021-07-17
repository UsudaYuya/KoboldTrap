using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// アイテム共通のデータ
/// </summary>
namespace KoboldTrap.Stage.Item
{
    public class Item
    {
        [System.Serializable]
        public struct Data
        {
            public string name;

            public int cost;

            public int hp;

            public float scale;

            public string explanation;

            public int player;

            public _Data[] data;

            public Sprite sprite(int p = -1) { if (p == -1) p = (int)StageController.player; return data[p].sprite; }
            public GameObject prefab(int p = -1) { if (p == -1) p = (int)StageController.player; return data[p].prefab; }
        }

        //プレイヤーによって変更されるアイテム
        [System.Serializable]
        public struct _Data
        {
            public Sprite sprite;

            public GameObject prefab;
        }
    }

    public class ItemData : MonoBehaviour
    {
        public static ItemData Instance;
        private void Awake() { Instance = this; }

        [SerializeField]
        private Item.Data[] data;
        public Item.Data[] _Data { get { return data; } set { data = value; } }
    }

    //public class ItemData : MonoBehaviour
    //{
    //[System.Serializable]
    //public struct Data
    //{
    //    public string name;//名前

    //    public GameObject prefab;//オブジェクト

    //    public Sprite[] sprite;

    //    public int cost;//コスト

    //    public int hp;//hp

    //    public float scale;//オブジェクトの大きさ

    //    public string explanation;//説明

    //    [HideInInspector]
    //    public Vector3 pos;//位置

    //    [HideInInspector]
    //    public int player;//設置したプレイヤー

    //    [HideInInspector]
    //    public bool isAttacked;

    //    public AudioClip putMusic;//設置する時の音
    //}

    //[SerializeField]
    //private Data[] data;
    //public  Data[] _Data { get { return data; } set { data = value; } }

    //}
}