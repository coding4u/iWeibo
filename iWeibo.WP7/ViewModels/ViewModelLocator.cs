using iWeibo.WP7.Adapters;
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

        public SinaTimelineViewModel SinaTimelineViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<SinaTimelineViewModel>();
            }
        }

        public PostNewViewModel PostNewViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<PostNewViewModel>();
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
