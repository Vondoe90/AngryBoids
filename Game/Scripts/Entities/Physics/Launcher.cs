using CryEngine;
using CryEngine.Utilities;

using System.Collections.Generic;
using System.Linq;

using CryGameCode.Entities;

namespace CryGameCode.Entities.AngryBoids
{
	/// <summary>
	/// The launcher is the entity that is responsible for firing the boids.
	/// It also handles calculations that determine the direction and force of the slingshot, using player input.
	/// </summary>
	[Entity(Category = "AngryBoids")]
	public class Launcher : Entity
	{
		public static Launcher Instance { get; private set; }

		protected override void OnReset(bool enteringGame)
		{
			var boids = Entity.GetByClass<TheBoringOne>();

			if(boids == null || boids.Count() < 1)
			{
				Debug.Log("[Warning] No boids found in the level");
				return;
			}

			remainingBoids = boids.ToList();

			Input.MouseEvents += ProcessMouseEvents;

			state = LauncherState.Ready;

			Instance = this;

			CurrentBoid.Position = Position;

			CurrentBoid.Physics.Resting = true;

			var playerCamera = Actor.Client as PlayerCamera;
			playerCamera.TargetEntity = this;
		}

		/// <summary>
		/// This function is used to process any mouse events, such as a change in cursor position, or clicks.
		/// </summary>
		/// <param name="x">The x position of the mouse on the screen.</param>
		/// <param name="y">The y position of the mouse on the screen.</param>
		/// <param name="mouseEvent">The event that occurred. Provides information on whether it was just a move, or whether something was clicked, etc.</param>
		/// <param name="wheelDelta"></param>
		private void ProcessMouseEvents(MouseEventArgs e)
		{
			switch(e.MouseEvent)
			{
				// If the event was the user left-clicking, then set the launcher into the Held state, which means we're getting ready to fire
				case MouseEvent.LeftButtonDown:
					{
						Vec3 relativePos = CurrentBoid.Position - Renderer.ScreenToWorld(e.X, e.Y);
						// Check if the player clicked on the active boid.
						if(CurrentBoid.LocalBoundingBox.Contains(ref relativePos) && remainingBoids.Any())
						{
							CurrentBoid.Position = Position;
							CurrentBoid.Physics.Resting = true;
							state = LauncherState.Held;
						}
					}
					break;

				// When the left mouse button is released, that's treated as the fire signal
				// (provided the state is Held, of course, just as a sanity check)
				case MouseEvent.LeftButtonUp:
					{
						if(state == LauncherState.Held)
						{
							var playerCamera = Actor.Client as PlayerCamera;
							playerCamera.TargetEntity = CurrentBoid;

							Fire(Renderer.ScreenToWorld(e.X, e.Y));
						}
					}
					break;

				case MouseEvent.Move:
					{
						var screenWorldPos = Renderer.ScreenToWorld(e.X, e.Y);
						Debug.DrawSphere(screenWorldPos, 0.5f, Color.Red, 0.05f);

						if(state == LauncherState.Held && (Math.Pow(screenWorldPos.Y - Position.Y, 2) + Math.Pow(screenWorldPos.Z - Position.Z, 2) - Math.Pow(MaxPullDistance, 2)) <= 0)
						{
							CurrentBoid.Position = Position - new Vec3(0, Position.Y - screenWorldPos.Y, Position.Z - screenWorldPos.Z);
						}
					}
					break;
			}
		}

		private void Fire(Vec3 mousePosWorld)
		{
			state = LauncherState.Firing;

			var targetDir = Vec3.ClampXYZ(Position - mousePosWorld, -MaxPullDistance, MaxPullDistance);
			targetDir.X = 0;

			CurrentBoid.Physics.Resting = false;
			CurrentBoid.Launch(targetDir * LauncherStrength);
			remainingBoids.Remove(CurrentBoid);
		}

		/// <summary>
		/// Invoked from the last fired boid (or the OnReset method) when it has stopped moving around.
		/// </summary>
		public void PostFire()
		{
			var playerCamera = Actor.Client as PlayerCamera;
			playerCamera.TargetEntity = CurrentBoid;

			CurrentBoid.Position = Position;
			CurrentBoid.Physics.Resting = true;

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

		private enum LauncherState
		{
			Ready,
			Held,
			Firing,
			Finished
		}
	}
}