using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KoboldTrap.Stage
{
    public class Pause : MonoBehaviour
    {
        [SerializeField]
        private GameObject pauseUI = null;

        private GameObject pause = null;

        new AudioSource audio;
        [SerializeField] AudioClip _submit = null;

        private void Start()
        {
            audio = Camera.main.GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Pause"))
            {
                if (pause)
                {
                    Destroy(pause);
                    Time.timeScale = 1;
                    audio.PlayOneShot(_submit);
                }
                else
                {
                    Time.timeScale = 0;
                    pause = Instantiate(pauseUI);
                    audio.PlayOneShot(_submit);
                }
            }
        }
    }
}
