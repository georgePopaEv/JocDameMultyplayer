using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            var mstate = Mouse.GetState();

            
            CheckMouseState(mstate);

            CheckKeyState(Keys.Down, state);
            CheckKeyState(Keys.S, state);
            CheckKeyState(Keys.W, state);
            CheckKeyState(Keys.A, state);
            CheckKeyState(Keys.D, state);
            CheckKeyState(Keys.Up, state);
            CheckKeyState(Keys.Right, state);
            CheckKeyState(Keys.Left, state);
        }

        private void CheckMouseState(MouseState mstate)
        {
            if (mstate.LeftButton == ButtonState.Pressed)
            {
                Point mousePosition = new Point(mstate.X, mstate.Y);
                int row, col;
                GetRowColFromMouse(mousePosition,out row, out col);
                _client.SendClickPosition(row, col); // in client trebuie sa avem numele si culoarea acestuia pe care o alege 
                // din form-ul principal 
            }
                
        }
        public void GetRowColFromMouse(Point mousePosition, out int row, out int col)
        {
            // Presupunem că există o dimensiune fixă a fiecărei celule într-o grilă
            int cellSize = 50;

            // Calculăm rândul și coloana pe baza poziției mouse-ului și dimensiunii celulei
            row = mousePosition.Y / cellSize;
            col = mousePosition.X / cellSize;
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
