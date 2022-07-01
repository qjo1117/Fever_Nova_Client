using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour 
{
    [System.Serializable]
    public class RigidState 
    {
        public bool             IsGround = false;
        public bool             IsForwardBlocked = false;
        public bool             IsVelocityBlocked = false;
	}

    [System.Serializable]
    public class RigidValue 
    {
        public Vector3          Explosion = Vector3.zero;
        public Vector3          Move = Vector3.zero;          // 움직임
        public Vector3          MoveVelocity = Vector3.zero;  // 움직임
        public Vector3          Velocity = Vector3.zero;
        public Vector3          Force = Vector3.zero;
        public Vector3          Friction = Vector3.zero;
    }

	private float				m_gravity = 0.0f;
    [SerializeField]
    private float               m_groundCheckDistance = 0.5f;
    private float               m_groundToDistance = 0.0f;
    private Vector3             m_groundPoint = Vector3.zero;
    private float               m_moveCheckDistance = 0.2f;
    private Vector3             m_velocityPoint = Vector3.zero;
    private int                 m_layer = 1 << (int)Define.Layer.Ground;
    private float               m_gravityForce = 9.8f;
    private float               m_frictionRatio = 3.0f;

    private float               m_mass = 1.0f;

	private CapsuleCollider		m_capsule = null;
    private float               m_capsuleRadius = 0.0f;
    private float               m_capsuleRadiusDiff = 0.0f;

    [SerializeField]
    private RigidState          m_state = new RigidState();
    private RigidValue          m_value = new RigidValue();

    public RigidState State { get => m_state; set => m_state = value; }
    public RigidValue Value { get => m_value; set => m_value = value; }

    private Vector3 CapsuleTopCenterPoint => new Vector3(transform.position.x, transform.position.y + m_capsule.height - m_capsule.radius, transform.position.z);
    private Vector3 CapsuleBottomCenterPoint => new Vector3(transform.position.x, transform.position.y + m_capsule.radius, transform.position.z);

    public void Init()
	{

        InitCapsuleCollider();

    }

    public void AddMovement(Vector3 p_move)
	{
        m_value.Move = p_move;
	}

    public void AddForce(Vector3 p_force)
	{
        m_value.Force += p_force;
    }

	public void OnUpdate()
	{
        CheckGround();
        CheckForward();
        CheckVelocity();

        UpdatePhysics();
        UpdateValue();

        ApplyMovementRigidbody();
    }

	private void InitCapsuleCollider()
	{
        TryGetComponent(out m_capsule);
        if (m_capsule == null) {
            m_capsule = gameObject.AddComponent<CapsuleCollider>();

            // 렌더러를 모두 탐색하여 높이 결정
            float maxHeight = -1f;

            // 1. SMR 확인
            var smrArr = GetComponentsInChildren<SkinnedMeshRenderer>();
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
                var mfArr = GetComponentsInChildren<MeshFilter>();
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


    private void CheckGround()
	{
        RaycastHit hit;

        m_state.IsGround = false;

        if(Physics.SphereCast(CapsuleBottomCenterPoint, m_capsuleRadius, Vector3.down, 
            out hit, m_groundCheckDistance, -1, QueryTriggerInteraction.Ignore)) {
            m_state.IsGround = true;
            m_groundToDistance = hit.distance;
            m_groundPoint = hit.point;
        }
    }
    private void CheckForward()
	{
        RaycastHit hit;

        m_state.IsForwardBlocked = false;
        if (Physics.CapsuleCast(CapsuleBottomCenterPoint, CapsuleTopCenterPoint, m_capsuleRadius, 
            m_value.Move + Vector3.down * 0.1f, out hit, m_moveCheckDistance, -1, QueryTriggerInteraction.Ignore)) {
            m_state.IsForwardBlocked = true;
            m_groundPoint = hit.point;
        }
	}
    private void CheckVelocity()
	{
        RaycastHit hit;

        m_state.IsVelocityBlocked = false;
        if (Physics.CapsuleCast(CapsuleBottomCenterPoint, CapsuleTopCenterPoint, m_capsuleRadius,
            m_value.Velocity + Vector3.down * 0.1f, out hit, m_moveCheckDistance, -1, QueryTriggerInteraction.Ignore))  {
            m_state.IsVelocityBlocked = true;
            m_velocityPoint = hit.point;
        }
    }
    private void UpdatePhysics()
	{
        if(m_state.IsGround == true) {
            m_gravity = 0.0f;
        }
        else {
            m_gravity += m_gravityForce * 0.05f;
        }
	}

    private void UpdateValue()
	{
        // 움직임에 대한 힘을 처리
        if(m_state.IsForwardBlocked == false && m_state.IsGround == true) {
            m_value.MoveVelocity = m_value.Move;
        }

        // 폭발에 대한 힘을 처리
        if(m_state.IsVelocityBlocked == true) {
            m_value.Velocity += m_value.Explosion;
		}
        else {
            m_value.Velocity = Vector3.zero;
		}


	}

    public void ApplyMovementRigidbody()
	{
        if(m_value.MoveVelocity != Vector3.zero) {
            transform.position += m_value.MoveVelocity * Time.fixedDeltaTime;
            m_value.MoveVelocity = Vector3.zero;
            m_value.Move = Vector3.zero;
        }

        if (m_state.IsGround == false) {
            transform.position -= Vector3.up * m_gravity * Time.fixedDeltaTime;
        }

        if (m_value.Force != Vector3.zero) {
            // 힘을 가해준다.
            Vector3 accel = m_value.Force * m_mass;

            m_value.Velocity += accel * Time.fixedDeltaTime;
            m_value.Force = Vector3.zero;
        }

        if (m_value.Velocity != Vector3.zero) {
            Vector3 dir = m_value.Velocity.normalized;

            m_value.Friction = dir / m_frictionRatio;
            float frictionForce = m_value.Friction.sqrMagnitude;

            if(frictionForce > m_value.Velocity.sqrMagnitude) {
                m_value.Velocity = Vector3.zero;
            }
            else {
                m_value.Velocity -= m_value.Friction;
            }

            transform.position += m_value.Velocity;
        }
    }

	private void OnDrawGizmosSelected()
	{
    }


}
