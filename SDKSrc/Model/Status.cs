using System.Collections;
using System.Collections.Generic;

namespace TencentWeiboSDK.Model
{
    /// <summary>
    /// 微博的 Model, 用于表示微博的对象.
    /// </summary>
    public class Status : BaseModel
    {
        private string city_code = string.Empty;
        private int count = 0;
        private string country_code = string.Empty;
        private string emotiontype = string.Empty;
        private string emotionurl = string.Empty;
        private string from = string.Empty;
        private string fromurl;
        private string geo = string.Empty;
        private string head = string.Empty;
        private string https_head = string.Empty;
        private string id = string.Empty;
        private List<string> image = null;
        private int isrealname = 0;
        private int isvip = 0;
        private string jing = string.Empty;
        private string latitude = string.Empty;
        private int likecount = 0;
        private string location = string.Empty;
        private string longitude = string.Empty;
        private int mcount = 0;
        //private Music music = null;
        private string name = string.Empty;
        private string nick = string.Empty;
        private string openid = string.Empty;
        private string origtext = string.Empty;
        private Pictures pic = null;
        private string province_code = string.Empty;
        private int readcount = 0;
        private int self = 0;
        private Status source = null;
        private int status = 0;
        private string text = string.Empty;
        private long timestamp = 0;
        private int type = 0;
        //private Video video=null
        private string wei = string.Empty;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Status()
        { }

        /// <summary>
        /// 城市代码
        /// </summary>
        public string City_Code
        {
            get
            {
                return city_code;
            }
            set
            {
                if (value != city_code)
                {
                    city_code = value;
                    NotifyPropertyChanged("City_Code");
                }
            }
        }

