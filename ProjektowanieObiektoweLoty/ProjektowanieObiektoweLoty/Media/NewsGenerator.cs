using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty.Media
{
    public class NewsGenerator : IMediaIterator, IEnumerable<string>
    {
        private List<(IMedia media, IReportable reportable)> CartesianProductList;
        private IEnumerator<string> enumerator;
        public NewsGenerator(List<IMedia> mediaList,List<IReportable> reportableList) 
        {
            var product = (from media in mediaList
                           from reportable in reportableList
                           select new { media, reportable }).ToList();
            CartesianProductList = new List<(IMedia, IReportable)>();
            foreach(var item in product)
            {
                CartesianProductList.Add((item.media, item.reportable));
            }
            enumerator = GetEnumerator();
        }
        public string GenerateNextNews()
        {
            
            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }
            else
            {
                return null;
            }
        }
        public IEnumerator<string> GetEnumerator()
        {
            foreach (var item in CartesianProductList)
            {
                yield return item.reportable.ReportFor(item.media);
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void DisplayALL()
        {
            foreach(var item in CartesianProductList)
            {
                Console.WriteLine(item.reportable.ReportFor(item.media));
            }
        }
    }
}
