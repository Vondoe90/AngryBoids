using CryEngine;
using CryGameCode.Entities;

namespace CryGameCode.Entities.AngryBoids
{
	public abstract class AngryBoid : Rigidbody
	{
		public AngryBoid()
		{
			ReceiveUpdates = true;
		}

		protected BoidState state = BoidState.Ready;

		public void Launch(Vec3 velocity)
		{
			state = BoidState.Launched;
			Physics.AddImpulse(velocity);
			OnLaunched(velocity);
		}

		public virtual void OnLaunched(Vec3 velocity) { }

		public override void OnUpdate()
		{
			if (Launcher.Instance != null && Math.IsInRange(Velocity.Length, -0.2, 0.2) && (Time.FrameStartTime - lastEvent) > 4000)
			{
				ReceiveUpdates = false;
				state = BoidState.Dead;

				Launcher.Instance.PostFire();
			}
		}

		protected override void OnCollision(EntityId targetEntityId, Vec3 hitPos, Vec3 dir, short materialId, Vec3 contactNormal)
		{
			// Hit something, reset last event time.
			if(state == BoidState.Launched)
				lastEvent = Time.FrameStartTime;
		}

		/// <summary>
		/// Defines the state of the boid
		/// </summary>
		protected enum BoidState
		{
			/// <summary>
			/// Before the boid has been used
			/// </summary>
			Ready,

			/// <summary>
			/// Whilst the boid is in play
			/// </summary>
			Launched,

			/// <summary>
			/// After the boid has finished its turn
			/// </summary>
			Dead
		}

		float lastEvent;
	}
}