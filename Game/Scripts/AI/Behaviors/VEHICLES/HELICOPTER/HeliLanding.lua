local Behavior = CreateAIBehavior("HeliLanding", "HeliReinforcement",
{
	Alertness = 0,

	Constructor = function( self, entity, sender, data )

		AIBehaviour.HeliReinforcement:Constructor( entity );

	end,
})