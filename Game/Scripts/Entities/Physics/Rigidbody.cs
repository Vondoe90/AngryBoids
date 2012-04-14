using CryEngine;

namespace CryGameCode.Entities
{
	[Entity(Category = "Physics")]
	public class Rigidbody : Entity
	{
		[EditorProperty(Type = EntityPropertyType.Object)]
		public string Model { get { return GetObjectFilePath(); } set { LoadObject(value); } }

		[EditorProperty]
		public float Mass { get { return Physics.Mass; } set { Physics.Mass = value; } }

		protected override void OnReset(bool enteringGame)
		{
			Physics.Type = PhysicalizationType.Rigid;
		}
	}
}
