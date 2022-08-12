namespace AI
{
    public enum State
    {
        SUCCESS, //성공
        FAILURE, //실패
        RUNNING, //동작중
        INVALID, //비활성화
        ERROR //에러
    }

    public enum NodeType
    {
        ROOT, //뿌리
        SELECTOR, //셀렉터
        SEQUENCE, //시퀀스
        PARALLEL, //페러렐
        DECORATOR, //데코레이터
        CONDITION, //조건문
        ACTION //액션
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
}