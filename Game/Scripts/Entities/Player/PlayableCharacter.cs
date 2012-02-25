using CryEngine;
using CryGameCode.Entities;

namespace CryGameCode
{
	public class Player : BasePlayer
	{
		public Player()
		{
			InputSystem.RegisterAction("move_right", OnMoveRight);
			InputSystem.RegisterAction("move_left", OnMoveLeft);
			InputSystem.RegisterAction("jump", OnJump);
			ReceiveUpdates = true;
		}

		public void OnRevive()
		{
			LoadObject(@"Objects/default/primitive_pyramid.cgf");
			Physics.Type = PhysicalizationType.Rigid;
			Physics.Mass = 100;

			//Spawn the player camera
			PlayerCam = EntitySystem.SpawnEntity<PlayerCamera>("Camera", new Vec3(), new Vec3(), new Vec3(1, 1, 1));

			PlayerCam.Target = this;
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
			Physics.AddImpulse(new Vec3 { Z = 1 });
		}

		PlayerCamera PlayerCam;
	}
}