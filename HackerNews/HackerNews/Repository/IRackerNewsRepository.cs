using HackerNews.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerNews.Repository
{
    public interface IRackerNewsRepository
    {
        List<NewsDbo> GetNewsCacheData();
        List<NewsDbo> SetHackerNewsData(List<NewsDbo> newHackerData);
    }
}
