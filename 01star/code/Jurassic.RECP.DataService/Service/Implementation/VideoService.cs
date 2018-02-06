using Jurassic.RECP.DataBase.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataService.Models;
using Jurassic.RECP.DataBase.Models;
using AutoMapper;
using Jurassic.RECP.DataService.Enum;

namespace Jurassic.RECP.DataService.Service
{
    public class VideoService : IVideoService
    {
        /// <summary>
        /// 数据层接口
        /// </summary>
        private IVideoEFRepository videoRepository { get; set; }

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="_albumRepository"></param>
        public VideoService(IVideoEFRepository _videoRepository)
        {
            videoRepository = _videoRepository;
        }

        /// <summary>
        /// 获取视频信息
        /// </summary>
        public IEnumerable<RECP_VideoModel> GetVideoInfo(RECP_VideoModel model)
        {
            IEnumerable<RECP_Video> info = videoRepository.GetQuery();
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.name)) info = info.Where(x => x.name == model.name);
                if (!string.IsNullOrEmpty(model.id)) info = info.Where(x => x.id == model.id);
                if (!string.IsNullOrEmpty(model.pid)) info = info.Where(x => x.pid == model.pid);
            }
            return info.Select(Mapper.Map<RECP_Video, RECP_VideoModel>).OrderBy(x => x.fieldIndex).ToList();
        }

        /// <summary>
        /// 通过目录树，处理视频信息（增，删，改）
        /// </summary>
        /// <param name="models"></param>
        public void ModifyVideoInfoByTree(RECP_VideoModel model)
        {
            if (model._state == EnumNodeState.added.ToString())
            {
                AddVideoInfo(model);
            }
            if (model._state == EnumNodeState.modified.ToString())
            {
                UpdateVideoInfo(model);
            }
            if (model._state == EnumNodeState.removed.ToString())
            {
                RemoveVideoInfo(model);
            }
        }

        /// <summary>
        /// 属性添加，对象树操作
        /// </summary>
        public void AddVideoInfo(RECP_VideoModel model)
        {
            videoRepository.Add(new RECP_Video
            {
                id = model.id,
                pid = model.pid,
                name = model.name
            });
        }

        /// <summary>
        /// 特定属性更新，对象树操作
        /// </summary>
        public void UpdateVideoInfo(RECP_VideoModel model)
        {
            var info = videoRepository.Find(t => t.id == model.id);
            if (info == null) return;
            info.name = model.name;
            info.pid = model.pid;
            videoRepository.Update(info);
        }

        /// <summary>
        /// 特定属性删除，对象树操作
        /// </summary>
        public void RemoveVideoInfo(RECP_VideoModel model)
        {
            var info = videoRepository.Find(t => t.id == model.id);
            if (info == null) return;
            videoRepository.Delete(info);
        }

        /// <summary>
        /// 特定属性信息保存
        /// </summary>
        public void SaveVideoInfo(RECP_VideoModel model)
        {
            var info = videoRepository.Find(t => t.id == model.id);
            if (info == null) return;
            info.name = model.name.Trim();
            info.fieldIndex = model.fieldIndex;
            info.url = model.url.Trim();
            info.link = model.link.Trim();
            videoRepository.Update(info);
        }
    }
}
