using System;
using CryEngine;

namespace CryGameCode
{
    public struct ExplosionInfo
    {
        EntityId shooterId; // Id of the shooter who triggered the explosion
        EntityId weaponId; // Id of the weapon used to create the explosion
        EntityId projectileId;//Id of the "bullet" (grenade, claymore...) to create the explosion
        UInt16 projectileClassId;

        float damage; // damage created by the explosion

        Vec3 pos; // position of the explosion
        Vec3 dir; // direction of the explosion
        float minRadius;	// min radius of the explosion
        float radius;	// max radius of the explosion
        float soundRadius;
        float minPhysRadius;
        float physRadius;
        float angle;
        float pressure; // pressure created by the explosion
        float hole_size;
        IntPtr pParticleEffect;
        string effect_name; // this is needed because we don't load particle effects on the dedicated server,
        // so we need to store the name if we are going to send it
        string effect_class;
        float effect_scale;
        int type; // type id of the hit, see IGameRules::GetHitTypeId for more information

        bool impact;
        bool propogate;
        bool explosionViaProxy; // true if the 'shooter' didn't actually shoot, ie. a weapon acting on their behalf did (team perks)

        Vec3 impact_normal;
        Vec3 impact_velocity;
        EntityId impact_targetId;
        float maxblurdistance;
        int friendlyfire;

        // Camera shake params
        float shakeMinR;
        float shakeMaxR;
        float shakeScale;
        float shakeRnd;

        //Flashbang params
        float blindAmount;
        float flashbangScale;

        int firstPassPhysicsEntities;  // Specify which physics types to hit on the first pass. defaults to 'ent_living', set to zero to disable
    }
}