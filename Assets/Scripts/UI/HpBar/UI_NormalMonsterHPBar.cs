using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NormalMonsterHPBar : UI_MonsterHPBar
{
    #region ����
    [SerializeField]
    protected Camera m_mainCamera;

    public int m_verticalOffset;               // ������ġ Y��ǥ offset��
    #endregion

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));
        m_isReady = true;


        GetHpBoost();
        if (m_mainCamera == null)
        {
            m_mainCamera = Camera.main;
        }

        HpBarUpdate();
        CanvasScaleChange();
    }

    private void Update()
    {
        HpBarPositionUpdate();
    }

    // hp���� ���� �������� ������Ʈ �ϴ� �Լ�
    private void HpBarPositionUpdate()
    {
        Collider l_parentCollider = transform.parent.GetComponent<Collider>();

        // hp���� ��ġ�� �ش� hp�ٸ� �������ִ� ����(hp���� �θ�) �� ��ġ���� �������� �ݶ��̴��� ysize�� + y��ǥ offset��ŭ ������������ ����
        transform.position = transform.parent.position + Vector3.up * (l_parentCollider.bounds.size.y + m_verticalOffset);

        // �ش� hp�ٸ� ������ ���� (hp���� �θ�)�� ȸ���ϸ� hp�ٵ� ȸ���ϱ⋚����, hp�ٸ� �׻� ī�޶� ���ϵ�����
        transform.LookAt(m_mainCamera.transform);
        // 180�� ���ư� ���¸� �ذ��ϱ����� -180�� ȸ��������
        transform.Rotate(Vector3.up, -180.0f);
    }

    // canvas�� ũ�⸦ hp�� ũ�⿡ �°� �������ִ� �Լ�
    private void CanvasScaleChange()
    {
        RectTransform l_hpbarRect = Get<Image>((int)Images.hpBarBackground).GetComponent<RectTransform>();
        Vector2 l_newCanvasSize = l_hpbarRect.sizeDelta * l_hpbarRect.localScale.x;

        GetComponent<RectTransform>().sizeDelta = l_newCanvasSize;
    }
}
