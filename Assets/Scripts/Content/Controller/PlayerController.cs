using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

[System.Serializable]
public class PlayerStat 
{
    public int      id = 1;
    public int      hp = 100;
    public int      attack = 70;
    public float    moveSpeed = 50.0f;
    public float    evasionSpeed = 10.0f;
    public int      score = 0;
}

// 기획자쪽에서 스탯 조절할 목록을 받으면 Class로 따로 스피드 체력 등등을 나눈다.
public class PlayerController : BaseController
{
    public enum PlayerState {
        Idle,
        Run,
        Shoot,
        Evasion,
    }


    #region 변수

    [SerializeField]
    private PlayerStat      m_stat = new PlayerStat();
    [SerializeField]
    private PlayerState     m_state = PlayerState.Idle;

    private Vector3         m_move = Vector3.zero;

    private float           m_lookRotation = 0.0f;
    private float           m_explosionDelayTime = 0.5f;
    private float           m_explosionTime = 0.0f;
    private bool            m_isExplosion = false;

    [SerializeField]
    private float           m_evasionDelayTime = 1.0f;      // 대기 시간
    private float           m_evasionTime = 0.0f;           // 현재 시간

    private List<Boom>      m_listBoom = new List<Boom>();

    #endregion

    #region 프로퍼티

    public PlayerStat Stat { get => m_stat; set => m_stat = value; }
    public PlayerState State { get => m_state; set => m_state = value; }

    #endregion


    private void Start()
	{
        base.Init();
    }

    private void Update()
    {
        UpdateState();
        UpdateInput();
    }

	private void FixedUpdate()
	{
        base.OnUpdate();
    }

	#region 상태 업데이트

	public void UpdateState()
	{
        switch(m_state) {
            case PlayerState.Evasion:
                EvasionState();
                break;
		}
	}

    public void EvasionState()
	{
        if(m_rigidValue.Velocity == Vector3.zero) {
            m_state = PlayerState.Idle;
		}
	}

	#endregion


	#region 입력 업데이트

	public void InputMouse()
	{
        InputMouseRotation();
        InputShoot();
    }

    private void InputMouseRotation()
    {
        Vector3 playerToScreenPos = Camera.main.WorldToScreenPoint(transform.position);

        Vector3 mousePos = Input.mousePosition;
        Vector3 tempPos = mousePos - playerToScreenPos;
        m_lookRotation = Mathf.Atan2(tempPos.y, tempPos.x);

        transform.rotation = Quaternion.Euler(0f , (-m_lookRotation * Mathf.Rad2Deg) + 90f , 0f);
    }

    public void InputShoot()
	{
        if(Managers.Input.GetKeyDown(UserKey.Shoot) == true){
            Managers.Resource.Instantiate("Boom", Managers.Game.Player.transform);
		}

        // 클릭했을때
        if (Managers.Input.GetKey(UserKey.Shoot) == true) {
            m_isExplosion = true;
            m_explosionTime += Time.deltaTime;
            
        }

        // 때었을때
        if (Managers.Input.GetKeyUp(UserKey.Shoot) == true) {
            // TODO : 분기해서 지연상태일때는 다른 경우도 체크해준다.
            m_explosionTime = 0.0f;
            m_isExplosion = false;
        }


        // 폭탄이 있을 경우
        if (m_isExplosion == true) {
            // 정해진 시간을 초과할 경우
            if(m_explosionTime <= m_explosionDelayTime) {
                // TODO : 폭탄의 상태를 변환시킨다.

			}
		}
    }

    public void UpdateInput()
	{
        // 회피중일때는 다른 상태는 불가능
        if (m_state == PlayerState.Evasion) {
            return;
        }

        InputMouse();

        m_state = PlayerState.Idle;
        if (Input.anyKey == false) {
            return;
		}

        InputMove();
        InputAddForce();
    }

    public void InputAddForce()
	{
        m_evasionTime += Time.deltaTime;

        if (m_evasionDelayTime >= m_evasionTime) {
            return;
		}

        if(Managers.Input.GetKeyDown(UserKey.Evasion) == true && m_state == PlayerState.Run) {
            AddForce(m_move * m_stat.evasionSpeed);
            m_evasionTime = 0.0f;
            m_state = PlayerState.Evasion;
        }
	}

    public void InputMove()
	{
        m_move = Vector3.zero;

        if(Managers.Input.GetKey(UserKey.Forward) == true) {
            m_move.z += 1.0f;
        }
        if (Managers.Input.GetKey(UserKey.Backward) == true) {
            m_move.z -= 1.0f;
        }
        if (Managers.Input.GetKey(UserKey.Right) == true) {
            m_move.x += 1.0f;
        }
        if (Managers.Input.GetKey(UserKey.Left) == true) {
            m_move.x -= 1.0f;
        }

        if(m_move != Vector3.zero) {
            m_state = PlayerState.Run;
        }

        AddMovement(m_move.normalized * m_stat.moveSpeed);
    }

	#endregion
}
