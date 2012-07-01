using System;
using CryEngine;

namespace CryGameCode
{
    public struct HitInfo
    {
        EntityId shooterId; // EntityId of the shooter
        EntityId targetId; // EntityId of the target which got shot
        EntityId weaponId; // EntityId of the weapon
        EntityId projectileId;  // 0 if hit was not caused by a projectile

        float damage; // damage count of the hit
        float impulse;
        float radius; // radius of the hit
        float angle;
        int material; // material id of the surface which got hit
        int type; // type id of the hit, see IGameRules::GetHitTypeId for more information
        int bulletType; //type of bullet, if hit was of type bullet

        float damageMin;
        float pierce; // bullet pierceability

        int partId;

        Vec3 pos; // position of the hit
        Vec3 dir; // direction of the hit
        Vec3 normal;

        UInt16 projectileClassId;
        UInt16 weaponClassId;

        bool remote;
        bool aimed; // set to true if shot was aimed - i.e. first bullet, zoomed in etc.
        bool knocksDown; // true if the hit should knockdown
        bool knocksDownLeg; // true if the hit should knockdown when hit in a leg
        bool hitViaProxy; // true if the 'shooter' didn't actually shoot, ie. a weapon acting on their behalf did (team perks)
        bool explosion; // true if this hit directly results from an explosion

        float armorHeating; // dynamic pierceability reduction
        int penetrationCount; // number of surfaces the bullet has penetrated
        bool forceLocalKill; // forces the hit to kill the victim + start hitdeathreaction/ragdoll NOT NET SERIALISED
        float impulseScale; // If this is non zero, this impulse will be applied to the partId set below.
    }
}