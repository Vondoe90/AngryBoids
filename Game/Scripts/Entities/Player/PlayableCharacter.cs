using CryEngine;

namespace CryGameCode.Entities
{
	public class PlayerEntity : Entity
	{
		protected override void OnReset(bool enteringGame)
		{
			LoadObject(@"Objects/default/primitive_pyramid.cgf");
			Physics.Type = PhysicalizationType.Rigid;
			Physics.Mass = 10;

			InputSystem.RegisterAction("move_right", OnMoveRight);
			InputSystem.RegisterAction("move_left", OnMoveLeft);
			InputSystem.RegisterAction("jump", OnJump);
		}

		public override void OnSpawn()
		{
			OnReset(false);
		}

		public override void OnPostScriptReload()
		{
			OnReset(false);	
		}

		public void OnMoveRight(ActionActivationMode activationMode, float value)
		{
			Physics.AddImpulse(new Vec3 { X = 10 });
		}

		public void OnMoveLeft(ActionActivationMode activationMode, float value)
		{
			Physics.AddImpulse(new Vec3 { X = -10 });
		}

		public void OnJump(ActionActivationMode activationMode, float value)
		{
			Physics.AddImpulse(new Vec3 { Z = 10 });
		}
	}
}