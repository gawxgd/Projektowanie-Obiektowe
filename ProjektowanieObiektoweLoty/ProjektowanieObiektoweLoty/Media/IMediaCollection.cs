using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty.Media
{
    public interface IMediaCollection
    {
        public IMediaIterator CreateIterator();
    }
}
