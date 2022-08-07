using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GamePlaySelect : UI_Popup
{
    enum GameObjects
    {
        EasterEgg
    }

    enum Images
    {
        Background,
        NoGlassesChan,
        Glasses
    }

    enum Buttons
    {
        BackButton,
        SinglePlayButton,
        MultiPlayButton
    }

    enum Texts
    {
        BackButtonText,
        SinglePlayButtonText,
        MultiPlayButtonText
    }

    private Coroutine m_runningCoroutine;

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        Get<Button>((int)Buttons.BackButton).onClick.AddListener(() => { ClosePopupUI(); });
        Get<Button>((int)Buttons.SinglePlayButton).onClick.AddListener(SinglePlayClick);
        Get<Button>((int)Buttons.MultiPlayButton).onClick.AddListener(MultiPlayClick);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(m_runningCoroutine == null)
            {
                m_runningCoroutine =  StartCoroutine(DepressedChanMove(true));
            }
        }
    }

    // 싱글플레이 버튼 클릭시
    private void SinglePlayClick()
    {
        Managers.UI.ShowPopupUI<UI_StageSelect>();
        Managers.Game.IsMulti = false;
    }

    // 멀티 플레이 버튼 클릭시  (현재 미구현,멀티플레이 아직 추가 X)
    // 일단 서버 접속때문에 같이 구현 - 박 -
    private void MultiPlayClick()
    {
        Managers.UI.ShowPopupUI<UI_StageSelect>();
        Managers.Game.IsMulti = true;
    }

    #region EasterEgg

    IEnumerator DepressedChanMove(bool _isUp)
    {
        Vector3         l_prevPosition;
        float           l_moveY = 2.0f;
        Transform   l_easterEggTrans = Get<GameObject>((int)GameObjects.EasterEgg).GetComponent<Transform>();

        while (true)
        {
            l_prevPosition = l_easterEggTrans.localPosition;
            if (_isUp)
            {
                l_easterEggTrans.localPosition = new Vector3(l_prevPosition.x, l_prevPosition.y + l_moveY, l_prevPosition.z);

                if (l_easterEggTrans.localPosition.y >= 0)
                {
                    m_runningCoroutine = StartCoroutine(GlassesMove(false));
                    yield break;
                }
            }
            else
            {
                l_easterEggTrans.localPosition = new Vector3(l_prevPosition.x, l_prevPosition.y - l_moveY, l_prevPosition.z);

                if (l_easterEggTrans.localPosition.y <= -1130)
                {
                    m_runningCoroutine = null;
                    yield break;
                }
            }

            yield return null;
        }
    }

    IEnumerator ImageRotate()
    {
        int     l_rotateCount = 0;
        float   l_angle = 0.0f;
        float   l_angleIncrease = 2.0f;

        while(true)
        {
            Get<Image>((int)Images.Glasses).GetComponent<RectTransform>().Rotate(Vector3.forward, l_angleIncrease);
            Get<Image>((int)Images.NoGlassesChan).GetComponent<RectTransform>().Rotate(Vector3.forward, -l_angleIncrease);

            l_angle += l_angleIncrease;

            if(l_angle >= 360.0f)
            {
                l_angle = 0.0f;
                l_rotateCount++;

                if(l_rotateCount >=2)
                {
                    m_runningCoroutine = StartCoroutine(GlassesMove(true));
                    yield break;
                }
            }

            yield return null;
        }
    }

    IEnumerator GlassesMove(bool _isInMove)
    {
        RectTransform l_glassesRectTrans = Get<Image>((int)Images.Glasses).GetComponent<RectTransform>();
        Vector3 l_initPosition = l_glassesRectTrans.localPosition;

        Vector2 l_moveVec = new Vector2(1.0f, -1.0f);
        Vector3 l_prevPosition;

        while(true)
        {
            l_prevPosition = l_glassesRectTrans.localPosition;
            if (_isInMove)
            {
                l_glassesRectTrans.localPosition = new Vector3(l_prevPosition.x - l_moveVec.x, l_prevPosition.y - l_moveVec.y, l_prevPosition.z);

                if (l_glassesRectTrans.localPosition.y - l_initPosition.y >= 100)
                {
                    m_runningCoroutine = StartCoroutine(DepressedChanMove(false));
                    yield break;
                }
            }
            else
            {
                l_glassesRectTrans.localPosition = new Vector3(l_prevPosition.x + l_moveVec.x, l_prevPosition.y + l_moveVec.y, l_prevPosition.z);

                if (l_initPosition.y - l_glassesRectTrans.localPosition.y >= 100)
                {
                    m_runningCoroutine = StartCoroutine(ImageRotate());
                    yield break;
                }
            }

            yield return null;
        }   
    }

    #endregion

}
