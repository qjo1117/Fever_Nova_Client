namespace AI
{
    public enum State
    {
        SUCCESS, //����
        FAILURE, //����
        RUNNING, //������
        INVALID, //��Ȱ��ȭ
        ERROR //����
    }

    public enum NodeType
    {
        ROOT, //�Ѹ�
        SELECTOR, //������
        SEQUENCE, //������
        PARALLEL, //�䷯��
        DECORATOR, //���ڷ�����
        CONDITION, //���ǹ�
        ACTION //�׼�
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