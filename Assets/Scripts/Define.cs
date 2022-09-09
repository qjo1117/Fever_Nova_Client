using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Path
{
    public const string Player                          = "Player";
    public const string Monster_Melee                   = "Assets/05Prefabs/Medical";
    public const string Monster_Range                   = "Assets/05Prefabs/Cop";
    public const string Monster_Boss                    = "Assets/05Prefabs/Robot";
    public const string Bomb                            = "Bomb";

    public const string Bomb_Particle                   = "Assets/05Prefabs/Boom_Particle";
    public const string Fire_Particle                   = "Assets/05Prefabs/Cop_Bullet";
    public const string FX_SwordStab_01                 = "Assets/05Prefabs/FX_SwordStab_01.prefab";
    public const string Robot_Bullet                    = "Assets/02 Skill/Robot_Bullet";
    public const string Bombardment_Charge_Indicator    = "Assets/05Prefabs/Bombardment_Charge_Indicator.prefab";
    public const string Bombardment_IndiCator           = "Assets/05Prefabs/Bombardment_IndiCator.prefab";
    public const string Bombardment_Effect              = "Assets/05Prefabs/Boom_Particle";

    public const string UI_NormalMonsterHPBar     = "UI/WorldSpace/UI_NormalMonsterHPBar";

    public const string UI_Aim              = "UI/Scene/UI_Aim";
    public const string UI_BombRange        = "UI/Scene/UI_BombRange";
    public const string UI_BombJumpRange    = "UI/Scene/UI_BombJumpRange";
    public const string UI_BombJumpArrow    = "UI/Scene/UI_BombJumpArrow";
    public const string UI_BombDropPoint    = "UI/Scene/UI_BombDropPoint";
    public const string UI_BossMonsterHPBar = "UI/Scene/UI_BossMonsterHPBar";
    public const string UI_PlayerHPBar      = "UI/Scene/UI_PlayerHPBar";
    public const string UI_Goal             = "UI/Scene/UI_Goal";
    public const string UI_Score            = "UI/Scene/UI_Score";
    public const string UI_Pause            = "UI/Scene/UI_Pause";
    public const string UI_MainMenu         = "UI/Scene/UI_MainMenu";

    public const string UI_PopupMsg         = "UI/Popup/UI_PopupMsg";
    public const string UI_PauseScreen      = "UI/Popup/UI_PauseScreen";
    public const string UI_ResultScreen     = "UI/Popup/UI_ResultScreen";
    public const string UI_Result           = "UI/Popup/UI_Result";
    public const string UI_StageSelect      = "UI/Popup/UI_StageSelect";
    public const string UI_GamePlaySelect   = "UI/Popup/UI_GamePlaySelect";
    public const string UI_Option           = "UI/Popup/UI_Option";
    public const string UI_KeyInput         = "UI/Popup/UI_KeyInput";

    public const string Health = "Health";

    public const string Slash_Particle = "Assets/Prefabs/FX_Slash_01";
}


namespace Define
{

    public enum MonsterType
	{
        Normal,
        Boss
	}

    public enum Layer
    {
        Ground = 6,
        Monster = 7,
        Player = 8,
        Block = 9,

    }

    public enum Scene
    {
        Unknown = -1,
        Main,
        InGame_1,
        InGame,//씬 이름이랑 일치해야해서 변경

    }


    public enum UserKey
    {
        Forward,
        Backward,
        Right,
        Left,
        Evasion,
        Shoot,
        Esc,
        End
    }

    public enum Mouse
    {
        PointerDown,
        Click,
        Press,
        PointerUp
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum BoomState
    {
        Default,
        Daley,
        Player
    }

    public enum SpawnType
    {
        Player,
        Monster
    }

    #region 찬혁의 Animation Define에 관한 주석(읽어주세요)
    // Player는 여러 애니메이터에서 사용되지 않으므로, 앞에 주체를 표시X
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

    // ------------------------ Player Animation Define
    public enum Player_AniState
    {
        Player_Run,
        Player_Evasion,
        GunJump,
        GunFire,
        GunAiming
    }

    public enum Player_AniParametar
    {
        None,
        JumpTrigger,
        ShotFlag,
        MoveXFloat,
        MoveZFloat,
        AimingFloat,
        FireFloat,
        EnvaisionTrigger
    }

}
