﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iWeibo.WP7.Adapters
{
    public interface IPhotoChooserTask
    {
        SettablePhotoResult TaskEventArgs
        {
            get;
            set;
        }

        event EventHandler<SettablePhotoResult> Completed;

        void Show();
    }
}
