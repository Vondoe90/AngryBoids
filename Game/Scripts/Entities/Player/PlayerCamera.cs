using CryEngine;

using CryGameCode.Entities.AngryBoids;

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

			ReceiveUpdates = true;

			// Defined in Libs/Config/defaultprofile.xml
			Input.RegisterAction("zoom_in", OnActionZoomIn);
			Input.RegisterAction("zoom_out", OnActionZoomOut);

			Input.MouseEvents += ProcessMouseEvents;

			MaxZoomLevel = 5;
			MaxDistanceFromTarget = 100;

			CurrentZoomLevel = MaxZoomLevel;

			// The CVar attribute isn't functional at the moment, so we use this workaround.
			CVar.RegisterInt("g_camMaxZoomLevel", ref MaxZoomLevel);
			CVar.RegisterFloat("g_camMaxDistanceFromTarget", ref MaxDistanceFromTarget);

			TargetEntity = Launcher.Instance;
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
			if(e.KeyEvent == KeyEvent.OnPress && CurrentZoomLevel <= MaxZoomLevel)
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

		public static int MaxZoomLevel;
		public static float MaxDistanceFromTarget;

		public int CurrentZoomLevel { get; set; }
	}
}