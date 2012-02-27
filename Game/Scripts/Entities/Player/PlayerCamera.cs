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

			Renderer.FieldOfView = 60;

			if(Target == null)
				Target = EntitySystem.SpawnEntity<PlayerEntity>("Player", Position);
			else
			{
				Target.Position = Position;
				Target.Rotation = Rotation;
			}

			CameraOffset = new Vec3(0, -10, 2);
		}
	}
}