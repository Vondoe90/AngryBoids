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

			textureId = Renderer._LoadTexture(@"Textures/skys/night/half_moon.dds");
        }

		public void OnRevive()
		{
			//LoadObject(@"Objects/default/primitive_sphere.cgf");
			//Physics.Type = PhysicalizationType.Rigid;
			//Physics.Mass = 25;
		}

		public override void OnUpdate()
		{
			Renderer._DrawTextureToScreen(0, 0, 256, 256, textureId);

			if (camera == null)
				camera = new Camera();

			Quat qRot = new Quat();
			qRot.Axis = Rotation;

			Position += qRot * desiredVelocity;

			camera.Position = Position + new Vec3(0,0,20);
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

		public void OnMoveForward(ActionActivationMode activationMode, float value)
		{
			desiredVelocity.Y = MovementSpeed * value;
		}

		public void OnMoveBackward(ActionActivationMode activationMode, float value)
		{
			desiredVelocity.Y = MovementSpeed * -value;
		}

		public void OnMoveRight(ActionActivationMode activationMode, float value)
		{
			desiredVelocity.X = MovementSpeed * value;
		}

		public void OnMoveLeft(ActionActivationMode activationMode, float value)
		{
			desiredVelocity.X = MovementSpeed * -value;
		}

		public void OnActionSprint(ActionActivationMode activationMode, float value)
		{
			Sprinting = value > 0;
		}

		Camera camera;
		Vec3 desiredVelocity;

		public bool Sprinting { get; private set; }

		int textureId;
    }
}