using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

[System.Serializable]
public class PlayerStat 
{
    public int      id = 1;
    public string   name = "Hello_Player";
    public int      hp = 100;
    public int      attack = 70;
    public float    moveSpeed = 300.0f;
    public float    evasionSpeed = 10.0f;
    public int      score = 0;
}

// 기획자쪽에서 스탯 조절할 목록을 받으면 Class로 따로 스피드 체력 등등을 나눈다.
public class PlayerController : MonoBehaviour
{
    public enum PlayerState {
        Idle,
        Run,
        Shoot,
        Evasion,
        Jump,
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

    private float           m_explosionJumpRadius = 4.0f;

    [SerializeField]
    private float           m_evasionDelayTime = 1.0f;      // 대기 시간
    private float           m_evasionTime = 0.0f;           // 현재 시간

    private Boom            m_boom = null;

    private Rigidbody       m_rigid = null;
    private Animator        m_anim = null;

    private Transform       m_handler = null;

    #endregion

    #region 프로퍼티

    public PlayerStat Stat { get => m_stat; set => m_stat = value; }
    public PlayerState State { get => m_state; set => m_state = value; }

    #endregion


    private void Start()
	{
        m_rigid  = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
        m_handler = Util.FindChild(gameObject, "@Handler", true).transform;
    }

    private void Update()
    {
        UpdateState();
        UpdateInput();
    }

	private void FixedUpdate()
	{

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

		transform.rotation = Quaternion.Euler(0f, (-m_lookRotation * Mathf.Rad2Deg) + 90f, 0f);

        // ----------------------------------------------------------
        // 폭탄 점프에 대한 것으로 애니메이션 전환을 일단은 여기서 하는 것
        // TODO : 괜찮은 위치를 찾으면 함수를 따로 파서 배치할 것
        Ray l_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit l_hit;
        Physics.Raycast(l_ray, out l_hit);
        Vector3 l_position = l_hit.point - transform.position;

        // 폭탄 반경보다 좁으면
        if (l_position.sqrMagnitude < m_explosionJumpRadius * m_explosionJumpRadius) {
            Managers.Log("Animation Change");
		}

        Debug.DrawRay(Camera.main.transform.position, l_ray.direction * 1000.0f, Color.red);
    }

    public void InputShoot()
	{
        if (Managers.Input.GetKeyDown(UserKey.Shoot) == true) {
            // 현재 점프 상태이면 아래로 폭탄을 쏘자
            if(m_state == PlayerState.Jump) {

			}

			// 가장 최신의 폭탄을 가지고 있는다.
			m_boom = Managers.Resource.Instantiate("Boom", Managers.Game.Boom.transform).GetComponent<Boom>();
            m_boom.Shoot(m_handler.position, transform.forward);
		}
        

        // 현재 Press중일때 카운트를 세어서 현재 상태값을 전환시킨다.
        if (Managers.Input.GetKey(UserKey.Shoot) == true) {
            m_isExplosion = true;
            m_explosionTime += Time.deltaTime;
        }

        // Up을 했을때 현재 상태를 전부 초기화해준다.
        if (Managers.Input.GetKeyUp(UserKey.Shoot) == true) {
            // TODO : 분기해서 지연상태일때는 다른 경우도 체크해준다.
            if(m_boom.State == Boom.BoomState.Delay) {
                m_boom.Explosion();
			}

            m_explosionTime = 0.0f;
            m_isExplosion = false;
        }


        // 폭탄이 있을 경우
        if (m_isExplosion == true) {
            // 정해진 시간을 초과할 경우
            if(m_explosionTime >= m_explosionDelayTime) {
                // 폭탄의 상태를 변환시킨다.
                m_boom.State = Boom.BoomState.Delay;
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

		if (Managers.Input.GetKeyDown(UserKey.Evasion) == true && m_state == PlayerState.Run) {
            m_rigid.AddForce(m_move * m_stat.evasionSpeed);
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
            m_rigid.AddForce(m_stat.moveSpeed * m_move.normalized);
        }
	}

	#endregion

	private void OnDrawGizmosSelected()
	{
        Vector3 l_position = transform.position;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(l_position, m_explosionJumpRadius);
	}
}
