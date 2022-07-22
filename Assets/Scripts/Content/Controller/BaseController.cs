using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* -----------------------------------
 *      생산성 떨어지는 것 같아서 폐기
 ----------------------------------- */


/* -----------------------------------------------------------------------------
 *                  주요 목표
 *          힘과 움직임을 이동 방식을 나눈다.
 *  이유 : 일반적인 Force개념으로 움직일 경우
 *       마찰력에 의한 회전이 들어가서 원하는 방향과 힘으로 적용되기 힘들어 보임.
 *  목표 : 일을 분배하기 위한 기초적인 움직임과 힘을 분리한 Movement구현
 *  변경사항 : 최적화가 똥망이므로 지속적인 피드백을 통한 개선이 필요
----------------------------------------------------------------------------- */

/*--------------------------------------------------------------------------------------------------------------------------------------------
 *                  설명
 *      AddMovement : 움직임을 제공합니다. (짧은 거리를 움직일때 사용)   기본 움직임 (문제점 폭탄이 밀렸을때 폭탄의 힘과 상쇄가 안된다.)
 *      AddForce : 힘을 제공합니다. (긴 거리를 움직일 때 사용)           폭탄       (MoveToVector를 사용한 결과 너무 고정적인 위치로만 움직인다.)
-------------------------------------------------------------------------------------------------------------------------------------------- */


public abstract class BaseController : MonoBehaviour 
{
    [System.Serializable]
    public class RigidState 
    {
        public bool             IsGround = false;
        public bool             IsForwardBlocked = false;
        public bool             IsForceBlocked = false;
        public bool             IsVelocityBlocked = false;
	}

    [System.Serializable]
    public class RigidValue 
    {
        public Vector3          Move = Vector3.zero;          // 움직임
        public Vector3          MoveVelocity = Vector3.zero;  // 움직임
        public Vector3          Velocity = Vector3.zero;
        public Vector3          Force = Vector3.zero;
        public Vector3          PosVelocity = Vector3.zero;
    }

	private float				m_gravity = 1.0f;
    private float               m_gravityForce = 0.98f;
    [SerializeField]
    private float               m_groundCheckDistance = 0.5f;
    private Vector3             m_groundPoint = Vector3.zero;

    private float               m_moveCheckDistance = 0.2f;

    private Vector3             m_velocityPoint = Vector3.zero;
    private float               m_velocityDistance = 0.0f;

    private int                 m_layer = 1 << (int)Define.Layer.Ground | 1 << (int)Define.Layer.Player | 1 << (int)Define.Layer.Monster;

    [SerializeField]
    private float               m_mass = 1.0f;

	private CapsuleCollider		m_capsule = null;
    private float               m_capsuleRadius = 0.0f;
    private float               m_capsuleRadiusDiff = 0.0f;

    [SerializeField]
    protected RigidState            m_rigidState = new RigidState();
    protected RigidValue            m_rigidValue = new RigidValue();

    public RigidState State { get => m_rigidState; set => m_rigidState = value; }
    public RigidValue Value { get => m_rigidValue; set => m_rigidValue = value; }

    private Vector3 m_capsuleTopCenterPoint = Vector3.zero;
    private Vector3 m_capsuleBottomCenterPoint = Vector3.zero;

    private Vector3 CapsuleTopCenterPoint { get => m_capsuleTopCenterPoint; }
    private Vector3 CapsuleBottomCenterPoint { get => m_capsuleBottomCenterPoint; }

    public void Init()
	{

        InitCapsuleCollider();

    }

    public void AddMovement(Vector3 p_move)
	{
        m_rigidValue.Move = p_move;
	}

    public void AddForce(Vector3 p_force)
	{
        m_rigidValue.Force += p_force;
    }

    // 상속된 녀석들이 업데이트를 호출하도록 한다.
	public void OnUpdate()
	{
        m_capsuleTopCenterPoint = new Vector3(transform.position.x, transform.position.y + m_capsule.height - m_capsule.radius, transform.position.z);
        m_capsuleBottomCenterPoint = new Vector3(transform.position.x, transform.position.y + m_capsule.radius, transform.position.z);

        CheckForward();             
        CheckForce();               
        CheckVelocity();            

        UpdatePhysics();            
        UpdateValue();              

        ApplyMovementRigidbody();   
    }

