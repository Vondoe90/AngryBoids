using CryEngine;

namespace CryGameCode.Entities
{
    public class Dragon : Entity
    {
        public override void OnUpdate()
        {
            //Velocity += new Vec3(2, 2, 0);
        }

		[Port(Name = "Let panic ensue", Description = "Kill them all.")]
		public void FlyAroundAndSetPeopleOnFire()
		{
			// TODO
		}

        public override void OnSpawn()
        {
			ReceiveUpdates = true;

            LoadObject(Model);

			spawnedPort.Activate();
        }

        protected override void OnReset(bool enteringGame)
        {
            LoadObject(Model);

			Physics.Mass = 5000;
			Physics.Type = PhysicalizationType.Rigid;
			Physics.Stiffness = 70;
        }

		[Port(Name = "Spawned", Description = "")]
		public static OutputPort spawnedPort;

        [EditorProperty(Type = EntityPropertyType.Object, DefaultValue="Objects/Characters/Dragon/Dragon.cdf")]
        public string Model { get { return GetObjectFilePath(); } set { LoadObject(value); } }
    }
}