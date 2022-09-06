namespace AI
{
    public enum State
    {
        SUCCESS, //����
        FAILURE, //����
        RUNNING, //������
        ERROR //����
    }

    public enum EnemyType
    {
        Melee,
        Range,
        Boss,
    }

    public enum EnemyState
    {
        Idle,
        Recon,
        Conflict
    }

    #region ������ Animation Define�� ���� �ּ�(�о��ּ���)
    // ���� �ִϸ����Ϳ��� ���������� ���Ǵ� �Ķ����, Animation ���´� ���� �տ� ��ü(������ State�ΰ�) ǥ�� X
    // �̰� ���� �Ķ���ͳ�, Animation ���µ��� ���� ���Ǵ� �ִϸ������� ��ü�� �տ� ǥ����
    // ex) Player_ ... / Meele_ ...

    // �ִϸ����Ϳ� ���Ǵ� �Ķ���͵� ����

    // �����ϴ� �ܾ���� ��������,�Ķ������ �ڷ����� ���� �޶�����.
    // Bool�� .. Flag�� ����
    // Int�� .. Int�� ����
    // Float�� ... Float�� ����
    // Trigger�� ... Trigger�� ����

    // ���� !!)
    // MyAnimator�� Animation Patametar name, Animator Clip name�� �ڵ����� �ʱ�ȭ ���ִ� AutoParametarInitialize(),
    // AutoAniFileNameInitialize() �Լ��� enum���� �Ҵ�� ���ڿ��� �������� �ϱ⋚����

    // State enum���� ���� State �̸��� ���ԵǾ���ϰ�, AniParametar enum���� ���� Animator Parametar �̸��� ���ԵǾ����.
    // ex) ���� State�� : Run, => State enum���� Melee_Run, Boss_Run, Run ....,
    // ���� Parametar�� : Shot (Trigger) => Parametar enum���� ShotTrigger
    // ���� Parametar�� : Move (Bool) => MoveFlag
    #endregion

    // ------------------------------- Enemy Animation Define
    public enum Enemy_AniState
    {
        Idle,
        Death,
        Move,
        NormalAttack,

        // --------------------------- Only Boss Animation State
        Boss_ShieldAttack,
        Boss_PistolAttack,
        Boss_ChargeAttack,
        Boss_Idle_Injured,
        Boss_Idle_Crouch
    }

    public enum Enemy_AniParametar
    {
        None,
        DeathFlag,
        MoveFlag,
        AttackFlag,

        // --------------------------- Only Boss Animation Parametar
        Boss_ShieldAttackFlag,
        Boss_PistolAttackFlag,
        Boss_ChargeAttackFlag,
        Boss_Idle_InjuredFlag,
        Boss_Idle_CrouchFlag
    }
}