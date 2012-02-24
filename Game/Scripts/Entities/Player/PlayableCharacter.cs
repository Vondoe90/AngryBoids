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
			InputSystem.RegisterAction("move_forward", OnJump);
			ReceiveUpdates = true;
		}

		public void OnRevive()
		{
			LoadObject(@"Objects/default/primitive_pyramid.cgf");
			Physics.Type = PhysicalizationType.Rigid;
			Physics.Mass = 100;

			//Spawn the player camera
			var playerCamera = EntitySystem.SpawnEntity<PlayerCamera>("Camera", new Vec3(), new Vec3(), new Vec3(1, 1, 1));

			playerCamera.Camera = new Camera { FieldOfView = 60 };
			playerCamera.Target = this;

			Renderer.Camera = playerCamera.Camera;
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