        /// <summary>
        /// 微博被转次数
        /// </summary>
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                if (value != count)
                {
                    count = value;
                    NotifyPropertyChanged("Count");
                }
            }
        }

        /// <summary>
        /// 国家代码
        /// </summary>
        public string Country_Code
        {
            get
            {
                return country_code;
            }
            set
            {
                if (value != country_code)
                {
                    country_code = value;
                    NotifyPropertyChanged("Country_Code");
                }
            }
        }

        /// <summary>
        /// 心情类型
        /// </summary>
        public string EmotionType
        {
            get
            {
                return emotiontype;
            }
            set
            {
                if (value != emotiontype)
                {
                    emotiontype = value;
                    NotifyPropertyChanged("EmotionType");
                }
            }
        }

        /// <summary>
        /// 心情图片url
        /// </summary>
        public string EmotionUrl
        {
            get
            {
                return emotionurl;
            }
            set
            {
                if (value != emotionurl)
                {
                    emotionurl = value;
                    NotifyPropertyChanged("EmotionUrl");
                }
            }
        }
        /// <summary>
        /// 来源
        /// </summary>
        public string From
        {
            get
            {
                return from;
            }
            set
            {
                if (value != from)
                {
                    from = value;
                    NotifyPropertyChanged("From");
                }
            }
        }

        /// <summary>
        /// 来源url
        /// </summary>
        public string FromUrl
        {
            get
            {
                return fromurl;
            }
            set
            {
                if (value != fromurl)
                {
                    fromurl = value;
                    NotifyPropertyChanged("FromUrl");
                }
            }
        }

        /// <summary>
        /// 发表者地理信息
        /// </summary>
        public string Geo
        {
            get
            {
                return geo;
            }
            set
            {
                if (value != geo)
                {
                    geo = value;
                    NotifyPropertyChanged("Geo");
                }
            }
        }

        /// <summary>
        /// 发表者头像url
        /// </summary>
        public string Head
        {
            get
            {
                return head+@"/50";
            }
            set
            {
                if (value != head)
                {
                    head = value;
                    NotifyPropertyChanged("Head");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Https_Head
        {
            get
            {
                return https_head;
            }
            set
            {
                if (value != https_head)
                {
                    https_head = value;
                    NotifyPropertyChanged("Https_Head");
                }
            }
        }

        /// <summary>
        /// 微博唯一id
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
        /// 图片url列表
        /// </summary>
        public List<string> Image
        {
            get
            {
                return image;
            }
            set
            {
                if (value != image)
                {
                    image = value;
                    NotifyPropertyChanged("Image");
                }
            }
        }

        /// <summary>
        /// 是否实名认证，0-老用户，1-已实名认证，2-未实名认证,
        /// </summary>
        public int IsRealName
        {
            get
            {
                return isrealname;
            }
            set
            {
                if (value != isrealname)
                {
                    isrealname = value;
                    NotifyPropertyChanged("IsRealName");
                }
            }
        }

        /// <summary>
        /// 是否微博认证用户，0-不是，1-是
        /// </summary>
        public int IsVIP
        {
            get
            {
                return isvip;
            }
            set
            {
                if (value != isvip)
                {
                    isvip = value;
                    NotifyPropertyChanged("IsVIP");
                }
            }
        }
        
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                if (value != latitude)
                {
                    latitude = value;
                    NotifyPropertyChanged("Latitude");
                }
            }
        }

        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                if (value != longitude)
                {
                    longitude = value;
                    NotifyPropertyChanged("Longitude");
                }
            }
        }

        /// <summary>
        /// 发表者所在地
        /// </summary>
        public string Location
        {
            get
            {
                return location;
            }
            set
            {
                if (value != location)
                {
                    location = value;
                    NotifyPropertyChanged("Location");
                }
            }
        }

        /// <summary>
        /// 点评次数
        /// </summary>
        public int MCount
        {
            get
            {
                return mcount;
            }
            set
            {
                if (value != mcount)
                {
                    mcount = value;
                    NotifyPropertyChanged("MCount");
                }
            }
        }

        /// <summary>
        /// 发表人帐户名
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

        /// <summary>
        /// 发表人昵称
        /// </summary>
        public string Nick
        {
            get
            {
                return nick;
            }
            set
            {
                if (value != nick)
                {
                    nick = value;
                    NotifyPropertyChanged("Nick");
                }
            }
        }

        /// <summary>
        /// 用户唯一id，与name相对应
        /// </summary>
        public string OpenId
        {
            get
            {
                return openid;
            }
            set
            {
                if (value != openid)
                {
                    openid = value;
                    NotifyPropertyChanged("OpenId");
                }
            }
        }

        /// <summary>
        /// 原始内容
        /// </summary>
        public string OrigText
        {
            get
            {
                return origtext;
            }
            set
            {
                if (value != origtext)
                {
                    origtext = value;
                    NotifyPropertyChanged("OrigText");
                }
            }
        }

        /// <summary>
        /// 图片信息列表
        /// </summary>
        public Pictures Pic
        {
            get
            {
                return pic;
            }
            set
            {
                if (value != pic)
                {
                    pic = value;
                    NotifyPropertyChanged("Pic");
                }
            }
        }

        /// <summary>
        /// 省份代码
        /// </summary>
        public string Province_Code
        {
            get
            {
                return province_code;
            }
            set
            {
                if (value != province_code)
                {
                    province_code = value;
                    NotifyPropertyChanged("Province_Code");
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public int ReadCount
        {
            get
            {
                return readcount;
            }
            set
            {
                if (value != readcount)
                {
                    readcount = value;
                    NotifyPropertyChanged("ReadCount");
                }
            }
        }

        /// <summary>
        /// 是否自已发的的微博
        /// </summary>
        public int Self
        {
            get
            {
                return self;
            }
            set
            {
                if (value != self)
                {
                    self = value;
                    NotifyPropertyChanged("Self");
                }
            }
        }

        /// <summary>
        /// 当type=2时，source即为源tweet
        /// </summary>
        public Status Source
        {
            get
            {
                return source;
            }
            set
            {
                if (value != source)
                {
                    source = value;
                    NotifyPropertyChanged("Source");
                }
            }
        }

        /// <summary>
        /// 微博状态，0-正常，1-系统删除，2-审核中，3-用户删除，4-根删除
        /// </summary>
        public int State
        {
            get
            {
                return status;
            }
            set
            {
                if (value != status)
                {
                    status = value;
                    NotifyPropertyChanged("State");
                }
            }
        }

        /// <summary>
        /// 微博内容
        /// </summary>      
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (value != text)
                {
                    text = value;
                    NotifyPropertyChanged("Text");
                }
            }
        }

        /// <summary>
        /// 发表时间
        /// </summary>
        public long TimeStamp
        {
            get
            {
                return timestamp;
            }
            set
            {
                if (value != timestamp)
                {
                    timestamp = value;
                    NotifyPropertyChanged("TimeStamp");
                }
            }
        }

        /// <summary>
        /// 微博类型，1-原创发表，2-转载，3-私信，4-回复，5-空回，6-提及，7-评论
        /// </summary>
        public int Type
        {
            get
            {
                return type;
            }
            set
            {
                if (value != type)
                {
                    type = value;
                    NotifyPropertyChanged("Type");
                }
            }
        }


        private bool isFavorite=false;
        /// <summary>
        /// （自定义属性）是否已添加到收藏
        /// </summary>
        public bool IsFavorite
        {
            get
            {
                return isFavorite;
            }
            set
            {
                if (value != isFavorite)
                {
                    isFavorite = value;
                    NotifyPropertyChanged("IsFavorite");
                }
            }
        }

        public bool HasPic
        {
            get
            {
                return Image == null ? false : true;
            }
        }

        public string ImageUrl
        {
            get
            {
                return Image == null ? "" : Image[0];
            }
        }

        public bool IsRetweetedStatus
        {
            get
            {
                return Source == null ? false : true;
            }
        }
    }
}
