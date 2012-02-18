using CryEngine;
using CryEngine.Arduino;

using System.IO.Ports;

[FlowNode(UICategory = "Arduino", Description = "Controls the global port settings", Category = FlowNodeCategory.Advanced)] 
public class PortController : FlowNode
{
	[Port(Name = "Create Port", Description = "")]
	public void CreatePort()
	{
		if(ArduinoHelper.Port != null && ArduinoHelper.Port.IsOpen)
			ArduinoHelper.Port.Close();

		ArduinoHelper.Port = new SerialPort(GetPortString(PortName), GetPortInt(BaudRate));
		ArduinoHelper.Port.Open();
		createdOutput.Activate();
	}

	[Port(Name = "Destroy Port", Description = "")]
	public void DestroyPort()
	{
		if(ArduinoHelper.Port != null && ArduinoHelper.Port.IsOpen)
			ArduinoHelper.Port.Close();

		ArduinoHelper.Port = null;
		destroyedOutput.Activate();
	}

	[Port(Name = "Baud Rate", Description = "")]
	public void BaudRate(int rate) { }

	[Port(Name = "Port Name", Description = "")]
	public void PortName(string name) { }

	[Port(Name = "Created", Description = "")]
	public OutputPort createdOutput;

	[Port(Name = "Destroyed", Description = "")]
	public OutputPort destroyedOutput;	
}