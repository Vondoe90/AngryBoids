using CryEngine;
using CryGameCode.Entities;

namespace CryGameCode.AngryBoids
{
	public abstract class AngryBoidBase : Entity
	{
		public void Launch(Vec3 velocity, Launcher launcher)
		{
			this.launcher = launcher;

			Physics.AddImpulse(velocity);
			OnLaunched(velocity);
		}

		public virtual void OnLaunched(Vec3 velocity) 
		{
		}

		public override void OnUpdate()
		{
			if (launcher != null && Velocity.Length == 0 && (Time.FrameStartTime - lastEvent) > 4000)
			{
				ReceiveUpdates = false;

				launcher.PostFire();

				launcher = null;
				ReceiveUpdates = false;
			}
		}

		protected override void OnCollision(EntityId targetEntityId, Vec3 hitPos, Vec3 dir, short materialId, Vec3 contactNormal)
		{
			// Hit something, reset last event time.
			if (launcher != null)
				lastEvent = Time.FrameStartTime;
		}

		float lastEvent;
		Launcher launcher = null;
	}
}