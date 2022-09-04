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
}