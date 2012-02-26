using CryEngine;

namespace CryGameCode.Entities
{
	public class PlayerCamera : StaticEntity
	{
		public override void OnUpdate()
		{
			ViewParams.idTarget = EntitySystem.GetEntity("test").Id;
			ViewParams.position = new Vec3(0, 0, 100);
			ViewParams.rotation = new Quat(new Vec3(0, 0, 0));

			Renderer._SetViewParams(ViewId, ViewParams);
			Renderer._SetActiveView(ViewId);
		}

		public override void OnSpawn()
		{
			ReceiveUpdates = true;

			ViewId = Renderer._CreateView();
			ViewParams = Renderer._GetViewParams(ViewId);

			ViewParams.fov = (float)Math.DegToRad(60);
			ViewParams.idTarget = 0;

			Renderer._SetViewParams(ViewId, ViewParams);
			Renderer._SetActiveView(ViewId);
		}

		public ViewParams ViewParams;
		public uint ViewId;

		public Player Target { get; set; }
	}
}