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
}