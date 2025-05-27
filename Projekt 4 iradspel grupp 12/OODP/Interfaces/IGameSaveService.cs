using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OODP.Interfaces
{
    public interface IGameSaveService // Gränssnitt för sparfunktioner
    {
        void SaveGame(string filePath, Stack<Tuple<int, int, bool>> moveHistory);
        List<Tuple<int, int, bool>> LoadGame(string filePath);
    }
}