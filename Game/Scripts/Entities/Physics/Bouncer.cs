using CryEngine;

namespace CryGameCode.Entities
{
	/// <summary>
	/// Quite possibly the cutest sphere in town.
	/// </summary>
	public class Bouncy : Entity
	{
		[EditorProperty]
		public float BounceMultiplier { get; set; }

		[EditorProperty(DefaultValue = 10)]
		public float Mass { get { return Physics.Mass; } set { Physics.Mass = value; } }

		protected override void OnCollision(uint targetEntityId, Vec3 hitPos, Vec3 dir, short materialId, Vec3 contactNormal)
		{
			Console.LogAlways("Bouncy.OnCollision (ID = {0}), targetID = {1}, hitpos = {2}, dir = {3}, materialId = {4}, normal = {5} | strength is {6}",
				this.Id, targetEntityId, hitPos, dir, materialId, contactNormal, -GlobalPhysics.GravityZ * BounceMultiplier);

			Physics.AddImpulse(new Vec3(0, 0, -GlobalPhysics.GravityZ * BounceMultiplier));
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