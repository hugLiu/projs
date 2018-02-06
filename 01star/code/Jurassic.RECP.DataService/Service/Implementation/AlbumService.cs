using AutoMapper;
using Jurassic.RECP.DataBase.Models;
using Jurassic.RECP.DataBase.Service;
using Jurassic.RECP.DataService.Enum;
using Jurassic.RECP.DataService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.RECP.DataService.Service
{
    public class AlbumService : IAlbumService
    {
        /// <summary>
        /// 数据层接口
        /// </summary>
        private IAlbumEFRepository albumRepository { get; set; }

        /// <summary>
        /// 构造函数注入
        /// </summary> 
        public AlbumService(IAlbumEFRepository _albumRepository)
        {
            albumRepository = _albumRepository;
        }

        /// <summary>
        /// 获取相册pid=null信息
        /// </summary> 
        public IEnumerable<RECP_AlbumModel> GetAlbumParentInfo()
        {
            IEnumerable<RECP_Album> info = albumRepository.GetQuery();
            info = info.Where(x => x.pid == null);
            return info.Select(Mapper.Map<RECP_Album, RECP_AlbumModel>).OrderBy(x => x.fieldIndex).ToList();
        }

        /// <summary>
        /// 获取相册信息
        /// </summary> 
        public IEnumerable<RECP_AlbumModel> GetAlbumInfo(RECP_AlbumModel model)
        {
            IEnumerable<RECP_Album> info = albumRepository.GetQuery();
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.id)) info = info.Where(x => x.id == model.id);
                if (!string.IsNullOrEmpty(model.name)) info = info.Where(x => x.name == model.name);
                if (!string.IsNullOrEmpty(model.pid)) info = info.Where(x => x.pid == model.pid);
            }
            return info.Select(Mapper.Map<RECP_Album, RECP_AlbumModel>).OrderBy(x => x.fieldIndex).ToList();
        }

        /// <summary>
        /// 保存相册文件信息
        /// </summary> 
        public void SaveAlbumSubInfo(IList<RECP_AlbumModel> models)
        {
            //先删除再添加 
            var pid = models[0].pid;
            albumRepository.DeleteList(t => t.pid == pid);
            foreach (var model in models)
            {
                albumRepository.Add(new RECP_Album
                {
                    id = model.id,
                    pid = model.pid,
                    name = model.name,
                    url = model.url,
                    fieldIndex = model.fieldIndex
                });
            }
        }

        /// <summary>
        /// 保存
        /// </summary> 
        public void SaveAlbumInfo(RECP_AlbumModel model)
        {
            var info = albumRepository.Find(t => t.id == model.id);
            if (info == null) return;
            info.name = model.name.Trim();
            info.fieldIndex = model.fieldIndex;
            info.url = model.url.Trim();
            albumRepository.Update(info);
        }

        /// <summary>
        /// 添加
        /// </summary>
        public void AddAlbumInfo(RECP_AlbumModel model)
        {
            albumRepository.Add(new RECP_Album
            {
                id = model.id,
                pid = model.pid,
                name = model.name,
                total = model.total,
                fieldIndex = model.fieldIndex
            });
            if (!string.IsNullOrEmpty(model.pid))
            {
                AddAlbumTotal(model.pid);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void UpdateAlbumInfo(RECP_AlbumModel model)
        {
            var info = albumRepository.Find(t => t.id == model.id);
            if (info == null) return;
            info.name = model.name;
            info.pid = model.pid;
            info.total = model.total;
            info.fieldIndex = model.fieldIndex;
            albumRepository.Update(info);
        }

        /// <summary>
        /// 通过目录树，处理视频信息（增，删，改）
        /// </summary>
        /// <param name="models"></param>
        public void ModifyAlbumInfoByTree(RECP_AlbumModel model)
        {
            if (model._state == EnumNodeState.added.ToString())
            {
                AddAlbumInfo(model);
            }
            if (model._state == EnumNodeState.modified.ToString())
            {
                UpdateAlbumInfo(model);
            }
            if (model._state == EnumNodeState.removed.ToString())
            {
                RemoveAlbumInfo(model);
            }
        }

        /// <summary>
        /// 特定属性删除，对象树操作
        /// </summary>
        public void RemoveAlbumInfo(RECP_AlbumModel model)
        {
            var info = albumRepository.Find(t => t.id == model.id);
            if (info == null) return;
            albumRepository.Delete(info);
            DeteleImg(info.url);
            MinusAlbumTotal(info.pid);
        }

        /// <summary>
        ///  删除相应图片
        /// </summary>
        public void DeteleImg(string url)
        {
            var physicalPath = AppDomain.CurrentDomain.BaseDirectory;
            var fileUrl = physicalPath.TrimEnd('\\') + url;
            if (!string.IsNullOrEmpty(url) && File.Exists(fileUrl)) File.Delete(fileUrl);
        }

        /// <summary>
        /// 减少相册文件夹文件的数量
        /// </summary>
        public void AddAlbumTotal(string pid)
        {
            var info = albumRepository.Find(t => t.id == pid);
            if (info == null) return;
            info.total = string.IsNullOrEmpty(info.total) ? "" : (int.Parse(info.total) + 1).ToString();
            albumRepository.Update(info);
        }

        public void MinusAlbumTotal(string pid)
        {
            var info = albumRepository.Find(t => t.id == pid);
            if (info == null) return;
            info.total = string.IsNullOrEmpty(info.total) ? "" : (int.Parse(info.total) - 1).ToString();
            albumRepository.Update(info);
        }
    }
}
