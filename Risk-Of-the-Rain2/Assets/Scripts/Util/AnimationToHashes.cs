using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class GolemAnimID
{
    public static readonly int ON_DAMAGED = Animator.StringToHash("_OnDamaged");
    public static readonly int DEAD = Animator.StringToHash("_Dead");
}
public static class AnimID

{
    public static readonly int MOVE_X = Animator.StringToHash("MoveX");
    public static readonly int MOVE_Y = Animator.StringToHash("MoveY");
    public static readonly int EULER_X = Animator.StringToHash("EulerX");
    public static readonly int EULER_Y = Animator.StringToHash("EulerY");

    public static readonly int PISTOL_LAUNCHED = Animator.StringToHash("PistolLaunched");
    public static readonly int PISTOL_DIRECTION = Animator.StringToHash("PistolDirection");

    public static readonly int IS_JUMPING = Animator.StringToHash("IsJumping");
}
public static class LayerID

{
    public static readonly string PLAYER = "Player";
    public static readonly string ENEMY = "ENEMY";
}

public static class TagID

{
    public static readonly string COMMANDO_BASIC_ATTACK = "CommandoBasicAttack";
    public static readonly string COMMANDO_ULTIMATE_ATTACK = "CommandoUltimateAttack";
    public static readonly string GOLEM = "Golem";
    public static readonly string PLAYER = "Player";
    public static readonly string TERRAIN = "Terrain";
    public static readonly string ENEMY = "Enemy";
    
    public static readonly string SIZE_TARGET = "SizeTarget";

}

public static class GolemShaderParamID
{
    public static readonly string SHOWING_PART = "_ShowingPart";
    public static readonly string BRIGHTNESS = "_Brightness";
}

public static class MessageID
{
    public static readonly string BOSS_SPAWN_EFFECT_ON = "BossSpawnEffectOn";
   
}








