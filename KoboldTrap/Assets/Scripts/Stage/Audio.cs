using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio Instance;
    private void Awake() { Instance = this; }

    AudioSource _audio;
    private void Start() { _audio = this.GetComponent<AudioSource>(); }
    //接地
    [Header("設置"), SerializeField] private AudioClip _ground = default;
    public void Ground() { _audio.PlayOneShot(_ground); }
    //攻撃
    [Header("攻撃"), SerializeField] private AudioClip _attack = default;
    public void Attack() { _audio.PlayOneShot(_attack); }
    //ゴール
    [Header("ゴール"), SerializeField] private AudioClip _goal = default;
    public void Goal() { _audio.PlayOneShot(_goal); }
    //ダメージ
    [Header("ダメージ"), SerializeField] private AudioClip _damage = default;
    public void Damage() { _audio.PlayOneShot(_damage); }  
    
    [Header("選択"), SerializeField] private AudioClip _select = default;
    public void Select () { _audio.PlayOneShot(_select); } 
    
    [Header("決定"), SerializeField] private AudioClip _submit = default;
    public void Submit() { _audio.PlayOneShot(_submit); }
}
