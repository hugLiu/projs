using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataService.Models;
using static Jurassic.RECP.DataService.Service.GT_TopicCardService;

namespace Jurassic.RECP.DataService.Service
{
    public interface IGT_TopicCardService
    {
        /// <summary>
        /// 通过topicID查阅GT_TopicCard表拿到所有的成果类型
        /// </summary>
        /// <param name="topicId">主题树Id</param>
        /// <returns></returns>
        ProductType GetProductTypeByTopicId(int topicId);
    }
}
