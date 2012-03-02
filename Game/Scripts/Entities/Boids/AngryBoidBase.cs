using CryEngine;
using CryGameCode.Entities;

namespace CryGameCode.AngryBoids
{
	public abstract class AngryBoidBase : Entity
	{
		public virtual void Init() { }

		public void Launch(Vec3 velocity)
		{
			Physics.AddImpulse(velocity);
			OnLaunched(velocity);
		}

		public virtual void OnLaunched(Vec3 velocity) { }
	}
}