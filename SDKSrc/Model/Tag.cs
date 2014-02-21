using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TencentWeiboSDK.Model
{
    /// <summary>
    /// 标签 Model, 用于表示用户标签的对象.
    /// </summary>
    public class Tag : BaseModel
    {
        private string id;
        private string name;


        /// <summary>
        /// 构造函数
        /// </summary>
        public Tag()
        { }

        /// <summary>
        /// 标签id
        /// </summary>
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                if (value != id)
                {
                    id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

        
        /// <summary>
        /// 标签名
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value != name)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }
    }
}
