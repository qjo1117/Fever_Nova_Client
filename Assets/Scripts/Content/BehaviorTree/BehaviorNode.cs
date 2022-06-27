
// 상태
public enum BehaviorStatus
{
	Invaild,
	Running,
	Success,
	Failure,
}


// 가장 기본으로 구성된 클래스

public abstract class BehaviorNode
{
	// 부모 노드를 가리킵니다.
	protected BehaviorNode m_parent = null;
	// 현재 노드의 상태를 표기합니다.
	protected BehaviorStatus m_status = BehaviorStatus.Invaild;


	// 상태를 반환합니다.
	public virtual BehaviorStatus Update() => BehaviorStatus.Invaild;

	// 무슨 상태인지 검사하는 함수 입니다.
	public bool IsSuccess() => m_status == BehaviorStatus.Success;
	public bool IsFailure() => m_status == BehaviorStatus.Failure;
	public bool IsRunning() => m_status == BehaviorStatus.Running;

	// 상태를 초기화합니다.
	public void StateReset() { m_status = BehaviorStatus.Invaild; }
}
