using CryEngine;

[Entity(Category = "Test", Icon = "")]
public class RenderTest : Entity
{
	[EditorProperty(Type = EntityPropertyType.Object, DefaultValue="Objects/default.cgf")]
	public string Model { set { LoadObject(value); } }
}