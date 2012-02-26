using CryEngine;

namespace CryGameCode.Entities
{
	public class PlayerEntity : Entity
	{
		Player PlayerProxy { get; set; }

		protected override void OnReset(bool enteringGame)
		{
			LoadObject(@"Objects/default/primitive_pyramid.cgf");
			Physics.Type = PhysicalizationType.Rigid;
			Physics.Mass = 10;

			ReceiveUpdates = true;

			InputSystem.RegisterAction("move_right", OnMoveRight);
			InputSystem.RegisterAction("move_left", OnMoveLeft);
			InputSystem.RegisterAction("jump", OnJump);
		}

		public override void OnUpdate()
		{
			if(PlayerProxy == null)
			{
				Console.LogAlways("PlayerEntity: No player proxy found! Not updating this frame");
				return;
			}

			Position = PlayerProxy.Position + PlayerProxy.CameraOffset;
			Console.LogAlways("Position is {0}, proxy pos is {1}", Position, PlayerProxy.Position);
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