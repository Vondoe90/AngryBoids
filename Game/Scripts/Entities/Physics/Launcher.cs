using CryEngine;
using CryEngine.Utils;
using System.Collections.Generic;
using System.Linq;

namespace CryGameCode.AngryBoids
{
	/// <summary>
	/// The launcher is the entity that is responsible for firing the boids.
	/// It also handles calculations that determine the direction and force of the slingshot, using player input.
	/// </summary>
	public class Launcher : Entity
	{
		public static Launcher Instance { get; private set; }

		/// <summary>
		/// Here, we override an entity callback to register our mouse listener and grab all the boids in the level.
		/// </summary>
		/// <param name="enteringGame"></param>
		protected override void OnReset(bool enteringGame)
		{
			if(enteringGame)
			{
				var boids = EntitySystem.GetEntities<TheBoringOne>();

				if(boids == null)
				{
					Debug.LogAlways("Boids is null");
					return;
				}

				Debug.LogAlways("Found {0} boids", boids.Count());
				remainingBoids = boids.ToList();
				InputSystem.MouseEvents += ProcessMouseEvents;
				state = LauncherState.Ready;
				Instance = this;

				CurrentBoid.Position = Position;
				CurrentBoid.Physics.Resting = true;
			}
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
			switch(mouseEvent)
			{
				// If the event was the user left-clicking, then set the launcher into the Held state, which means we're getting ready to fire
				case MouseEvent.LeftButtonDown:
					{
						if(remainingBoids.Any())
						{
							CurrentBoid.Position = Position;
							CurrentBoid.Physics.Resting = true;
							state = LauncherState.Held;
							heldPos = Renderer.ScreenToWorld(x, y);
						}
					}
					break;

				// When the left mouse button is released, that's treated as the fire signal
				// (provided the state is Held, of course, just as a sanity check)
				case MouseEvent.LeftButtonUp:
					{
						if(state == LauncherState.Held)
							Fire(Renderer.ScreenToWorld(x, y));
					}
					break;
			}
		}

		private void Fire(Vec3 mousePosWorld)
		{
			Debug.LogAlways("Firing");

			state = LauncherState.Firing;

			var targetDir = heldPos - mousePosWorld;
			targetDir.X = 0;

			CurrentBoid.Physics.Resting = false;
			CurrentBoid.Launch(targetDir * LauncherStrength);
			remainingBoids.Remove(CurrentBoid);
			PostFire();
		}

		/// <summary>
		/// Invoked from the last fired boid (or the OnReset method) when it has stopped moving around.
		/// </summary>
		public void PostFire()
		{
			// TODO: Add a proper delay between boids
			state = LauncherState.Ready;
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

		/// <summary>
		/// Quick shortcut for accessing the current boid
		/// </summary>
		private TheBoringOne CurrentBoid
		{
			get
			{
				return remainingBoids.First();
			}
		}

		/// <summary>
		/// Defines the current state of the launcher; eg, is the launcher ready to fire
		/// </summary>
		private LauncherState state = LauncherState.Ready;

		private IList<TheBoringOne> remainingBoids;

		private Vec3 heldPos;

		private enum LauncherState
		{
			Ready,
			Held,
			Firing,
			Finished
		}
	}
}