using CryEngine;

namespace CryGameCode.Entities
{
	/// <summary>
	/// HAX LIKE YOU'VE NEVER HAXED BEFORE
	/// </summary>
	public class Player : BasePlayer
	{
		public PlayerEntity Target { get; set; }
		public Vec3 CameraOffset { get; set; }

		public void OnRevive()
		{
			ReceiveUpdates = true;

			var viewId = Renderer._GetActiveView();
			var viewParams = Renderer._GetViewParams(viewId);
			viewParams.fov = (float)Math.DegToRad(60);
			Renderer._SetViewParams(viewId, viewParams);

			if(Target == null)
				Target = EntitySystem.SpawnEntity<PlayerEntity>("Player", Position, Rotation, new Vec3(1, 1, 1));
			else
			{
				Target.Position = Position;
				Target.Rotation = Rotation;
			}

			CameraOffset = new Vec3(0, -10, 2);
		}
	}
}