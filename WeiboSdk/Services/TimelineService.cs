using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiboSdk.Models;

namespace WeiboSdk.Services
{
    public class TimelineService:BaseService
    {
        public TimelineService(string accessToken)
            :base(accessToken)
        {

        }


        public void GetFriendsTimeline(
            int count,
            long maxId,
            long sinceId,
            Action<Callback<WStatusCollection>> action)
        {
            SdkCmdBase cmdArg = new CmdStatusTimeline
            {
                acessToken = this.AccessToken,
                count = count.ToString(),
                max_id = maxId.ToString(),
                since_id = sinceId.ToString()
            };

            this.NetEngine.RequestCmd(SdkRequestType.FRIENDS_TIMELINE, cmdArg, (requestType, response) =>
                {
                    if (action != null)
                    {
                        if (response.errCode == SdkErrCode.SUCCESS)
                        {
                            WStatusCollection collection = null;
                            collection = JsonConvert.DeserializeObject<WStatusCollection>(response.content);

                            action(new Callback<WStatusCollection>(collection));
                        }
                        else
                        {
                            action(new Callback<WStatusCollection>(ErrCodeToMsg.GetMsg(response.errCode)));
                        }
                    }
                });
        }


        public void GetMentionsTimeline(
            int count,
            long maxId,
            long sinceId,
            Action<Callback<WStatusCollection>> action)
        {
            SdkCmdBase cmdArg = new CmdStatusTimeline
            {
                acessToken = this.AccessToken,
                count = count.ToString(),
                max_id = maxId.ToString(),
                since_id = sinceId.ToString()
            };

            this.NetEngine.RequestCmd(SdkRequestType.MENTIONS_TIMELINE, cmdArg, (requestType, response) =>
                {
                    if (action != null)
                    {
                        if (response.errCode == SdkErrCode.SUCCESS)
                        {
                            WStatusCollection collection;
                            collection = JsonConvert.DeserializeObject<WStatusCollection>(response.content);

                            action(new Callback<WStatusCollection>(collection));
                        }
                        else
                        {
                            action(new Callback<WStatusCollection>(ErrCodeToMsg.GetMsg(response.errCode)));
                        }
                    }
                });
        }

        public void GetFavoritesTimeline(
            int count,
            long maxId,
            long sinceId,
            Action<Callback<WFavoriteCollection>> action)
        {
            SdkCmdBase cmdArg = new CmdStatusTimeline
            {
                acessToken = this.AccessToken,
                count = count.ToString(),
                max_id = maxId.ToString(),
                since_id = sinceId.ToString()
            };

            this.NetEngine.RequestCmd(SdkRequestType.FAVORITE_TIMLINE, cmdArg, (requestType, response) =>
                {
                    if (action != null)
                    {
                        if (response.errCode == SdkErrCode.SUCCESS)
                        {
                            WFavoriteCollection collection;
                            collection = JsonConvert.DeserializeObject<WFavoriteCollection>(response.content);

                            action(new Callback<WFavoriteCollection>(collection));
                        }
                        else
                        {
                            action(new Callback<WFavoriteCollection>(ErrCodeToMsg.GetMsg(response.errCode)));
                        }

                    }
                });
        }
    }
}
