using HackerNews.Model;
using System.Collections.Generic;

namespace HackerNews.Service
{
    public interface IRackerNews
    {
      List<NewsDbo>  GetHackerNews();
    }
}
