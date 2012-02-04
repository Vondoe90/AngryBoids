-- Zero Gravity entity
		
GravitySphere = {
	Properties = {
		bActive = 1,
		Radius = 10,
		bUniform = 1,
		Gravity = { x=0,y=0,z=0 },
		Falloff = { x=0,y=0,z=0 },
		Damping = 0,
	},

		_PhysTable = { Area={}, },
		
		Editor={
		Icon = "GravitySphere.bmp",
		IconOnTop=1,
		},
}



-------------------------------------------------------
function GravitySphere:OnLoad(table)
  self.bActive = table.bActive
	self:PhysicalizeThis();
end

-------------------------------------------------------
function GravitySphere:OnSave(table)
  table.bActive = self.bActive
end

------------------------------------------------------------------------------------------------------
function GravitySphere:OnInit()
	self:OnReset();
end

------------------------------------------------------------------------------------------------------
-- OnPropertyChange called only by the editor.
------------------------------------------------------------------------------------------------------
function GravitySphere:OnPropertyChange()
	if (self.bActive ~= self.Properties.bActive) then
		self:OnReset();
	end
end

------------------------------------------------------------------------------------------------------
-- OnReset called only by the editor.
------------------------------------------------------------------------------------------------------
function GravitySphere:OnReset()
	self.bActive = self.Properties.bActive;
	self:PhysicalizeThis();
end

------------------------------------------------------------------------------------------------------
function GravitySphere:PhysicalizeThis()
	--Log("GravitySphere:PhysicalizeThis");
	if (self.bActive == 1) then
		local Properties = self.Properties;
		local Area = self._PhysTable.Area;
		Area.type = AREA_SPHERE;
		Area.radius = Properties.Radius;
		Area.uniform = Properties.bUniform;
		Area.falloff = Properties.Falloff;
		Area.gravity = Properties.Gravity;
		Area.damping = Properties.Damping;
		
		self:Physicalize( 0,PE_AREA,self._PhysTable );
		
		self:SetPhysicParams(PHYSICPARAM_FOREIGNDATA,{foreignData = ZEROG_AREA_ID});
	else
		self:DestroyPhysics();
	end
end

------------------------------------------------------------------------------------------------------
function GravitySphere:Event_Activate()
	if (self.bActive ~= 1) then
		self.bActive = 1;
		self:PhysicalizeThis();
		BroadcastEvent(self, "Activate");
	end
end


------------------------------------------------------------------------------------------------------
function GravitySphere:Event_Deactivate()
	if (self.bActive ~= 0) then
		self.bActive = 0;
		self:PhysicalizeThis();
		BroadcastEvent(self, "Deactivate");
	end
end

GravitySphere.FlowEvents =
{
	Inputs =
	{
		Deactivate = { GravitySphere.Event_Deactivate, "bool" },
		Activate = { GravitySphere.Event_Activate, "bool" },
	},
	Outputs =
	{
		Deactivate = "bool",
		Activate = "bool",
	},
}
