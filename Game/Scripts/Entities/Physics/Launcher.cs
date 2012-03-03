using CryEngine;
using CryEngine.Utils;

namespace CryGameCode.AngryBoids
{
	/// <summary>
	/// The launcher is the entity that is responsible for firing the boids.
	/// It also handles calculations that determine the direction and force of the slingshot, using player input.
	/// </summary>
	public class Launcher : Entity
	{
		/// <summary>
		/// Here, we override an entity callback to register our mouse listener.
		/// </summary>
		/// <param name="enteringGame"></param>
		protected override void OnReset(bool enteringGame)
		{
			InputSystem.MouseEvents += ProcessMouseEvents;

			if (enteringGame)
			{
				// spawn the first boid
				RemainingBoids = BoidCount;
				if(currentBoid == null && RemainingBoids > 0)
					currentBoid = EntitySystem.SpawnEntity<TheBoringOne>("sadface", Position);
			}

			Firing = false;
		}

		/// <summary>
		/// This function is used to process any mouse events, such as a change in cursor position, or clicks.
		/// </summary>
		/// <param name="x">The x position of the mouse on the screen.</param>
		/// <param name="y">The y position of the mouse on the screen.</param>
		/// <param name="mouseEvent">The event that occurred. Provides information on whether it was just a move, or whether something was clicked, etc.</param>
		/// <param name="wheelDelta"></param>
		private void ProcessMouseEvents(int x, int y, MouseEvent mouseEvent, int wheelDelta)
		{
			// Get the position of the cursor projected in the world
			var mousePos = Renderer.ScreenToWorld(x, y);

			// Set our targetDirection variable to represent the distance between the launcher itself, and the cursor
			var targetDirection = mousePos - Position;
			targetDirection = new Vec3(0, targetDirection.Y, targetDirection.Z);

			// If our direction is longer than the maximum distance of the slingshot, then we normalise it and multiply it by the distance to cap it
			if(targetDirection.Length > MaxPullDistance)
			{
				//targetDirection = targetDirection.Normalized * MaxPullDistance;
			}

			Debug.DrawLine(Position, Position + targetDirection, Color.Red, 0.3f);
			Debug.DrawSphere(mousePos, 2, Color.White, 0.3f);

			// This tells us which mouse events, such as clicks, occurred
			switch(mouseEvent)
			{
				// If the event was the user left-clicking, then set the launcher into the Held state, which means we're getting ready to fire
				case MouseEvent.LeftButtonDown:
					{
						state = LauncherState.Held;

						heldPos = Renderer.ScreenToWorld(x, y);
					}
					break;

				// When the left mouse button is released, that's treated as the fire signal
				// (provided the state is Held, of course, just as a sanity check)
				case MouseEvent.LeftButtonUp:
					if(state == LauncherState.Held)
						Fire(Renderer.ScreenToWorld(x, y));
					break;
			}
		}

		private void Fire(Vec3 mousePosWorld)
		{
			if (RemainingBoids <= 0 || Firing)
				return;

			Debug.LogAlways("Firing");

			Firing = true;

			var targetDir = heldPos - mousePosWorld;
			targetDir.X = 0;

			RemainingBoids--;
			currentBoid.Launch(targetDir * LauncherStrength, this);

			state = LauncherState.Ready;
		}

		/// <summary>
		/// Invoked from the last fired boid (or the OnReset method) when it has stopped moving around.
		/// </summary>
		public void PostFire()
		{
			Firing = false;

			if (currentBoid == null)
			{
				if (RemainingBoids > 0)
					currentBoid = EntitySystem.SpawnEntity<TheBoringOne>("sadface", Position);
				else
					currentBoid = null;
			}
		}

		/// <summary>
		/// Allows use to assign a model to this entity.
		/// </summary>
		[EditorProperty(Type = EntityPropertyType.Object)]
		public string Model { get { return GetObjectFilePath(); } set { LoadObject(value); } }

		/// <summary>
		/// Controls how far the spawned boids are thrown.
		/// </summary>
		[EditorProperty(Min = 0, Max = 10000, DefaultValue = 100)]
		public float LauncherStrength { get; set; }

		/// <summary>
		/// Controls how far the launcher can be pulled
		/// </summary>
		[EditorProperty(Min = 0, Max = 10000)]
		public float MaxPullDistance { get; set; }

		[EditorProperty(DefaultValue = 3)]
		public int BoidCount { get; set; }

		private int RemainingBoids;

		public bool Firing { get; set; }

		/// <summary>
		/// Defines the current state of the launcher; eg, is the launcher ready to fire
		/// </summary>
		private LauncherState state = LauncherState.Ready;

		private Vec3 heldPos { get; set; }

		private AngryBoidBase currentBoid;

		private enum LauncherState
		{
			Ready,
			Held
		}
	}
}