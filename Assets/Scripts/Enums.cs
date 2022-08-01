namespace AI
{
    public enum State
    {
        SUCCESS,
        FAILURE,
        RUNNING,
        INVALID,
        ERROR
    }

    public enum NodeType
    {
        ROOT,
        SELECTOR,
        SEQUENCE,
        PARALLEL,
        DECORATOR,
        CONDITION,
        ACTION
    }

    public enum EnemyType
    {
        Melee,
        Boss
    }
}