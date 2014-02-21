
using System.Collections.Generic;
namespace TencentWeiboSDK.Model
{
    public class Pictures
    {
        public List<PicInfo> info { get; set; }
    }

    public class PicInfo
    {
        public List<int> pic_XDPI{ get; set; }

        public List<int> pic_YDPI{ get; set; }

        public List<int> pic_height{ get; set; }

        public List<int> pic_size{ get; set; }

        public List<int> pic_type { get; set; }

        public List<int> pic_width { get; set; }

        public List<string> url{ get; set; }

    }
}
