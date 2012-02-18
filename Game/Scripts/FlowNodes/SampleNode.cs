using System.Collections.Generic;

using CryEngine;

namespace CryGameCode
{
    [FlowNode(UICategory = "Samples", Category = FlowNodeCategory.Approved, Description = "Does awesome CryMono things")]
    public class SampleNode : FlowNode
    {
        [Port(Name = "Activate", Description = "Test of a void input")]
        public void OnActivateTriggered()
        {
            Console.LogAlways("The activate port was triggered.");

            activatedPortId.Activate();
        }

        [Port(Name = "Test Int", Description = "Test of an int input")]
        public void OnIntTriggered(int value)
        {
            Console.LogAlways("The int port was triggered, value is {0}", value.ToString());

            testIntPortId.Activate(value);
        }

        [Port(Name = "Activated", Description = "")]
        public OutputPort activatedPortId;

        [Port(Name = "Test Int", Description = "")]
        public OutputPort<int> testIntPortId;
    }
}