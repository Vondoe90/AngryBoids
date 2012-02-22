using CryEngine;

namespace CryGameCode.Entities
{
	/// <summary>
	/// Quite possibly the cutest sphere in town.
	/// </summary>
	public class Bouncy : Entity
	{
		protected override void OnCollision(uint targetEntityId, Vec3 hitPos, Vec3 dir, short materialId, Vec3 contactNormal)
		{
			StaticEntity targetEnt = EntitySystem.GetEntity(targetEntityId);
			if (targetEnt != null)
			{
				targetEnt.Physics.AddImpulse(new Vec3(0,0,9.81f * 20));
			}
		}

		protected override void OnReset(bool enteringGame)
		{
			LoadObject(Model);
			Physics.Type = PhysicalizationType.Rigid;
			Physics.Stiffness = 70;
			Physics.Mass = 10;
		}

		[EditorProperty(Type = EntityPropertyType.Object)]
		public string Model { get { return GetObjectFilePath(); } set { LoadObject(value); } }
	}
}