using CryEngine;

namespace CryGameCode.Entities
{
	public class PlayerCamera : Entity
	{
		public override void OnUpdate()
		{
			if(Target == null)
				return;

			ViewParams.idTarget = 0;
			ViewParams.position = Target.Position + new Vec3(0, 0, 100);
			ViewParams.rotation = new Quat(new Vec3(0, 0, 0));

			Renderer.ViewParams = ViewParams;
		}

		public override void OnSpawn()
		{
			ViewParams = Renderer.ViewParams;

			ViewParams.fov = (float)Math.DegToRad(60);
			ViewParams.idTarget = 0;

			Renderer.ViewParams = ViewParams;
		}

		public ViewParams ViewParams;
		public Player Target { get; set; }
	}
}