    // 캡슐콜라이더의 크기를 가져온다.
	private void InitCapsuleCollider()
	{
        TryGetComponent(out m_capsule);
        if (m_capsule == null) {
            m_capsule = gameObject.AddComponent<CapsuleCollider>();

            // 렌더러를 모두 탐색하여 높이 결정
            float maxHeight = -1f;

            // 1. SMR 확인
            SkinnedMeshRenderer[] smrArr = GetComponentsInChildren<SkinnedMeshRenderer>();
            if (smrArr.Length > 0) {
                foreach (var smr in smrArr) {
                    foreach (var vertex in smr.sharedMesh.vertices) {
                        if (maxHeight < vertex.y)
                            maxHeight = vertex.y;
                    }
                }
            }
            // 2. MR 확인
            else {
                MeshFilter[] mfArr = GetComponentsInChildren<MeshFilter>();
                if (mfArr.Length > 0) {
                    foreach (var mf in mfArr) {
                        foreach (var vertex in mf.mesh.vertices) {
                            if (maxHeight < vertex.y)
                                maxHeight = vertex.y;
                        }
                    }
                }
            }

            // 3. 캡슐 콜라이더 값 설정
            if (maxHeight <= 0)
                maxHeight = 1f;

            float center = maxHeight * 0.5f;

            m_capsule.height = maxHeight;
            m_capsule.center = Vector3.up * center;
            m_capsule.radius = 0.2f;
        }

        m_capsuleRadius = m_capsule.radius * 0.9f;
        m_capsuleRadiusDiff = m_capsule.radius - m_capsuleRadius + 0.05f;
    }

    // 바닥에 잘 있는지 확인한다.
    private void CheckGround()
	{
        RaycastHit hit;

        m_rigidState.IsGround = false;

        if(Physics.SphereCast(CapsuleBottomCenterPoint, m_capsuleRadius, Vector3.down, 
            out hit, m_gravity * Time.fixedDeltaTime, m_layer, QueryTriggerInteraction.Ignore)) {
            m_rigidState.IsGround = true;
            m_groundPoint = hit.point;
        }
    }

    // 앞으로 갈 수 있는지 체크한다.
    private void CheckForward()
	{
        m_rigidState.IsForwardBlocked = false;

        if(m_rigidValue.Move == Vector3.zero) {
            return;
		}

        RaycastHit hit;

        if (Physics.CapsuleCast(CapsuleBottomCenterPoint, CapsuleTopCenterPoint, m_capsuleRadius, 
            m_rigidValue.Move + Vector3.down * 0.01f, out hit, m_moveCheckDistance, m_layer, QueryTriggerInteraction.Ignore)) {
            m_rigidState.IsForwardBlocked = true;
        }
	}

    // 힘으로 인한 움직임이 가능한지 체크한다.
    private void CheckForce()
	{
        // 자신 기준으로 폭발이 가했을때 걸리는 녀석을 찾는다.
        m_rigidState.IsForceBlocked = false;

        // 힘이 안가해졌을 경우에는 체크를 안한다.
        if (m_rigidValue.Force == Vector3.zero) {
            return;
		}

        RaycastHit hit;

        if (Physics.CapsuleCast(CapsuleBottomCenterPoint, CapsuleTopCenterPoint, m_capsuleRadius,
            m_rigidValue.Force + Vector3.down * 0.01f, out hit, m_rigidValue.Force.magnitude, m_layer, QueryTriggerInteraction.Ignore))  {
            m_rigidState.IsForceBlocked = true;
            m_velocityDistance = hit.distance;
            m_velocityPoint = hit.point;
            m_velocityPoint.y = transform.position.y + m_capsule.radius;
        }
    }

