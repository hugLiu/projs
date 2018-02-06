using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Jurassic.RECP.DataBase.Models;
using Jurassic.RECP.DataBase.Service;
using Jurassic.RECP.DataService.Models;

namespace Jurassic.RECP.DataService.Service
{
    public class GT_TopicCardService : IGT_TopicCardService
    {
        /// <summary>
        /// 数据层接口
        /// </summary>
        private IGT_TopicCardEFRepository GT_TopicCardEFRepository { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="IGT_TopicCardEFRepository"></param>
        public GT_TopicCardService(IGT_TopicCardEFRepository IGT_TopicCardEFRepository)
        {
            GT_TopicCardEFRepository = IGT_TopicCardEFRepository;
        }

        public ProductType GetProductTypeByTopicId(int topicId)
        {
            var gt_topicCard = GT_TopicCardEFRepository.GetQuery().Where(t => t.TopicId == topicId).ToList();

            string[] ptArray = new string[gt_topicCard.Count];
            for(var i=0;i<gt_topicCard.Count;i++)
            {
                ptArray[i] = gt_topicCard[i].Definition;
            }
            var producttype = new ProductType();
            producttype.pt = ptArray;
            return producttype;
        }

        public class ProductType
        {
            public Array pt;
        }
    }
}
