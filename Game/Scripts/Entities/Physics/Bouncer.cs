using CryEngine;

namespace CryGameCode.Entities
{
	/// <summary>
	/// Quite possibly the cutest sphere in town.
	/// </summary>
	[Entity(Category = "Physics")]
	public class Bouncy : Entity
	{
		[EditorProperty(Min = 0, Max = 10000)]
		public float BounceMultiplier { get; set; }

		[EditorProperty(DefaultValue = 10)]
		public float Mass { get { return Physics.Mass; } set { Physics.Mass = value; } }

		protected override void OnCollision(EntityId targetEntityId, Vec3 hitPos, Vec3 dir, short materialId, Vec3 contactNormal)
		{
			Physics.AddImpulse(contactNormal * BounceMultiplier);
		}

		protected override void OnReset(bool enteringGame)
		{
			LoadObject(Model);
			Physics.Type = PhysicalizationType.Rigid;
		}

		[EditorProperty(Type = EntityPropertyType.Object)]
		public string Model { get { return GetObjectFilePath(); } set { LoadObject(value); } }
	}
}