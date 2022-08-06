using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//------------------------------------------------------------
//�ݰ�, ������ ��Ÿ���� UI�ǰ�� ǥ���� �̹����� �����ҋ�
//Pixel per unit���� �����ؼ� Unity�� 1Unit�� �ش��ϴ� ũ���
//�����ϰ�, SetUIScale�Լ��� 'm_rangeRadius / 0.5f '�� 0.5f����
//������ �̹����� ���������� �������־����.
//
//������ ���ؼ� ���� ����ȭ �����־����
//------------------------------------------------------------


public class UI_BombRange : UI_Scene
{
    #region ����
    [SerializeField]
    protected float       m_rangeRadius = 0.5f;                 // �ݰ� ������
    #endregion

    #region ������Ƽ
    public float RangeRadius
    {
        get
        {
            return m_rangeRadius;
        }
        set
        {
            m_rangeRadius = value;
            SetUIScale();
        }
    }
    #endregion

    public override void Init()
    {

    }

    private void FixedUpdate()
    {
        PositionUpdate();
    }

    // �������� ���� UI�� ũ�� �����ϴ� �Լ�
    public void SetUIScale()
    {
        // ���� �̹����� Pixel per unit �����ؼ� scale 1,1,1 ���� ������ 0.5f
        transform.localScale = Vector3.one * (m_rangeRadius / 0.5f);
    }

    // UI�� ��ġ�� �÷��̾��� ��ġ�� �������ִ� �Լ�
    private void PositionUpdate()
    {
        PlayerController l_player = Managers.Game.Player.MainPlayer;
        if (l_player != null) {
            // �ٴڰ� ���ļ� UI�� �Ⱥ��̹Ƿ� ������ ��ġ Vector�� y���� 0.1f�� ����
            transform.position = new Vector3(l_player.transform.position.x, l_player.transform.position.y + 0.2f, l_player.transform.position.z);
        }
    }


    private void OnDrawGizmos()
    {
        // ������ ���� 
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + m_rangeRadius, transform.position.y, transform.position.z));
    }
}
