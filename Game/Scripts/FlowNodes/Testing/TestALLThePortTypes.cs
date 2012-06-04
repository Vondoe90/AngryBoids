using CryEngine;

namespace CryGameCode.FlowNodes.Testing
{
	[FlowNode(UICategory = "Samples",
		Description = "Testing node for various node operations",
		Category = FlowNodeCategory.Debug)]
	public class TestALLThePortTypes : FlowNode
	{
		[Port(Name = "Activation Test", Description = "")]
		public void Activate() { activatedOutput.Activate(); }

		[Port(Name = "Default Value Test", Description = "")]
		public void TestAll()
		{
			activatedOutput.Activate();
			intOutput.Activate(GetPortInt(IntInput));
			floatOutput.Activate(GetPortFloat(FloatInput));
			stringOutput.Activate(GetPortString(StringInput));
			boolOutput.Activate(GetPortBool(BoolInput));
			vec3Output.Activate(GetPortVec3(Vec3Input));
			entityIdOutput.Activate(GetPortEntityId(EntityIdInput));
		}

		#region Data Inputs

		[Port(Name = "Integer Test")]
		public void IntInput(int value) { intOutput.Activate(value); }

		[Port(Name = "Float Test")]
		public void FloatInput(float value) { floatOutput.Activate(value); }

		[Port(Name = "Bool Test")]
		public void BoolInput(bool value = true) { boolOutput.Activate(value); }

		[Port(Name = "String Test")]
		public void StringInput(string value = "woo default value") { stringOutput.Activate(value); }

		[Port(Name = "Vec3 Test")]
		public void Vec3Input(Vec3 value) { vec3Output.Activate(value); }

		[Port(Name = "EntityId Test")]
		public void EntityIdInput(EntityId value) { entityIdOutput.Activate(value); }

		#endregion

		#region Outputs

		[Port(Name = "Activated Output")]
		public OutputPort activatedOutput { get; set; }

		[Port(Name = "Int Output")]
		public OutputPort<int> intOutput { get; set; }

		[Port(Name = "Float Output")]
		public OutputPort<float> floatOutput { get; set; }

		[Port(Name = "String Output")]
		public OutputPort<string> stringOutput { get; set; }

		[Port(Name = "Vec3 Output")]
		public OutputPort<Vec3> vec3Output { get; set; }

		[Port(Name = "Bool Output")]
		public OutputPort<bool> boolOutput { get; set; }

		[Port(Name = "EntityId Output")]
		public OutputPort<EntityId> entityIdOutput { get; set; }

		#endregion
	}
}
