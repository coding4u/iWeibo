﻿using System;
using System.Net;
using System.Collections.Generic;

namespace TencentWeiboSDK.Model
{
    /// <summary>
    /// 微腾微博列表，包含微博对象，以及微博所提到的用户列表
    /// </summary>
    public class StatusCollection:List<Status>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StatusCollection()
        {
            this.Users = new List<User>();
        }

        /// <summary>
        /// 该列表中所提到的用户列表.
        /// </summary>
        public List<User> Users { get; set; }

        /// <summary>
        /// 列表中最后一条微博的时间戳
        /// </summary>
        public long LastTimeStamp
        {
            get
            {
                return this[this.Count - 1].TimeStamp;
            }
        }

        /// <summary>
        /// 列表中第一条微博的时间戳
        /// </summary>
        public long FirstTimeStamp
        {
            get
            {
                return this[0].TimeStamp;
            }
        }
    }
}