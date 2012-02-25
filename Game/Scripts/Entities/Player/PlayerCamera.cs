using CryEngine;

namespace CryGameCode.Entities
{
	public class PlayerCamera : Entity
	{
		public override void OnUpdate()
		{
			if(Target == null)
				return;

			Camera.Position = Target.Position + new Vec3(0, 0, 10);
			Camera.Angles = new Vec3(0, 0, 0);

			Renderer.Camera = Camera;
		}

		public override void OnSpawn()
		{
			Camera = new Camera { FieldOfView = 60 };

			Renderer.Camera = Camera;
		}

		public Camera Camera;
		public Player Target { get; set; }
	}
}