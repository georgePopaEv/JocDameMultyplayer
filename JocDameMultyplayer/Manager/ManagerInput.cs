using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace JocDameMultyplayer.Manager
{
    
    class ManagerInput
    {

        private Client _client;

        public ManagerInput(Client client)
        {
            _client = client; // se asigneaza clientul care face mutariel / care da comenzile de input
        }
        public void Update(double gameTime)
        {
            var state = Keyboard.GetState(); //se citeste starea tastaturii , care si ce pasta este apasata
            CheckKeyState(Keys.Down, state);
        }

        private void CheckKeyState(Keys key, KeyboardState state)
        {
            if (state.IsKeyDown(key))
            {
                _client.SendInput(key);
            }
        }
    }
}
