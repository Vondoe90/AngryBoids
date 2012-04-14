using CryEngine;
using CryGameCode.Entities;

namespace CryGameCode.AngryBoids
{
	[Entity(Category = "AngryBoids")]
	public class TheBoringOne : AngryBoid
	{
		protected override void OnCollision(EntityId targetEntityId, Vec3 hitPos, Vec3 dir, short materialId, Vec3 contactNormal)
		{
			base.OnCollision(targetEntityId, hitPos, dir, materialId, contactNormal);
		}
	}
}