
namespace TencentWeiboSDK.Model
{
    public class Education:BaseModel
    {
        private int departmentid = 0;
        private int id = 0;
        private int level = 0;
        private int schoolid=0;
        private int year = 0;

        public Education()
        {

        }

        public int DepartmentId
        {
            get
            {
                return departmentid;
            }
            set
            {
                if (value != departmentid)
                {
                    departmentid = value;
                    NotifyPropertyChanged("DepartmentId");
                }
            }
        }

        public int Id
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

        public int SchoolId
        {
            get
            {
                return schoolid;
            }
            set
            {
                if (value != schoolid)
                {
                    schoolid = value;
                    NotifyPropertyChanged("SchoolId");
                }
            }
        }

        public int Year
        {
            get
            {
                return year;
            }
            set
            {
                if (value != year)
                {
                    year = value;
                    NotifyPropertyChanged("Year");
                }
            }
        }
    }
}
