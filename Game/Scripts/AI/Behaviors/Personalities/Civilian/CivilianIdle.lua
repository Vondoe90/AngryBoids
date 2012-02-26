local Behavior = CreateAIBehavior("CivilianIdle",
{
	Alertness = 0,

	OnCivilianPOIDetected = function(self, entity, sender, data)
		entity:ReleaseTargetPOI();	-- Don't select the same POI twice in a row
		entity:SetTargetPOI(sender);
		entity:SelectPipe(0, "Civilian_GoToPOI");
	end,
	OnCivilianPOIDone = function(self, entity)
		entity:SelectPipe(0, "Civilian_DoNothing");
		AI.ModifySmartObjectStates(entity.id, "-Busy");
	end,
	
	OnBulletRain = function(self, entity, sender, data)
		local attentionTarget = AI.GetAttentionTargetEntity(entity.id);
		if (attentionTarget) then
			local faction = AI.GetFactionOf(attentionTarget.id);
			if (faction == "Players") then
				entity:InsertSubpipe(AIGOALPIPE_NOTDUPLICATE, "Civilian_LookAtPlayer");
			else
				-- TODO: Run away
			end
		end
	end,
})
