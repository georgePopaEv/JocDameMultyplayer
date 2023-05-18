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
            CheckKeyState(Keys.S, state);
            CheckKeyState(Keys.W, state);
            CheckKeyState(Keys.A, state);
            CheckKeyState(Keys.D, state);
            CheckKeyState(Keys.Up, state);
            CheckKeyState(Keys.Right, state);
            CheckKeyState(Keys.Left, state);
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
