using NetTopologySuite.Triangulate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty.Media
{
    public interface IMediaIterator
    {
        string GenerateNextNews();
        public void DisplayALL();
    }
}
