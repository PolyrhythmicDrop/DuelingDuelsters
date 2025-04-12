using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuelingDuelsters.Classes
{
    internal class MenuManager
    {
        public MenuManager(ConsoleKeyInfo keyInfo)
        {
            _keyInfo = keyInfo;
        }

        ConsoleKeyInfo _keyInfo;
        public ConsoleKeyInfo KeyInfo {
            get => _keyInfo; 
            set => _keyInfo = value; 
        }

        public ConsoleKey Key { 
            get => _keyInfo.Key; 
        }


    }
}
