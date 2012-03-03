using CryEngine;
using CryGameCode.Entities;

namespace CryGameCode.AngryBoids
{
	public class TheBoringOne : AngryBoidBase
	{
		protected override void OnCollision(EntityId targetEntityId, Vec3 hitPos, Vec3 dir, short materialId, Vec3 contactNormal)
		{
			if(EntitySystem.GetEntity(targetEntityId) is Rigidbody)
			{
				Debug.LogAlways("Hit a rigidbody!");
			}

			base.OnCollision(targetEntityId, hitPos, dir, materialId, contactNormal);
		}
	}
}