using CryEngine;

namespace CryGameCode.AngryBoids
{
	public class PlayerCamera : Actor
	{
		public void Init()
		{
			View.ActiveView.FieldOfView = Math.DegreesToRadians(60);

			ReceiveUpdates = true;
		}

		public override void OnUpdate()
		{
			if(TargetEntity == null)
				return;

			Position = TargetEntity.Position + new Vec3(-30, 0, 0);
		}

		/// <summary>
		/// Follow this entity
		/// </summary>
		public Entity TargetEntity { get; set; }
	}
}