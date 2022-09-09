namespace AI
{
    public enum State
    {
        SUCCESS, //성공
        FAILURE, //실패
        RUNNING, //동작중
        ERROR //에러
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

    #region 찬혁의 Animation Define에 관한 주석(읽어주세요)
    // 여러 애니메이터에서 공통적으로 사용되는 파라메터, Animation 상태는 상태 앞에 주체(누구의 State인가) 표시 X
    // 이것 외의 파라메터나, Animation 상태들은 각자 사용되는 애니메이터의 주체를 앞에 표시함
    // ex) Player_ ... / Meele_ ...

    // 애니메이터에 사용되는 파라메터들 선언

    // 선언하는 단어들의 마지막은,파라메터의 자료형에 따라 달라진다.
    // Bool값 .. Flag로 끝남
    // Int값 .. Int로 끝남
    // Float값 ... Float로 끝남
    // Trigger값 ... Trigger로 끝남

    // 주의 !!)
    // MyAnimator의 Animation Patametar name, Animator Clip name을 자동으로 초기화 해주는 AutoParametarInitialize(),
    // AutoAniFileNameInitialize() 함수는 enum값에 할당된 문자열을 기준으로 하기떄문에

    // State enum에는 실제 State 이름이 포함되어야하고, AniParametar enum에는 실제 Animator Parametar 이름이 포함되어야함.
    // ex) 실제 State명 : Run, => State enum에서 Melee_Run, Boss_Run, Run ....,
    // 실제 Parametar명 : Shot (Trigger) => Parametar enum에서 ShotTrigger
    // 실제 Parametar명 : Move (Bool) => MoveFlag
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