    private void CheckVelocity()
	{
        m_rigidState.IsVelocityBlocked = false;

        if(m_rigidValue.Velocity == Vector3.zero) {
            return;
		}

        RaycastHit hit;

        if (Physics.CapsuleCast(CapsuleBottomCenterPoint, CapsuleTopCenterPoint, m_capsuleRadius,
            m_rigidValue.Velocity + Vector3.down * 0.01f, out hit, m_rigidValue.Velocity.magnitude, m_layer, QueryTriggerInteraction.Ignore)) {
            m_rigidState.IsVelocityBlocked = true;
        }
	}

    // 중력을 가한다.
    private void UpdatePhysics()
	{
        if (m_rigidValue.Velocity != Vector3.zero) {
            return;
		}

        m_gravity += m_mass * m_gravityForce;

        CheckGround();

        if (m_rigidState.IsGround == true) {
            if(m_gravity != 1.0f && m_groundPoint.y >= transform.position.y - m_gravity) {
                m_gravity = 1.0f;
                transform.position = new Vector3(transform.position.x, m_groundPoint.y, transform.position.z);
            }
        }
        transform.position -= Vector3.up * m_gravity * Time.fixedDeltaTime;
    }

    // 움직임, 힘에대한 처리르 해준다.
    private void UpdateValue()
	{
        // 움직임에 대한 처리
        if(m_rigidState.IsForwardBlocked == false) {
            m_rigidValue.MoveVelocity = m_rigidValue.Move;
            m_rigidValue.Move = Vector3.zero;
        }


        // 힘에 대한 처리 (선으로 검사하는 이유 : 힘을 가했을때 체크하는 문구이기때문)
        if (m_rigidValue.Force != Vector3.zero) {
            // 벽에 걸리는 녀석이 없을 경우
            if (m_rigidState.IsForceBlocked == false) {
                // 힘을 그대로 진행하면 된다.
                m_rigidValue.Velocity = m_rigidValue.Force / m_mass;
                m_rigidValue.Force = Vector3.zero;
            }
            // 벽에 걸리는 녀석이 있는 경우
            else {
                if (m_velocityDistance >= m_capsule.radius) {
                    // 최대 지점을 제한한다.
                    Vector3 subVec = m_velocityPoint - transform.position;
                    subVec.y = 0.0f;
                    Vector3 force = subVec + (-m_rigidValue.Force.normalized * m_capsule.radius);
                    m_rigidValue.Velocity = force / m_mass;
                }
                m_rigidValue.Force = Vector3.zero;
            }

            m_rigidValue.PosVelocity = m_rigidValue.Velocity + transform.position;
        }



        if(m_rigidState.IsVelocityBlocked == true) {

		}


        // 폭발과 움직임에 대한 힘의 상쇄를 해준다.
        // 문제는 힘과 힘을 다시 더해준 것이므로 다시 검사해야한다. : TODO : 나중에 할래...
        //if(m_rigidValue.Velocity != Vector3.zero && m_rigidValue.MoveVelocity != Vector3.zero) {
        //    m_rigidValue.Velocity += m_rigidValue.MoveVelocity * Time.fixedDeltaTime;
        //    m_rigidValue.MoveVelocity = Vector3.zero;
        //}
	}

    // 모든 힘에 대한 연산을 해준다.
    public void ApplyMovementRigidbody()
	{
        // 움직임에 대한 연산
        if(m_rigidValue.MoveVelocity != Vector3.zero) {
            transform.position += m_rigidValue.MoveVelocity * Time.fixedDeltaTime;
            m_rigidValue.MoveVelocity = Vector3.zero;
        }
        
        if(m_rigidValue.Velocity != Vector3.zero) {
            transform.position = Vector3.MoveTowards(transform.position, m_rigidValue.PosVelocity, 0.5f);

            Vector3 dist = m_rigidValue.PosVelocity - transform.position;
            if (dist.sqrMagnitude <= Mathf.Epsilon) {
                m_rigidValue.Velocity = Vector3.zero;
			}
        }
    }

	private void OnDrawGizmosSelected()
	{
        // 움직임체크 : 주석한 이유 / 너무 연산 많이 잡아먹음
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, m_rigidValue.PosVelocity);
        Gizmos.DrawWireSphere(m_rigidValue.PosVelocity, 0.5f);
    }


}
