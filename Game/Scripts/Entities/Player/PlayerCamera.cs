using CryEngine;

namespace CryGameCode.Entities
{
	public class PlayerCamera : Entity
	{
		public Camera Camera { get; set; }
		public Player Target { get; set; }

		public override void OnUpdate()
		{
			if(Camera == null || Target == null)
				return;

			Camera.Position = Target.Position - new Vec3(5, 5, 5);
			Camera.ViewDir = new Vec3(0, 0, 0);
		}
	}
}