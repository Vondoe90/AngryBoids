using CryEngine;

namespace CryGameCode.Entities
{
	/// <summary>
	/// HAX LIKE YOU'VE NEVER HAXED BEFORE
	/// </summary>
	public class PlayerCameraProxy : BasePlayer
	{
		public StaticEntity Target { get; set; }
		public Vec3 CameraOffset { get; set; }

		public override void OnUpdate()
		{
			if(Target == null)
				return;

			Position = Target.Position + CameraOffset;
		}

		public void OnRevive()
		{
			//var viewId = Renderer._GetActiveView();
			//var viewParams = Renderer._GetViewParams(viewId);
			//viewParams.fov = (float)Math.DegToRad(60);
			//Renderer._SetViewParams(viewId, viewParams);

			Target = EntitySystem.SpawnEntity<Player>("Player Entity", Position, Rotation, new Vec3(1, 1, 1));
			CameraOffset = new Vec3(0, -10, 2);
		}
	}
}