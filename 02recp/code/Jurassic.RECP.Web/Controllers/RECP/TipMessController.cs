using Jurassic.AppCenter.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.RECP.Web.Controllers.RECP
{
    public class TipMessController : Controller
    {
        ReturnValueWithTips mTips;

        /// <summary>
        /// 本页的提示信息对象，它可能跨页传递给下一个页面。
        /// </summary>
        ReturnValueWithTips Tips
        {
            get
            {
                if (mTips == null)
                {
                    mTips = new ReturnValueWithTips();
                }
                return mTips;
            }
            set
            {
                mTips = value;
            }
        }
        /// <summary>
        /// 为前台的操作定义既要返回提示信息又要返回对象
        /// </summary>
        public class ReturnValueWithTips
        {
            /// <summary>
            /// 创建一个提示信息对象
            /// </summary>
            internal ReturnValueWithTips() { }
            /// <summary>
            /// 提示信息的内容
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// 提示信息的类型,可以是success,warning或error三种
            /// </summary>
            public string Type { get; set; }
            /// <summary>
            /// 提示信息的标题
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 附加的对象信息
            /// </summary>
            public object ReturnValue { get; set; }
            /// <summary>
            /// 提示信息是否有Type
            /// </summary>
            public bool IsEmpty
            {
                get { return String.IsNullOrWhiteSpace(Type); }
            }
            /// <summary>
            /// 根据一个对象创建提示信息对象
            /// </summary>
            /// <param name="value"></param>
            internal ReturnValueWithTips(object value)
            {
                ReturnValue = value;
            }
            /// <summary>
            /// 根据提示信息和对象创建提示对象
            /// </summary>
            /// <param name="type">提示信息的类型,可以是success,warning或error三种</param>
            /// <param name="title">提示信息的标题</param>
            /// <param name="message">提示信息的内容</param>
            /// <param name="value">附加的对象信息</param>
            public ReturnValueWithTips(string type, string title, string message, object value)
            {
                this.Message = message;
                this.Type = type;
                this.Title = title;
                ReturnValue = value;
            }
        }
        /// <summary>
        /// 使用多语言版本显示信息
        /// </summary>
        /// <param name="type">error,warning,success中的一个</param>
        /// <param name="titleKey">信息标题的多语言Key</param>
        /// <param name="messageKey">信息内容的多语言Key</param>
        /// <param name="returnValue">附加的对象数据</param>
        /// <param name="formatArgs">以message作为格式化字符串的格式化参数</param>
        /// <returns>Json序列化后的ReturnValueWithTips对象</returns>
        protected JsonResult JsonTipsLang(string type, string titleKey, string messageKey, object returnValue = null, params object[] formatArgs)
        {
            Tips.Type = type;
            Tips.Title = ResHelper.GetStr(titleKey);
            Tips.Message += String.Format(ResHelper.GetStr(messageKey), formatArgs);
            Tips.ReturnValue = returnValue;
            return Json(Tips);
        }
    }
}