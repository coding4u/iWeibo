iWeibo
======
iWeibo is a microblogging aggregation type applications, integration Sina, Tencent two platforms information, support one-click publishing to multiple platforms, currently supports homepage, mentioned collection, forwarding and reviews, view pictures and other functions.

This Application is based on Windows Phone 7.1.It use Prism library for Windows Phone to implement MVVM pattern.
The following outlines the main projects that make up this application:

iWeibo.WP7                This project contains the views and view models for this application,
                          along with supporting classes and resources.
              
iWeibo.WP7.Adapters       This project contains interfaces,adapters,and facades for Windows Phone
                          API functionality.
                          
iWeibo.WP7.Services       This projects contains interfaces and implementations to persisting application setting
                          and data to and from Isolated Storage.

TencentWeibo.SDK          This project is Tencent Weibo's official SDK for Windwows Phone On CodePlex.
                          Based on this, I have added some new features and fixes some bugs found during test
                          
WeiboSDK                  This project is Sina Weibo's fofficial SDK for Windows Phone On GitHub .
                          I also added some features that are not had, added some models, 
                          and fixes a number of bugs found during debugging.

This is my first formal Windows Phone App,It has been published in the Windows Phone Store:
http://www.windowsphone.com/zh-cn/store/app/爱微博/f75417a7-32c4-4f2d-baa6-78bbabe99056
No doubt,there are still some need to improve,you can reach me at coding4u@outlook.com.
