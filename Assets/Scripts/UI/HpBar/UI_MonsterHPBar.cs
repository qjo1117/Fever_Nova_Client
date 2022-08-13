using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MonsterHPBar : UI_Scene
{
    #region UI컴포넌트_ENUM
    protected enum Images
    {
        hpBarBackground,                                         // hp바 뒷배경 (hp바 하얀색 배경)
        hpBar                                                    // hp바       (녹색 hp바)
    }

    protected enum GameObjects
    {
        hpLine                                                  // 하얀색 사각형프레임(Image,Image(1)....) 관리 오브젝트 
    }
    #endregion

    #region 변수
    protected int m_hp = 0;                          // hp바 출력에 사용될 hp값
    protected int m_maxHp;
    protected bool m_isReady;

    public float m_unitHp;                          // 단위 hp 
                                                  // (단위 hp가 낮을수록 최대 체력이 증가했을떄 하얀색 사각형이 촘촘하개 배열됨)
    #endregion

    #region 프로퍼티

    // 프로퍼티를 통해 hp값을 변경하면 자동으로 hpBar의 상태를 갱신하도록함
    public int HP
    {
        get
        {
            return m_hp;
        }
        set
        {
            m_hp = value;
            HpBarUpdate();
        }
    }
    public int MaxHP
    {
        get
        {
            return m_maxHp;
        }
        set
        {
            m_maxHp = value;
            GetHpBoost();
            HpBarUpdate();
        }
    }
    #endregion

    public override void Init()
    {
    }


    public void GetHpBoost()
    {
        if (!m_isReady)
            return;

        // (단위 hp / 최대체력) 계산하여 하얀색 사각 프레임의 스케일값 구함
        float l_scaleX = m_unitHp / m_maxHp;

        // hpLine 오브젝트의 자식들 (하얀색 사각 프레임들)의 스케일값을 변경
        // (HorizontalLayoutGroup 컴포넌트가 활성화 되어있으면 Scale이 제대로 적용이 안되므로 적용전에 잠시 비활성화 시켜줌)

        GameObject l_hpLine = Get<GameObject>((int)GameObjects.hpLine);
        l_hpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach (Transform child in l_hpLine.transform)
        {
            child.gameObject.transform.localScale = new Vector3(l_scaleX, 1, 1);
        }
        l_hpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
    }


    // hp 바의 상태 갱신하는 함수
    protected void HpBarUpdate()
    {
        if (!m_isReady)
            return;

        Get<Image>((int)Images.hpBar).fillAmount = m_hp / (float)m_maxHp;
    }
}
