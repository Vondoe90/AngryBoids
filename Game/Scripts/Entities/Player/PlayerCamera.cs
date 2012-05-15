using CryEngine;

using System.Linq;

namespace CryGameCode.Entities
{
	public class PlayerCamera : Actor
	{
		public void Init()
		{
			var spawnpoints = Entity.GetEntities<SpawnPoint>();
			if(spawnpoints.Count() > 0)
			{
				var spawn = spawnpoints.First();
				Position = spawn.Position;
				Rotation = spawn.Rotation;
			}

			var newView = View.Get(Id, true);

			newView.FoV = Math.DegreesToRadians(60);
			newView.TargetId = Id;
			newView.Position = Position;
			newView.Rotation = Rotation;

			View.Active = newView;

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

		public void OnActionZoomIn(ActionMapEventArgs e)
		{
			if(e.KeyEvent == KeyEvent.OnPress && CurrentZoomLevel > 1)
				CurrentZoomLevel--;
		}

		public void OnActionZoomOut(ActionMapEventArgs e)
		{
			if(e.KeyEvent == KeyEvent.OnPress && CurrentZoomLevel < MaxZoomLevel)
				CurrentZoomLevel++;
		}

		private void ProcessMouseEvents(MouseEventArgs e)
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