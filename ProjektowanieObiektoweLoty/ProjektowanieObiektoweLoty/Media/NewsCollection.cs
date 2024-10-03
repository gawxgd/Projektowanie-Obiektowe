using NetTopologySuite.Index.HPRtree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty.Media
{
    public class NewsCollection : IMediaCollection
    {
        public List<IMedia> MediaList;
        public List<IReportable> ReportableList;
        public NewsCollection()
        {
            CreateMediaAndAddToList();
            CreateReportableList();
        }
        private void CreateMediaAndAddToList()
        {
            MediaList = new List<IMedia>();
            Television Abelowa = new Television("Telewizja abelowa");
            MediaList.Add(Abelowa);
            Television Tensor = new Television("Kanal TV-tensor");
            MediaList.Add(Tensor);
            Radio Kwantyfikator = new Radio("Radio Kwantyfikator");
            MediaList.Add(Kwantyfikator);
            Radio Shmem = new Radio("Radio Shmem");
            MediaList.Add(Shmem);
            Newspaper cato = new Newspaper("Gazeta kategoryczna");
            MediaList.Add(cato);
            Newspaper poli = new Newspaper("Dziennik politechniczny");
            MediaList.Add(poli);
        }
        private void CreateReportableList()
        {
            ReportableList = new List<IReportable>();
            foreach(Airport a in CreateFtrObject.IDtoAirport.Values)
            {
                ReportableList.Add(a);
            }
            foreach (CargoPlane cp in CreateFtrObject.IDtoCargoPlane.Values)
            {
                ReportableList.Add(cp);
            }
            foreach (PassengerPlane pp in CreateFtrObject.IDtoPassengerPlane.Values)
            {
                ReportableList.Add(pp);
            }
        }
        public IMediaIterator CreateIterator()
        {
            return new NewsGenerator(MediaList, ReportableList);
        }
    }
}
