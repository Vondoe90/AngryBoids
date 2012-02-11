using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine;

namespace CryGameCode
{
    public class Player : BasePlayer
    {
        public Player() 
        {
			InputSystem.RegisterAction("move_forward", OnMoveForward);
			InputSystem.RegisterAction("move_backward", OnMoveBackward);
			InputSystem.RegisterAction("move_right", OnMoveRight);
			InputSystem.RegisterAction("move_left", OnMoveLeft);

			ReceiveUpdates = true;

			velocity = new Vec3();

			camera = new Camera();
			Renderer.Camera = camera;
        }

		public override void OnUpdate()
		{
			Position += velocity;

			camera.Position = Position;
			camera.ViewDir = Rotation;

			Renderer.Camera = camera;
		}

        public override void OnSpawn()
        {
            Console.LogAlways("Player.OnSpawn");
        }

		public void OnMoveForward(InputSystem.ActionActivationMode activationMode, float value)
		{
		}

		public void OnMoveBackward(InputSystem.ActionActivationMode activationMode, float value)
		{
		}

		public void OnMoveRight(InputSystem.ActionActivationMode activationMode, float value)
		{
		}

		public void OnMoveLeft(InputSystem.ActionActivationMode activationMode, float value)
		{
		}

		private Vec3 velocity;
		private Camera camera;
    }
}