using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public static class AnimID

{
    public static readonly int MOVE_X = Animator.StringToHash("MoveX");
    public static readonly int MOVE_Y = Animator.StringToHash("MoveY");
    public static readonly int EULER_X = Animator.StringToHash("EulerX");
    public static readonly int EULER_Y = Animator.StringToHash("EulerY");

    public static readonly int PISTOL_LAUNCHED = Animator.StringToHash("PistolLaunched");
    public static readonly int PISTOL_DIRECTION = Animator.StringToHash("PistolDirection");
    
    public static readonly int IS_JUMPING= Animator.StringToHash("IsJumping");
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
    public static readonly string PLAYER = "Player";
    public static readonly string TERRAIN = "Terrain";
    public static readonly string ENEMY = "Enemy";

}






