using System.Collections.Generic;

namespace TencentWeiboSDK.Model
{
    /// <summary>
    /// 用户 Model，用来表示微博用户的对象.
    /// </summary>
    public class User : BaseModel
    {
        private string birth_day = string.Empty;
        private string brith_month = string.Empty;
        private string birth_year = string.Empty;
        private string city_code = string.Empty;
        private List<Company> comp = null;
        private string country_code = string.Empty;
        private List<Education> edu = null;
        private string email = string.Empty;
        private int exp = 0;
        private int fansnum = 0;
        private int favnum = 0;
        private string head = string.Empty;
        private string homecity_code = string.Empty;
        private string homecountry_code = string.Empty;
        private string homepage = string.Empty;
        private string homeprovince_code = string.Empty;
        private string hometown_code = string.Empty;
        private string https_head = string.Empty;
        private int idolnum = 0;
        private string industry_code = string.Empty;
        private string introduction = string.Empty;
        private int isent = 0;
        private int ismyblack = 0;
        private int ismyfans = 0;
        private int ismyidol = 0;
        private int isrealname = 0;
        private int isvip = 0;
        private int level = 0;
        private string location = string.Empty;
        private int mutual_fans_num = 0;
        private string name = string.Empty;
        private string nick = string.Empty;
        private string openid = string.Empty;
        private string province_code = string.Empty;
        private string regtime = string.Empty;
        private int send_private_flag = 0;
        private int sex = 0;
        private List<Tag> tag = null;
        private List<Status> tweetinfo = null;
        private int tweetnum = 0;
        private string verifyinfo = string.Empty;

        /// <summary>
        /// 构造函数
        /// </summary>
        public User()
        { }

        /// <summary>
        /// 出生天
        /// </summary>
        public string Birth_Day
        {
            get
            {
                return birth_day;
            }
            set
            {
                if (value != birth_day)
                {
                    birth_day = value;
                    NotifyPropertyChanged("Birth_Day");
                }
            }
        }

        /// <summary>
        /// 出生月
        /// </summary>
        public string Birth_Month
        {
            get
            {
                return brith_month;
            }
            set
            {
                if (value != brith_month)
                {
                    brith_month = value;
                    NotifyPropertyChanged("Birth_Month");
                }
            }
        }

        /// <summary>
        /// 出生年
        /// </summary>
        public string Birth_Year
        {
            get
            {
                return birth_year;
            }
            set
            {
                if (value != birth_year)
                {
                    birth_year = value;
                    NotifyPropertyChanged("Birth_Year");
                }
            }
        }

        /// <summary>
        /// 出生年月日
        /// </summary>
        public string Birth_Date
        {
            get
            {
                return string.Format("{0}-{1}-{2}", birth_year, brith_month, birth_day);
            }
            //set
            //{
            //    string birth_data = string.Format("{0}-{1}-{2}", birth_year, brith_month, birth_day);
            //    if (value != birth_data)
            //    {
            //        string[] items = birth_data.Split('-');
            //        birth_year = items[0];
            //        brith_month = items[1];
            //        birth_day = items[2];
            //        NotifyPropertyChanged("Birth_Data");
            //    }
            //}
        }

