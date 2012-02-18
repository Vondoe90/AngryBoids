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
		}

		#region Data Inputs

		[Port(Name = "Integer Test", Description = "")]
		public void IntInput(int value) { intOutput.Activate(value); }

		[Port(Name = "Float Test", Description = "")]
		public void FloatInput(float value) { floatOutput.Activate(value); }

		[Port(Name = "Bool Test", Description = "")]
		public void BoolInput(bool value = true) { boolOutput.Activate(value); }

		[Port(Name = "String Test", Description = "")]
		public void StringInput(string value = "woo default value") { stringOutput.Activate(value); }

		[Port(Name = "Vec3 Test", Description = "")]
		public void Vec3Input(Vec3 value) { vec3Output.Activate(value); }

		#endregion

		#region Outputs

		[Port(Name = "Activated Output", Description = "")]
		public OutputPort activatedOutput;

		[Port(Name = "Int Output", Description = "")]
		public OutputPort<int> intOutput;

		[Port(Name = "Float Output", Description = "")]
		public OutputPort<float> floatOutput;

		[Port(Name = "String Output", Description = "")]
		public OutputPort<string> stringOutput;

		[Port(Name = "Vec3 Output", Description = "")]
		public OutputPort<Vec3> vec3Output;

		[Port(Name = "Bool Output", Description = "")]
		public OutputPort<bool> boolOutput;

		#endregion
	}
}
