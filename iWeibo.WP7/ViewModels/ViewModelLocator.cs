﻿using iWeibo.WP7.Adapters;
using iWeibo.WP7.Services;
using iWeibo.WP7.ViewModels.SinaViewModels;
using iWeibo.WP7.ViewModels.TencentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iWeibo.WP7.ViewModels
{
    public class ViewModelLocator:IDisposable
    {
        private readonly ContainerLocator containerLocator;
        private bool disposed;

        public ViewModelLocator()
        {
            this.containerLocator = new ContainerLocator();
        }

        public MainPageViewModel MainPageViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<MainPageViewModel>();
            }
        }

        public TencentTimelineViewModel TencentTimelineViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<TencentTimelineViewModel>();
            }
        }

        public TencentStatusDetailViewModel TencentStatusDetailViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<TencentStatusDetailViewModel>();
            }
        }

        public TencentRepostPageViewModel TencentRepostPageViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<TencentRepostPageViewModel>();
            }
        }

        public SinaTimelineViewModel SinaTimelineViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<SinaTimelineViewModel>();
            }
        }

        public SinaStatusDetailViewModel SinaStatusDetailViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<SinaStatusDetailViewModel>();
            }
        }

        public SinaRepostPageViewModel SinaRepostPageViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<SinaRepostPageViewModel>();
            }
        }

        public PostNewViewModel PostNewViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<PostNewViewModel>();
            }
        }

        public PictureViewViewModel PictureViewViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<PictureViewViewModel>();
            }
        }

        public SettingsViewModel SettingsViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<SettingsViewModel>();
            }
        }

        public INavigationService NavigationService
        {
            get
            {
                return this.containerLocator.Container.Resolve<INavigationService>();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (disposing)
                this.containerLocator.Dispose();

            this.disposed = true;
        }
    }
}
