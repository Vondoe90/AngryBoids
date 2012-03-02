using CryEngine;
using CryGameCode.Entities;

namespace CryGameCode.AngryBoids
{
	public class TheBoringOne : AngryBoidBase
	{
		public override void Init()
		{
			LoadObject("objects/default/primitive_sphere.cgf");
			Physics.Type = PhysicalizationType.Rigid;
			Physics.Mass = 50;
		}

		protected override void OnCollision(EntityId targetEntityId, Vec3 hitPos, Vec3 dir, short materialId, Vec3 contactNormal)
		{
			if(EntitySystem.GetEntity(targetEntityId) is Rigidbody)
			{
				Debug.LogAlways("Hit a rigidbody!");
			}
		}
	}
}