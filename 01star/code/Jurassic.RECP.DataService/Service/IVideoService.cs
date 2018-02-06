using Jurassic.RECP.DataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Service
{
    public interface IVideoService
    {
        /// <summary>
        /// 获取视频信息
        /// </summary>
        IEnumerable<RECP_VideoModel> GetVideoInfo(RECP_VideoModel model);

        /// <summary>
        /// 处理视频信息
        /// </summary>
        void ModifyVideoInfoByTree(RECP_VideoModel model);

        /// <summary>
        /// 保存视频信息
        /// </summary>
        void SaveVideoInfo(RECP_VideoModel model);
    }
}
