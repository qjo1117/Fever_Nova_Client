using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EffectManager : MonoBehaviour
{
    //��ġ, ����, ȸ��

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
        /*�ʼ� �Է�*/Define.Effect _effectType, string _fileName, Vector3 _pos, Vector3 _direction,
        /*Define.Effect.OneOff�� �ʿ�*/float _playSpeed, float _playTime,
        /*Define.Effect.Projectile�� �ʿ�*/float _lifeTime,
        /*Ư�� ��ü�� ����ٳ���ϴ� ��� �Է�*/Transform _parent = null)
    {
        ParticleSystem l_effect = Managers.Resource.Instantiate(_fileName, _parent).GetComponent<ParticleSystem>();
        l_effect.Stop();

        //��ġ ����&ȸ��
        l_effect.transform.position = _pos;
        l_effect.transform.Rotate(Quaternion.FromToRotation(l_effect.transform.forward, _direction).eulerAngles);

        switch(_effectType)
        {
            case Define.Effect.OneOff:
                {
                    //��� �ӵ�&�ð�
                    l_effect.playbackSpeed = _playSpeed;//1�� ����ӵ�

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

    //��������
    public bool GetHitTiming(ParticleSystem _effect)
    {
        if ((int)_effect.duration == (int)_effect.time * 2) return true;
        else return false;
    }

    public void Clear()
    {
        
    }
}
