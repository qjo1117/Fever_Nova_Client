using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SceneHPBar : UI_Scene
{
    #region UI������Ʈ_ENUM
    protected enum Images
    {
        hpBarBackground,                                         // hp�� �޹�� (hp�� �Ͼ�� ���)
        hpBar                                                    // hp��       (��� hp��)
    }

    protected enum GameObjects
    {
        hpLine                                                  // �Ͼ�� �簢��������(Image,Image(1)....) ���� ������Ʈ 
    }
    #endregion

    #region ����
    protected int m_hp = 0;                          // hp�� ��¿� ���� hp��
    protected int m_maxHp;
    protected bool m_isReady;

    public int m_unitHp;                          // ���� hp 
                                                  // (���� hp�� �������� �ִ� ü���� ���������� �Ͼ�� �簢���� �����ϰ� �迭��)
    #endregion

    #region ������Ƽ

    // ������Ƽ�� ���� hp���� �����ϸ� �ڵ����� hpBar�� ���¸� �����ϵ�����
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
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));
        m_isReady = true;


        GetHpBoost();
        HpBarUpdate();
    }


    public void GetHpBoost()
    {
        if (!m_isReady)
            return;

        // (���� hp / �ִ�ü��) ����Ͽ� �Ͼ�� �簢 �������� �����ϰ� ����
        float l_scaleX = (float)m_unitHp / m_maxHp;

        // hpLine ������Ʈ�� �ڽĵ� (�Ͼ�� �簢 �����ӵ�)�� �����ϰ��� ����
        // (HorizontalLayoutGroup ������Ʈ�� Ȱ��ȭ �Ǿ������� Scale�� ����� ������ �ȵǹǷ� �������� ��� ��Ȱ��ȭ ������)

        GameObject l_hpLine = Get<GameObject>((int)GameObjects.hpLine);
        l_hpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach (Transform child in l_hpLine.transform)
        {
            child.gameObject.transform.localScale = new Vector3(l_scaleX, 1, 1);
        }
        l_hpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
    }


    // hp ���� ���� �����ϴ� �Լ�
    protected void HpBarUpdate()
    {
        if (!m_isReady)
            return;

        Get<Image>((int)Images.hpBar).fillAmount = m_hp / (float)m_maxHp;
    }
}
