using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KoboldTrap.Stage.Item
{
    public class RotationBlade : ItemBehaviour
    {
        //回転刃　接触で判定　回転する

        private void Start()
        {
            this.GetComponent<AudioSource>().PlayOneShot(putAudio);
        }

        private void FixedUpdate()
        {
            if (player != StageController.player)
            {
                //回転させる
                transform.Rotate(new Vector3(0, 0, -3));
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (player != StageController.player)
            {
                if (collision.gameObject.GetComponent<Player.Player>())
                {
                    Audio.Instance.Damage();
                    collision.GetComponent<Player.PlayerBehaviour>().Damage(1);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
