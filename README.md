iWeibo for Windows Phone 7.x
======

iWeibo is a microblogging aggregation type applications, integration Sina, Tencent two platforms information, support one-click publishing to multiple platforms, currently supports homepage, mentioned collection, forwarding and reviews, view pictures and other functions.

This Application use Prism library for Windows Phone to implement MVVM pattern.
The following outlines the main projects that make up this application:

* **iWeibo.WP7** - contains the views and view models for this application,
                          along with supporting classes and resources.
              
* **iWeibo.WP7.Adapters** -contains interfaces,adapters,and facades for Windows Phone
                          API functionality.
                          
* **iWeibo.WP7.Services**  -contains interfaces and implementations to persisting application setting
                          and data to and from Isolated Storage.

* **TencentWeibo.SDK**  - Tencent Weibo's official SDK for Windwows Phone On CodePlex.
                          Based on this, I have added some new features and fixes some bugs found during test
                          
* **WeiboSDK**  - Sina Weibo's fofficial SDK for Windows Phone On GitHub .
                          I also added some features that are not had, added some models, 
                          and fixes a number of bugs found during debugging.


