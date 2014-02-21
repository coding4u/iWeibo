using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiboSdk.Services
{
    public abstract class BaseService
    {
        public BaseService(string accessToken)
        {
            this.accessToken = accessToken;
            this.netEngine = new SdkNetEngine();
        }

        private string accessToken;

        public string AccessToken
        {
            get { return accessToken; }
        }

        private SdkNetEngine netEngine;

        public SdkNetEngine NetEngine
        {
            get { return netEngine; }
        }


    }
}
