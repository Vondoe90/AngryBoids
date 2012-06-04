using CryEngine;

using System.Linq;

namespace CryGameCode.Entities
{
	public class PlayerCamera : Actor
	{
		public void Init()
		{
			var spawns = Entity.GetByClass<SpawnPoint>();
			if(spawns.Count() > 0)
			{
				Position = spawns.First().Position;
				Rotation = spawns.First().Rotation;
			}

			View = View.Get(Id);

			View.Position = Position;
			View.Rotation = Rotation;
			View.FieldOfView = Math.DegreesToRadians(60);

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

			View.Position = TargetEntity.Position + new Vec3(MaxDistanceFromTarget * ((float)CurrentZoomLevel / MaxZoomLevel), 0, 0);
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

		public View View { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public const int MaxZoomLevel = 5;
		public const float MaxDistanceFromTarget = 100;

		public int CurrentZoomLevel { get; set; }
	}
}