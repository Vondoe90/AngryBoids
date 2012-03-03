using CryEngine;

namespace CryGameCode.Entities
{
	[Entity(Category="TestCategory", EditorHelper="Editor/Objects/anchor.cgf", Icon="", Flags=EntityClassFlags.Default)]
    public class SampleEntity : StaticEntity
    {
        public override void OnSpawn()
        {
            Debug.LogAlways("OnSpawn");
        }

		//Floats/ints have optional constraints
		[EditorProperty(Description="How awesome is this entity?", Min=0, Max=9001)]
		public float awesomenessLevel { get; set; }

        [EditorProperty(Type = EntityPropertyType.File)]
		public string fileSelector { get; set; }

		[EditorProperty(Type=EntityPropertyType.Object)]
		public string objectSelector { get; set; }

		[EditorProperty(Type=EntityPropertyType.Texture)]
		public string textureSelector { get; set; }

		[EditorProperty(Type=EntityPropertyType.Sound)]
		public string soundSelector { get; set; }

		[EditorProperty(Type=EntityPropertyType.Dialogue)]
		public string dialogueSelector { get; set; }

		[EditorProperty(Type=EntityPropertyType.Color)]
		public Vec3 colorSelector { get; set; }

		[EditorProperty]
		public Vec3 vectorTest { get; set; }

		[EditorProperty(Type=EntityPropertyType.Sequence)]
		public string sequenceTest { get; set; }

		[EditorProperty(Description="Is this entity epic?")]
		public bool isEpic { get; set; }
    }
}