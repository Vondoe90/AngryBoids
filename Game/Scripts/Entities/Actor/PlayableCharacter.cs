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

			desiredVelocity = new Vec3();

			camera = new Camera();
			Renderer.Camera = camera;
        }

		public override void OnUpdate()
		{
			Quat qRot = new Quat();
			qRot.Axis = Rotation;

			Position += qRot * desiredVelocity;

			camera.Position = Position;
			camera.ViewDir = Rotation;

			Renderer.Camera = camera;
		}

		public float MovementSpeed
		{
			get
			{
				var movementSpeed = 1f;

				if(Sprinting)
					movementSpeed *= 10;

				return movementSpeed;
			}
		}

		public void OnMoveForward(InputSystem.ActionActivationMode activationMode, float value)
		{
			desiredVelocity.Y = MovementSpeed * value;
		}

		public void OnMoveBackward(InputSystem.ActionActivationMode activationMode, float value)
		{
			desiredVelocity.Y = MovementSpeed * -value;
		}

		public void OnMoveRight(InputSystem.ActionActivationMode activationMode, float value)
		{
			desiredVelocity.X = MovementSpeed * value;
		}

		public void OnMoveLeft(InputSystem.ActionActivationMode activationMode, float value)
		{
			desiredVelocity.X = MovementSpeed * -value;
		}

		public void OnActionSprint(InputSystem.ActionActivationMode activationMode, float value)
		{
			Sprinting = value > 0;
		}

		Camera camera;
		Vec3 desiredVelocity;

		public bool Sprinting { get; private set; }
    }
}