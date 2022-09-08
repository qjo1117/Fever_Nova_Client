using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EffectManager : MonoBehaviour
{
    //위치, 방향, 회전

    public void Init()
    {
        GameObject root = GameObject.Find("@Effect");
        if (root == null)
        {
            root = new GameObject { name = "@Effect" };
            Object.DontDestroyOnLoad(root);
        }
    }

    public ParticleSystem CreateEffect(
        /*필수 입력*/Define.Effect _effectType, string _fileName, Vector3 _pos, Vector3 _direction,
        /*Define.Effect.OneOff면 필요*/float _playSpeed, float _playTime,
        /*Define.Effect.Projectile면 필요*/float _lifeTime,
        /*특정 객체를 따라다녀야하는 경우 입력*/Transform _parent = null)
    {
        ParticleSystem l_effect = Managers.Resource.Instantiate(_fileName, _parent).GetComponent<ParticleSystem>();
        l_effect.Stop();

        //위치 조정&회전
        l_effect.transform.position = _pos;
        l_effect.transform.Rotate(Quaternion.FromToRotation(l_effect.transform.forward, _direction).eulerAngles);

        switch(_effectType)
        {
            case Define.Effect.OneOff:
                {
                    //재생 속도&시간
                    l_effect.playbackSpeed = _playSpeed;//1이 정상속도

                    var l_particle = l_effect.main;
                    l_particle.duration = _playTime;
                    l_particle.startLifetime = _playTime;

                    //Managers.Resource.Destroy(l_effect.gameObject, _playTime);
                }
                break;
            case Define.Effect.Projectile:
                Managers.Resource.Destroy(l_effect.gameObject, _lifeTime);
                break;
        }
        return l_effect;
    }

    //쓰지마셈
    public bool GetHitTiming(ParticleSystem _effect)
    {
        if ((int)_effect.duration == (int)_effect.time * 2) return true;
        else return false;
    }

    public void Clear()
    {
        
    }
}
