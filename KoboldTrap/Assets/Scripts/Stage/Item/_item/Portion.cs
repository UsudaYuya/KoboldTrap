using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KoboldTrap.Stage.Item
{
    public class Portion : ItemBehaviour
    {
        private enum Type { SpeedUp,JumpUp}
        [SerializeField]private Type type = Type.SpeedUp;
        //ポーション
        private void Start()
        {
            this.GetComponent<AudioSource>().PlayOneShot(putAudio);
            if (type == Type.SpeedUp)
                GameObject.FindObjectOfType<StageController>()._actionBehaviours[(int)player].SpeedUp();
            if(type == Type.JumpUp)
                GameObject.FindObjectOfType<StageController>()._actionBehaviours[(int)player].JumpUp();
            Destroy(this.gameObject);
        }
    }
}