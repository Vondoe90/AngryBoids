local Behavior = CreateAIBehavior("VtolGoto", "HeliGoto",
{
	Alertness = 0,
	
	OnEnemySeen = function( self, entity, fDistance )
		AIBehaviour.HELIDEFAULT:heliRequest2ndGunnerShoot( entity );
	end,
})