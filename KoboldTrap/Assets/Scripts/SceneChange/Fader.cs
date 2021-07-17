using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KoboldTrap
{
    public class Fader : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InstantateFader()
        {
            fader = Instantiate(Resources.Load<GameObject>("UI/FadeCanvas"));
            faderBlack = Instantiate(Resources.Load<GameObject>("UI/FadeCanvasBlack"));

            fader.GetComponent<FadeCanvas>().faderImg.raycastTarget = false;
            faderBlack.GetComponent<FadeCanvas>().faderImg.raycastTarget = false;

            DontDestroyOnLoad(fader);
            DontDestroyOnLoad(faderBlack);
        }

        private static GameObject fader;
        private static GameObject faderBlack;

        static public void FadeOut(float time)
        {
            fader.GetComponent<FadeCanvas>().FadeOut(time);
        }

        static public void FadeOutBlack(float time)
        {
            faderBlack.GetComponent<FadeCanvas>().FadeOut(time);
        }

        static public void FadeIn(float time, string sceneName)
        {
            fader.GetComponent<FadeCanvas>().FadeIn(time, sceneName);
        }

        static public void FadeIn(float time)
        {
            fader.GetComponent<FadeCanvas>().FadeIn(time);
        }

        static public void FadeInBlack(float time, string sceneName)
        {
            faderBlack.GetComponent<FadeCanvas>().FadeIn(time, sceneName);
        }

        static public void FadeInBlack(float time)
        {
            faderBlack.GetComponent<FadeCanvas>().FadeIn(time);
        }

        static public void FadeInAndOut(float inTime, float waitTime, float outTime)
        {
            Canvas faderCanvas = fader.GetComponent<Canvas>();
            faderCanvas.sortingOrder = 100;
            fader.GetComponent<FadeCanvas>().FadeInAndOut(inTime, waitTime, outTime, faderCanvas);
        }

        static public void FadeInAndOutBlack(float inTime, float waitTime, float outTime)
        {
            Canvas faderBlackCanvas = faderBlack.GetComponent<Canvas>();
            faderBlackCanvas.sortingOrder = 100;
            faderBlack.GetComponent<FadeCanvas>().FadeInAndOut(inTime, waitTime, outTime, faderBlackCanvas);
        }
    }}
