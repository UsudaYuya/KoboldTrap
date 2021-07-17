using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KoboldTrap.Title
{
    public class TitleController : MonoBehaviour
    {
        enum DisplayMode
        {
            Title, GameEnd
        }
        DisplayMode _displayMode = DisplayMode.Title;

        enum IndexPosition
        {
            Left, Right
        }
        /// <summary>タイトルモード・終了モードで共有されるIndex </summary>
        IndexPosition _buttonIndex = IndexPosition.Right;

        [System.Serializable]
        class ButtonImage
        {
            private const int _buttonLength = 2;

            [Header("大きさ2")]
            public Image[] _buttons = new Image[_buttonLength];

            /// <summary>モード切り替え時にオンオフを切り替える</summary>
            /// <param name="isActive"></param>
            public void ManageButtonsActive(bool isActive)
            {
                for (int i = 0; i < _buttons.Length; i++)
                {
                    _buttons[i].gameObject.SetActive(isActive);
                }
            }
        }

        [Header("タイトルモード 0:スタート 1:エンド , ゲーム終了確認モード : 0:タイトル 1:エンド")]
        [SerializeField] private ButtonImage[] _systemButtons = null;

        [Header("タイトルモード , ゲーム終了確認モード")]
        [SerializeField] private Image[] _backgroundImages = null;

        [SerializeField] private AudioClip cursorMove = null;
        [SerializeField] private AudioClip submit = null;
        [SerializeField] private AudioClip start = null;

        AudioSource audioSource;
        private void Start()
        {
            audioSource = this.GetComponent<AudioSource>();
            StartCoroutine(Title());
        }

        IEnumerator Title()
        {
            while (!JudgeStartInput())
            {
                yield return null;
            }
            float outTIme = 1f;
            float changeWait = 0.7f;
            float inTime = 1f;
            Fader.FadeInAndOutBlack(inTime, changeWait, outTIme);
            yield return new WaitForSeconds(inTime);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Stage");
        }

        private bool JudgeStartInput()
        {
            //ポーズ中は何も行わない
            if (Mathf.Approximately(Time.timeScale, 0f))
                return false;

            switch (_displayMode)
            {
                case DisplayMode.Title:
                    if (Input.GetButtonDown("Submit"))
                    {

                        if(IsStartInputPosition()) 
                        return true;
                    }
                    MoveButtonIndex();
                    ChangeButtonSize();
                    break;
                case DisplayMode.GameEnd:
                    if (Input.GetButtonDown("Submit"))
                    {
                        audioSource.clip = submit;
                        audioSource.Play();

                        GameEndOrTitleInput();
                    }
                    MoveButtonIndex();
                    ChangeButtonSize();
                    break;
                default:
                    break;
            }
            return false;
        }

        /// <summary>ボタンindexを移動する モード遷移後もIndexの値は引き継がれる</summary>
        private void MoveButtonIndex()
        {
            int buttonIndex = (int)_buttonIndex;

            buttonIndex = key(buttonIndex);
            _buttonIndex = (IndexPosition)buttonIndex;
        }

        bool select = false; 
        private int key(int index)
        {
            float hor = Input.GetAxis("Horizontal");
            if (select)
            {
                if (hor > 0.6f || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (index != 1)
                    {
                        AudioSource.PlayClipAtPoint(cursorMove, Camera.main.transform.position);
                        AudioSource.PlayClipAtPoint(cursorMove, Camera.main.transform.position);

                        index++;
                        select = false;
                    }
                }
                else if (hor < -0.6f || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (index != 0)
                    {
                        AudioSource.PlayClipAtPoint(cursorMove, Camera.main.transform.position);
                        AudioSource.PlayClipAtPoint(cursorMove, Camera.main.transform.position);

                        index--;
                        select = false;
                    }
                }
            }
            else if (Mathf.Abs(hor) < 0.2f)
            {
                select = true;
            }
            return index;
        }

        /// <summary>ボタンの大きさを変更する</summary>
        private void ChangeButtonSize()
        {
            var nowButtons = _systemButtons[(int)_displayMode]._buttons;
            for (int i = 0; i < nowButtons.Length; i++)
            {
                //拡大処理
                if (i == (int)_buttonIndex)
                {
                    var bigButtonSize = 1.2f;
                    var normalSize = 1f;
                    nowButtons[i].rectTransform.localScale = new Vector3(bigButtonSize, bigButtonSize, normalSize);
                }
                else
                {
                    var normalSize = 1f;
                    nowButtons[i].rectTransform.localScale = new Vector3(normalSize, normalSize, normalSize);
                }
            }
        }

        /// <summary>指定したモードへ移動 現状の背景イメージをオフにし、変更後のイメージをオンにする
        /// タイトルボタンの表示を切り替える</summary>
        /// <param name="changedDisplayMode">変更後の画面の状態</param>
        private void ChangeBackGround(DisplayMode changedDisplayMode)
        {
            //タイトル画面に移動する際は、上に出ているイメージをオフにする
            if (changedDisplayMode == DisplayMode.Title)
            {
                _backgroundImages[(int)_displayMode].gameObject.SetActive(false);
                //タイトルボタンの表示を切り替える
                _systemButtons[(int)DisplayMode.Title].ManageButtonsActive(true);
            }
            else
            {
                //タイトルボタンの表示を切り替える
                _systemButtons[(int)DisplayMode.Title].ManageButtonsActive(false);
            }
            _displayMode = changedDisplayMode;
            _backgroundImages[(int)_displayMode].gameObject.SetActive(true);
        }

        /// <summary>ゲーム開始終了を決める入力後の処理を行う</summary>
        bool IsStartInputPosition()
        {
            switch (_buttonIndex)
            {
                case IndexPosition.Left:
                    audioSource.clip = submit;
                    audioSource.Play();
                    ChangeBackGround(DisplayMode.GameEnd);
                    return false;
                case IndexPosition.Right:
                    AudioSource.PlayClipAtPoint(start, Camera.main.transform.position, 0.2f);
                    return true;
                default:
                    break;
            }
            return false;
        }

        /// <summary>ゲーム終了を決める画面での入力後の処理を行う</summary>
        void GameEndOrTitleInput()
        {
            switch (_buttonIndex)
            {
                case IndexPosition.Left:
                    ChangeBackGround(DisplayMode.Title);
                    break;
                case IndexPosition.Right:
                    GameEnd();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// ゲームの終了処理
        /// </summary>
        private void GameEnd()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
        }
    }
}
