using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KoboldTrap.Stage
{
    public class ResultController : MonoBehaviour
    {
        [Header("アニメーション"),SerializeField] private Animator[] _playerAnima = new Animator[2];
        [Header("勝利画像"),SerializeField] private Sprite[] _winSprite = new Sprite[2];
        [Header("勝利のUI"),SerializeField] private Image _winImage = default;
        [Header("0:Retry 1:Title"),SerializeField] private RectTransform[] _selectImage = default;

        [SerializeField] private AudioClip _resultStart = default;
        int index = 0;

        AudioSource _audio;
        private void Start()
        {
            _audio = this.GetComponent<AudioSource>();
        }

        /// <summary>
        /// リザルトの開始
        /// </summary>
        /// <param name="winNo"></param>
        public void ResultStart(int winNo)
        {
            //勝利画像
            _winImage.sprite = _winSprite[winNo];

            //勝利アニメーション
            _playerAnima[winNo].SetBool("Win", winNo == 0);
            _playerAnima[winNo].SetBool("Win", winNo == 1);

            _audio.PlayOneShot(_resultStart);//リザルトの開始音
            index = 0;
        }

        /// <summary>
        /// リザルトの状態
        /// </summary>
        public void ResultUpdate()
        {
            index = IndexChange(index);//Indexの変更
            SelectImageScele(index);//選択しているボタンの表記

            if (Input.GetButtonDown("Submit"))
            {
                Audio.Instance.Submit();//決定音
                if (index == 0)
                    Retry();
                else
                    Title();
            }
        }


        /// <summary>
        /// 選択されているImageの大きさの変更
        /// </summary>
        private void SelectImageScele(int index)
        {
            for (int i = 0; i < _selectImage.Length; i++)
            {
                if (index == i)
                    _selectImage[i].localScale = new Vector3(1.2f, 1.2f, 1.2f);
                else
                    _selectImage[i].localScale = new Vector3(1, 1, 1);
            }
        }

        /// <summary>
        /// indexの変更
        /// </summary>
        private int IndexChange(int index)
        {
            float hor = Input.GetAxis("Horizontal");
            if (hor > 0.4f)
            {
                if (index == 0)
                {
                    Audio.Instance.Select();
                    return 1;
                }
            }
            else if (hor < -0.4f)
            {
                if (index == 1)
                {
                    Audio.Instance.Select();
                    return 0;
                }
            }
            return index;
        }

        /// <summary>
        /// リトライ
        /// </summary>
        private void Retry()
        {
            SceneManager.LoadScene("Stage");
        }

        /// <summary>
        /// タイトルに戻る
        /// </summary>
        private void Title()
        {
            SceneManager.LoadScene("Title");
        }
    }
}
