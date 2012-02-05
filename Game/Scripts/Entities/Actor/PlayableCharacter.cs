using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine;

namespace CryGameCode
{
    public class Player : BasePlayer
    {
        public Player() 
        {
			InputSystem.RegisterAction("move_right", OnMoveRight);
			InputSystem.RegisterAction("move_left", OnMoveLeft);

			ReceiveUpdates = true;

			m_velocity = new Vec3();
        }

		public override void OnUpdate()
		{
			Position += m_velocity;
		}

        public override void OnSpawn()
        {
            Console.LogAlways("Player.OnSpawn");
        }

		public void OnMoveRight(InputSystem.ActionActivationMode activationMode, float value)
		{
			m_velocity.X = value;
		}

		public void OnMoveLeft(InputSystem.ActionActivationMode activationMode, float value)
		{
			m_velocity.X = -value;
		}

		private Vec3 m_velocity;
    }
}