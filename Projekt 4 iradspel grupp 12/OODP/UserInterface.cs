using OODP.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OODP
{
    internal class UserInterface : IUserInterface // Implementerar gränssnittet IUserInterface
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}