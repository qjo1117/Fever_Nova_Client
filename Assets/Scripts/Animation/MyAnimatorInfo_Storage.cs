using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Define;
using AI;

// Player, Enemy들의 Ainmation Info (Animation Clip FileName, Animator Controller Parametar) 를 저장하고있는 클래스
class MyAnimatorInfo_Storage
{
    static private MyAnimatorInfo_Storage m_instance = null;                                    // instance 변수

    // Player의 MyAnimator에 설정되어있는 m_playerId값을 Key값으로 사용
    private Dictionary<int, Dictionary<Player_AniState, string>> m_playerAniFileName;           // Player Animation Clip FileName
    private Dictionary<int, Dictionary<Player_AniParametar, string>> m_playerAniParametar;      // Player Animator Controller Parametar

    // Enemy들의 MyAnimator에 설정되어있는 m_enemySubjectType을 Key값으로 사용
    private Dictionary<EnemyType, Dictionary<Enemy_AniState, string>> m_enemyAniFileName;       // Enemy Animation Clip FileName
    private Dictionary<EnemyType, Dictionary<Enemy_AniParametar, string>> m_enemyAniParametar;  // Enemy Animator Controller Parametar

    private MyAnimatorInfo_Storage()
    {
        m_playerAniFileName = new Dictionary<int, Dictionary<Player_AniState, string>>();
        m_playerAniParametar = new Dictionary<int, Dictionary<Player_AniParametar, string>>();

        m_enemyAniFileName = new Dictionary<EnemyType, Dictionary<Enemy_AniState, string>>();
        m_enemyAniParametar = new Dictionary<EnemyType, Dictionary<Enemy_AniParametar, string>>();
    }

    static public MyAnimatorInfo_Storage Instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = new MyAnimatorInfo_Storage();
            }
            return m_instance;
        }
    }

    public Dictionary<int, Dictionary<Player_AniState, string>> PlayerAniFileName { get => m_playerAniFileName; }
    public Dictionary<int, Dictionary<Player_AniParametar, string>> PlayerAniParametar { get => m_playerAniParametar; }
    public Dictionary<EnemyType,Dictionary<Enemy_AniState,string>> EnemyAniFileName { get => m_enemyAniFileName; }
    public Dictionary<EnemyType,Dictionary<Enemy_AniParametar,string>> EnemyAniParametar { get => m_enemyAniParametar; }

}
