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

			ReceiveUpdates = true;

			//Spawn the player camera
			var playerCamera = EntitySystem.SpawnEntity<PlayerCamera>("Camera");

			playerCamera.Camera = new Camera { FieldOfView = 60 };
			playerCamera.Target = this;
        }

		public void OnRevive()
		{
			LoadObject(@"Objects/default/primitive_sphere.cgf");
			Physics.Type = PhysicalizationType.Rigid;
			Physics.Mass = 100;
		}

		public void OnMoveRight(ActionActivationMode activationMode, float value)
		{
			Physics.AddImpulse(new Vec3 { Y = 10 });
		}

		public void OnMoveLeft(ActionActivationMode activationMode, float value)
		{
			Physics.AddImpulse(new Vec3 { Y = -10 });
		}
    }
}