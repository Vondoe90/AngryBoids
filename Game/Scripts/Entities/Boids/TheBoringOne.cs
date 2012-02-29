using CryEngine;

namespace CryGameCode.AngryBoids
{
	public class TheBoringOne : Entity
	{
		public void Init()
		{
			LoadObject("objects/default/primitive_sphere.cgf");
			Physics.Type = PhysicalizationType.Rigid;
			Physics.Mass = 50;
		}

		public void Launch(Vec3 velocity)
		{
			Physics.AddImpulse(velocity);
			Console.LogAlways("Launched, velocity is {0}", velocity);
		}

		protected override void OnCollision(EntityId targetEntityId, Vec3 hitPos, Vec3 dir, short materialId, Vec3 contactNormal)
		{

		}
	}
}