        /// <summary>
        /// 城市Id
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
                    NotifyPropertyChanged("CityCode");
                }
            }
        }

        /// <summary>
        /// 公司信息列表
        /// </summary>
        public List<Company> Comp
        {
            get
            {
                return comp;
            }
            set
            {
                if (value != comp)
                {
                    comp = value;
                    NotifyPropertyChanged("Comp");
                }
            }
        }

        /// <summary>
        /// 国家Id
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
        /// 教育信息列表
        /// </summary>
        public List<Education> Edu
        {
            get
            {
                return edu;
            }
            set
            {
                if (value != edu)
                {
                    edu = value;
                    NotifyPropertyChanged("Edu");
                }
            }
        }

        /// <summary>
        /// Email
        /// </summary>
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                if (value != email)
                {
                    email = value;
                    NotifyPropertyChanged("Email");
                }
            }
        }

        /// <summary>
        /// 经验值
        /// </summary>
        public int Exp
        {
            get
            {
                return exp;
            }
            set
            {
                if (value != exp)
                {
                    exp = value;
                    NotifyPropertyChanged("Exp");
                }
            }
        }
                
        /// <summary>
        /// 听众数
        /// </summary>
        public int FansNum
        {
            get
            {
                return fansnum;
            }
            set
            {
                if (value != fansnum)
                {
                    fansnum = value;
                    NotifyPropertyChanged("FansNum");
                }
            }
        }

        /// <summary>
        /// 收藏数
        /// </summary>
        public int FavNum
        {
            get
            {
                return favnum;
            }
            set
            {
                if (value != favnum)
                {
                    favnum = value;
                    NotifyPropertyChanged("FavNum");
                }
            }
        }

        /// <summary>
        /// 头像Url
        /// </summary>
        public string Head
        {
            get
            {
                return head;
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
        /// 家乡所在城市Id
        /// </summary>
        public string HomeCity_Code
        {
            get
            {
                return homecity_code;
            }
            set
            {
                if (value != homecity_code)
                {
                    homecity_code = value;
                    NotifyPropertyChanged("HomeCity_Code");
                }
            }
        }

        /// <summary>
        /// 家乡所在国家Id
        /// </summary>
        public string HomeCountry_Code
        {
            get
            {
                return homecountry_code;
            }
            set
            {
                if (value != homecountry_code)
                {
                    homecountry_code = value;
                    NotifyPropertyChanged("HomeCountry_code");
                }
            }
        }

        /// <summary>
        /// 个人主页
        /// </summary>
        public string HomePage
        {
            get
            {
                return homepage;
            }
            set
            {
                if (value != homepage)
                {
                    homepage = value;
                    NotifyPropertyChanged("HomePage");
                }
            }
        }

        /// <summary>
        /// 家乡所在省Id
        /// </summary>
        public string HomeProvince_Code
        {
            get
            {
                return homeprovince_code;
            }
            set
            {
                if (value != homeprovince_code)
                {
                    homeprovince_code = value;
                    NotifyPropertyChanged("HomeProvince_Code");
                }
            }
        }

        /// <summary>
        /// 家乡所在城镇Id
        /// </summary>
        public string HomeTown_Code
        {
            get
            {
                return hometown_code;
            }
            set
            {
                if (value != hometown_code)
                {
                    hometown_code = value;
                    NotifyPropertyChanged("HomeTown_Code");
                }
            }
        }

        /// <summary>
        /// 收听的人数
        /// </summary>
        public int IdolNum
        {
            get
            {
                return idolnum;
            }
            set
            {
                if (value != idolnum)
                {
                    idolnum = value;
                    NotifyPropertyChanged("IdolNum");
                }
            }
        }

        /// <summary>
        /// 行业Id
        /// </summary>
        public string Industry_Code
        {
            get
            {
                return industry_code;
            }
            set
            {
                if (value != industry_code)
                {
                    industry_code = value;
                    NotifyPropertyChanged("Industry_Code");
                }
            }
        }

        /// <summary>
        /// 个人介绍
        /// </summary>
        public string Introduction
        {
            get
            {
                return introduction;
            }
            set
            {
                if (value != introduction)
                {
                    introduction = value;
                    NotifyPropertyChanged("Introduction");
                }
            }
        }

        /// <summary>
        /// 是否企业机构
        /// </summary>
        public int IsEnt
        {
            get
            {
                return isent;
            }
            set
            {
                if (value != isent)
                {
                    isent = value;
                    NotifyPropertyChanged("IsEnt");
                }
            }
        }

        /// <summary>
        /// 是否在我的黑名单中
        /// </summary>
        public int IsMyBlack
        {
            get
            {
                return ismyblack;
            }
            set
            {
                if (value != ismyblack)
                {
                    ismyblack = value;
                    NotifyPropertyChanged("IsMyBlack");
                }
            }
        }

        /// <summary>
        /// 是否收听我
        /// </summary>
        public int IsMyFans
        {
            get
            {
                return ismyfans;
            }
            set
            {
                if (value != ismyfans)
                {
                    ismyfans = value;
                    NotifyPropertyChanged("IsMyFans");
                }
            }
        }

        /// <summary>
        /// 是否是我收听的人
        /// </summary>
        public int IsMyIdol
        {
            get
            {
                return ismyidol;
            }
            set
            {
                if (value != ismyidol)
                {
                    ismyidol = value;
                    NotifyPropertyChanged("IsMyIdol");
                }
            }
        }

        /// <summary>
        /// 是否实名认证
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
        /// 是否是认证用户
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
        /// 等级
        /// </summary>
        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                if (value != level)
                {
                    level = value;
                    NotifyPropertyChanged("Level");
                }
            }
        }

        /// <summary>
        /// 所在地.
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
        /// 互听好友数
        /// </summary>
        public int Mutual_Fans_Num
        {
            get
            {
                return mutual_fans_num;
            }
            set
            {
                if (value != mutual_fans_num)
                {
                    mutual_fans_num = value;
                    NotifyPropertyChanged("Mutual_Fans_Num");
                }
            }
        }

        /// <summary>
        /// 帐户名.
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
        /// 昵称.
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
        /// 唯一id，与name相对应.
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
        /// 地区Id
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
        /// 注册时间 
        /// </summary>
        public string RegTime
        {
            get
            {
                return regtime;
            }
            set
            {
                if (value != regtime)
                {
                    regtime = value;
                    NotifyPropertyChanged("RegTime");
                }
            }
        }

        /// <summary>
        /// 是否允许所有人给当前用户发私信
        /// </summary>
        /// <remarks>0-仅有偶像，1-名人+听众，2-所有人</remarks>
        public int Send_Private_Flag
        {
            get
            {
                return send_private_flag;
            }
            set
            {
                if (value != send_private_flag)
                {
                    send_private_flag = value;
                    NotifyPropertyChanged("Send_Private_Flag");
                }
            }
        }

        /// <summary>
        /// 性别
        /// </summary>
        /// <remarks>1-男，2-女，0-未填写</remarks>
        public int Sex
        {
            get
            {
                return sex;
            }
            set
            {
                if (value != sex)
                {
                    sex = value;
                    NotifyPropertyChanged("Sex");
                }
            }
        }

        /// <summary>
        /// 标签列表
        /// </summary>
        public List<Tag> Tag
        {
            get
            {
                return tag;
            }
            set
            {
                if (value != tag)
                {
                    tag = value;
                    NotifyPropertyChanged("Tag");
                }
            }
        }

        /// <summary>
        /// 最近的一条原创微博信息
        /// </summary>
        public List<Status> TweetInfo
        {
            get
            {
                return tweetinfo;
            }
            set
            {
                if (value != tweetinfo)
                {
                    tweetinfo = value;
                    NotifyPropertyChanged("TweetInfo");
                }
            }
        }

        /// <summary>
        /// 发表的微博数量
        /// </summary>
        public int TweetNum
        {
            get
            {
                return tweetnum;
            }
            set
            {
                if (value != tweetnum)
                {
                    tweetnum = value;
                    NotifyPropertyChanged("Tweetnum");
                }
            }
        }

        /// <summary>
        /// 认证信息
        /// </summary>
        public string VerifyInfo
        {
            get
            {
                return verifyinfo;
            }
            set
            {
                if (value != verifyinfo)
                {
                    verifyinfo = value;
                    NotifyPropertyChanged("VerifyInfo");
                }
            }
        }


    }
}
