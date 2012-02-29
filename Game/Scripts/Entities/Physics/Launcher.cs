using CryEngine;

namespace CryGameCode.AngryBoids
{
	public class Launcher : Entity
	{
		[EditorProperty(Type = EntityPropertyType.Object)]
		public string Model { get { return GetObjectFilePath(); } set { LoadObject(value); } }

		[EditorProperty(Min = 0, Max = 10000)]
		public float ZForce { get; set; }

		[EditorProperty(Min = 0, Max = 10000)]
		public float YForce { get; set; }

		[EditorProperty(Min = 0, Max = 10000)]
		public float XForce { get; set; }

		protected override void OnReset(bool enteringGame)
		{
			InputSystem.RegisterAction("fire", Fire);
		}

		void Fire(ActionActivationMode mode = 0, float value = 0)
		{
			Console.LogAlways("Firing...");

			var poorBastard = EntitySystem.SpawnEntity<TheBoringOne>("sadface", Position);

			poorBastard.Init();
			poorBastard.Launch(new Vec3(XForce, YForce, ZForce) * poorBastard.Physics.Mass);

			Console.LogAlways("Fired!");
		}
	}
}