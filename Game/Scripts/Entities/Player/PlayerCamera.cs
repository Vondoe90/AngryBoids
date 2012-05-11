using CryEngine;

namespace CryGameCode.AngryBoids
{
	public class PlayerCamera : Actor
	{
		public void Init()
		{
			View.Active.FoV = Math.DegreesToRadians(60);

			CurrentZoomLevel = MaxZoomLevel;
			ReceiveUpdates = true;

			Input.RegisterAction("zoom_in", OnActionZoomIn);
			Input.RegisterAction("zoom_out", OnActionZoomOut);

			Input.MouseEvents += ProcessMouseEvents;
		}

		public override void OnUpdate()
		{
			if(TargetEntity == null)
				return;

			Position = TargetEntity.Position - new Vec3(MaxDistanceFromTarget * ((float)CurrentZoomLevel / MaxZoomLevel), 0, 0);
		}

		public void OnActionZoomIn(object sender, ActionMapEventArgs e)
		{
			if(e.KeyEvent == KeyEvent.OnPress && CurrentZoomLevel > 1)
				CurrentZoomLevel--;
		}

		public void OnActionZoomOut(object sender, ActionMapEventArgs e)
		{
			if(e.KeyEvent == KeyEvent.OnPress && CurrentZoomLevel < MaxZoomLevel)
				CurrentZoomLevel++;
		}

		private void ProcessMouseEvents(object sender, MouseEventArgs e)
		{
			switch(e.MouseEvent)
			{
				// TODO: Rotate camera view around the TargetEntity.
				case MouseEvent.RightButtonDown:
					break;
			}
		}

		/// <summary>
		/// Follow this entity
		/// </summary>
		public Entity TargetEntity { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public const int MaxZoomLevel = 5;
		public const float MaxDistanceFromTarget = 100;

		public int CurrentZoomLevel { get; set; }
	}
}