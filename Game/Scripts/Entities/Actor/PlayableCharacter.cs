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

			InputSystem.RegisterAction("sprint", OnActionSprint);

			ReceiveUpdates = true;

			DesiredVelocity = new Vec3();

			camera = new Camera();
			Renderer.Camera = camera;
        }

		public override void OnUpdate()
		{
			Quat qRot = new Quat();
			qRot.Axis = Rotation;

			Position += qRot * DesiredVelocity;

			camera.Position = Position;
			camera.ViewDir = Rotation;

			Renderer.Camera = camera;
		}

		public float GetMovementSpeed()
		{
			const float movementSpeed = 1.0f;

			if (Sprinting)
				return movementSpeed * 10.0f;

			return movementSpeed;
		}

		public void OnMoveForward(InputSystem.ActionActivationMode activationMode, float value)
		{
			DesiredVelocity.Y = GetMovementSpeed() * value;
		}

		public void OnMoveBackward(InputSystem.ActionActivationMode activationMode, float value)
		{
			DesiredVelocity.Y = GetMovementSpeed() * -value;
		}

		public void OnMoveRight(InputSystem.ActionActivationMode activationMode, float value)
		{
			DesiredVelocity.X = GetMovementSpeed() * value;
		}

		public void OnMoveLeft(InputSystem.ActionActivationMode activationMode, float value)
		{
			DesiredVelocity.X = GetMovementSpeed() * -value;
		}

		public void OnActionSprint(InputSystem.ActionActivationMode activationMode, float value)
		{
			Sprinting = value > 0;
		}

		private Camera camera;

		private bool Sprinting;
		private Vec3 DesiredVelocity;
    }
}