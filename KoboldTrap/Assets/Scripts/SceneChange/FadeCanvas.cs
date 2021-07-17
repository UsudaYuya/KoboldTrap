using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace KoboldTrap
{
    public class FadeCanvas : MonoBehaviour
    {
        public Image faderImg;
        private float alpha;
        private int fadeSwitch;
        private float fadeDelta;

        [SerializeField] private GameObject load = null;
        [SerializeField] private Slider loadSlider = null;
        [SerializeField] private Image icon = null;
        private AsyncOperation async;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            faderImg.color = new Color(faderImg.color.r, faderImg.color.g, faderImg.color.b, alpha);
            switch (fadeSwitch)
            {
                default:
                    break;
                case 1:
                    if (alpha > 0)
                    {
                        alpha -= fadeDelta;
                    }
                    else
                    {
                        alpha = 0;
                        fadeSwitch = 0;
                    }
                    break;
                case 2:
                    if (alpha < 1)
                    {
                        alpha += fadeDelta;
                    }
                    else
                    {
                        alpha = 1;
                        fadeSwitch = 0;
                    }
                    break;
            }
        }

        public void FadeOut(float time)
        {
            StartCoroutine(StartFadeOut(time));
        }

        public void FadeIn(float time, string sceneName)
        {
            StartCoroutine(StartFadeIn(time, sceneName));
        }

        public void FadeIn(float time)
        {
            StartCoroutine(SimpleFadeIn(time));
        }

        public void FadeInAndOut(float fadeIn, float wait, float fadeOut, Canvas canvas)
        {
            StartCoroutine(InAndOutProcess(fadeIn, wait, fadeOut, canvas));
        }

        private IEnumerator StartFadeOut(float time)
        {
            alpha = 1;
            fadeDelta = Time.deltaTime / time;
            fadeSwitch = 1;
            yield return null;
            while (fadeSwitch != 0)
            {
                yield return null;
            }
            Destroy(gameObject);
            yield break;
        }

        private IEnumerator StartFadeIn(float time, string sceneName)
        {
            faderImg.raycastTarget = true;
            alpha = 0;
            fadeDelta = Time.deltaTime / time;

            fadeSwitch = 2;
            yield return null;
            while (fadeSwitch != 0)
            {
                yield return null;
            }

            load.SetActive(true);
            async = SceneManager.LoadSceneAsync(sceneName);
            while (!async.isDone)
            {
                loadSlider.value = async.progress;
                if (icon != null) icon.rectTransform.Rotate(0, 0, 60f * Time.deltaTime);
                yield return null;
            }
            faderImg.raycastTarget = false;
        }

        private IEnumerator SimpleFadeIn(float time)
        {
            faderImg.raycastTarget = true;
            alpha = 0;
            fadeDelta = Time.deltaTime / time;

            fadeSwitch = 2;
            yield return null;
            while (fadeSwitch != 0)
            {
                yield return null;
            }
            Destroy(gameObject);
            yield break;
        }

        private IEnumerator InAndOutProcess(float fadeIn, float wait, float fadeOut, Canvas canvas)
        {
            faderImg.raycastTarget = true;
            alpha = 0;
            fadeDelta = Time.deltaTime / fadeIn;

            fadeSwitch = 2;
            yield return null;
            while (fadeSwitch != 0)
            {
                yield return null;
            }
            alpha = 1;

            yield return new WaitForSeconds(wait);

            fadeDelta = Time.deltaTime / fadeOut;
            fadeSwitch = 1;
            yield return null;
            while (fadeSwitch != 0)
            {
                yield return null;
            }
            canvas.sortingOrder = -100;
            faderImg.raycastTarget = false;
            yield break;
        }
    }
}
