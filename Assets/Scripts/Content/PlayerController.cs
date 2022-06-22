using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

// 기획자쪽에서 스탯 조절할 목록을 받으면 Class로 따로 스피드 체력 등등을 나눈다.

public class PlayerController : MonoBehaviour
{
    public enum PlayerState {
        Idle,
        Run,
        Shoot,
        Evasion,
    }

    [SerializeField]
    private PlayerState m_state = PlayerState.Idle;             // 현재 상태
    private PlayerState m_beState = PlayerState.Idle;           // 이전 상태

    private float       m_moveSpeed = 10.0f;
    private Vector3     m_move = Vector3.zero;

    private float       m_evasionSpeed = 500.0f;
    private Vector3     m_evasionTarget = Vector3.zero;

    private int         m_id = -1;                      // 몬스터가 인식할 수 있게 할려고 발급 받는 아이디
    private bool        m_isMain = false;               // 메인이여야 키보드 입력을 받을 수 있기 때문

    private Rigidbody   m_rigid = null;
    private Ability     m_evasionAbility = null;                // 회피 체크
    private Ability     m_boomAbility = null;                   // 폭탄 체크

	#region 프로퍼티
	public Rigidbody Rigid { get { return m_rigid; } }
    public PlayerState State { get { return m_state; } }
    public int ID { get { return m_id; } }
	#endregion

	private void Start()
	{
        m_rigid = GetComponent<Rigidbody>();
        m_evasionAbility = GetComponent<Ability>();
        m_evasionAbility.Init(2.0f, "Evasion");
        m_boomAbility.Init(0.3f, "Boom");

    }

    private void Update()
    {
        InputUpdate();
        UpdateState();
    }

	private void FixedUpdate()
	{

    }

	// 시작하기 전에 초기화를 꼭 해서 아이디를 재 발급하자.
	public void Init(int p_id, bool p_isMain)
	{
        // 아이디로 메인인지를 판별하지는 않을 것
        m_id = p_id;
        m_isMain = p_isMain;
    }


	private void InputUpdate()
    {
        // 메인 플레이어가 아니면 제외
        if(m_isMain == false) {
            return;
		}

        // Up 
        InputMouseRotation();

        if (Input.anyKey == false) {
            return;
        }

        InputMove();
        InputAction();
    }


    private void UpdateState()
	{
        // 상태 검사를 굳이 안해도 될때
        if(m_beState == m_state) {

            // 상태 전환 검사를 해야할때
            switch (m_state) {
                case PlayerState.Evasion:
                    EvasionPressState();
                    break;
                case PlayerState.Run:
                    RunPressState();
                    break;
            }
        }
        else {
            // 상태 전환이 일어났을때 애니메이션을 재생하면서 하는 처음으로 시작해야하는 짓을 시작한다.
            switch(m_state) {
                case PlayerState.Idle:
                    IdleEnterState();
                    break;

                case PlayerState.Run:
                    RunEnterState();
                    break;

                case PlayerState.Evasion:
                    EvasionEnterState();
                    break;

                case PlayerState.Shoot:
                    ShootEnterState();
                    break;
			}


            // 행동이 바뀌었을때 해야하는 것들
            m_beState = m_state;
        }
    }

    private void IdleEnterState()
	{

	}

    private void RunEnterState()
    {
        m_rigid.AddForce(m_move * m_moveSpeed);
    }

    private void ShootEnterState()
    {

    }

    private void EvasionEnterState()
	{
        // 일단 기범이 말대로 근데 완전한 등속 운동은 아님
        // 그리고 RigidBody때문에 완벽하게 목표 지점까지는 못감 (감속 운동 덕분에 방향이 틀어짐)
        m_evasionTarget = m_move * m_evasionSpeed;
        m_rigid.AddForce(m_evasionTarget);
	}

    private void EvasionPressState()
    {
        m_state = PlayerState.Idle;
    }

    private void RunPressState()
    {
        m_rigid.AddRelativeForce(m_move);
    }

    private void InputAction()
	{
        // 키를 누르고
        if(Managers.Input.GetKeyDown(UserKey.Evasion)) {
            // 스페이스바와 방향키가 입력된다면
            if (m_evasionAbility.IsAction() == true && m_move != Vector3.zero) {
                // 움직인다.
                m_state = PlayerState.Evasion;
                m_evasionAbility.Action();
            }
		}
	}

    // 현재 고민인거 서버로 실시간통신을 이용해서 좌표를 얻을 것인지 움직이는 힘을 줄 것인지
    private void InputMove()
    {
        m_move = Vector3.zero;

        if (Managers.Input.GetKey(UserKey.Forward)) {
            m_move.z += 1.0f;
        }
        if (Managers.Input.GetKey(UserKey.Backward)) {
            m_move.z -= 1.0f;
        }
        if (Managers.Input.GetKey(UserKey.Right)) {
            m_move.x += 1.0f;
        }
        if (Managers.Input.GetKey(UserKey.Left)) {
            m_move.x -= 1.0f;
        }

        // 정규화
        if (m_move.magnitude > 1.6f) {
            m_move = m_move.normalized;
        }

        m_rigid.AddForce(m_move * m_moveSpeed);


    }

	// 귀찮으닌깐 재탕을 하자
	private void InputMouseRotation()
	{
		Vector3 playerToScreenPos = Camera.main.WorldToScreenPoint(transform.position);

		Vector3 mousePos = Input.mousePosition;
		Vector3 tempPos = mousePos - playerToScreenPos;
		float lookRadius = Mathf.Atan2(tempPos.y, tempPos.x);

		transform.rotation = Quaternion.Euler(0f
			, (-lookRadius * Mathf.Rad2Deg) + 90f
			, 0f);
	}

	private void OnDrawGizmos()
	{
		switch(m_state) {
            case PlayerState.Evasion:
                EvasionGizmos();
                break;
		}
	}

    private void EvasionGizmos()
	{
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, m_evasionTarget);
	}
}
