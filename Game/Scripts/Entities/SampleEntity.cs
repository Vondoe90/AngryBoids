using CryEngine;

namespace CryGameCode.Entities
{
	[Entity(Category="Samples", EditorHelper="Editor/Objects/anchor.cgf", Icon="")]
    public class SampleEntity : Entity
	{
		public override void OnSpawn()
		{
			spawnedPort.Activate();
		}

		#region Entity Flownode ports
		[Port(Name = "Physicalize", Description = "")]
		public void Physicalize()
		{
			LoadObject(Model);

			Physics.Mass = Mass;
			Physics.Type = PhysicalizationType.Rigid;
			Physics.Stiffness = 70;
		}

		[Port(Name = "OnSpawn", Description = "")]
		public OutputPort spawnedPort;
		#endregion

		#region Editor Properties
		[EditorProperty(Min=0, Max=100)]
		public float Mass { get; set; }

        [EditorProperty(Type = EntityPropertyType.File)]
		public string FileSelector { get; set; }

		[EditorProperty(Type=EntityPropertyType.Object)]
		public string Model { get; set; }

		[EditorProperty(Type=EntityPropertyType.Texture)]
		public string TextureSelector { get; set; }

		[EditorProperty(Type=EntityPropertyType.Sound)]
		public string SoundSelector { get; set; }

		[EditorProperty(Type=EntityPropertyType.Dialogue)]
		public string DialogueSelector { get; set; }

		[EditorProperty(Type=EntityPropertyType.Color)]
		public Vec3 ColorSelector { get; set; }

		[EditorProperty]
		public Vec3 VectorTest { get; set; }

		[EditorProperty(Type=EntityPropertyType.Sequence)]
		public string SequenceTest { get; set; }

		[EditorProperty]
		public bool BooleanTest { get; set; }
		#endregion
	}
}