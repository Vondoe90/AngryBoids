using CryEngine;

namespace CryGameCode.BoidUIEvents
{
	[UINode(Name = "OnBoidLaunched", Description = "Triggered when a boid is shot", Category = "BoidEvents")]
	public class OnBoidLaunched
	{
		[Port(Name = "BoidName", Description = "Name of the launched boid")]
		public OutputPort<string> boidType;

		[Port(Name = "Remaining Boids", Description = "Amount of boids left to launch")]
		public OutputPort<int> boidsLeft;
	}

	[UINode(Name = "SetDifficulty", Description = "Sets Angry Boids difficulty level", Category = "BoidEvents")]
	public class SetDifficulty : UIFunction
	{
		[Port(Name = "DifficultyLevel", Description = "The difficulty level we want to set!")]
		public string DifficultyLevel { get; set; }

		[Port(Name = "RequireRestart", Description = "Should difficulty be changed immediately or wait until next round")]
		public bool RequireRestart { get; set; }
	}
}