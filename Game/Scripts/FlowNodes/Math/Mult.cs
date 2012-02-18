using CryEngine;

namespace CryGameCode.FlowNodes.Samples
{
    [FlowNode(UICategory = "Samples", Description = "Reimplementation of multiplication in C#", Category = FlowNodeCategory.Approved)]
    public class Multiplier : FlowNode
    {
        [Port(Name = "Activate", Description = "Do the maths")]
        public void Activate()
        {
			answerOutput.Activate(GetPortFloat(LeftSide) * GetPortFloat(RightSide));
        }

        [Port(Name = "Left Side", Description = "The left side of the calculation")]
        public void LeftSide(float value) { }

        [Port(Name = "Right Side", Description = "The right side of the calculation")]
        public void RightSide(float value) { }

        [Port(Name = "Answer", Description = "Get the answer")]
        public OutputPort<float> answerOutput;
    }
}