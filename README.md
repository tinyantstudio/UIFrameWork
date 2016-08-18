# UIFrameWork
A tiny and powerful UIFrameWork for Unity4.x

Now Only for Chinese and simple English wiki to help someone need it.

*****
# 中文介绍
*****
基于NGUI编写的简单可扩展自动化UI框架


更详细的内容请查看文章：
地址 ： http://blog.csdn.net/fredomyan/article/details/46879203


一： 框架实现的主要内容如下：

1.加载，显示，隐藏，关闭页面，根据标示获得相应界面实例 

2.提供界面显示隐藏动画接口 

3.单独界面层级，Collider，背景管理 

4.根据存储的导航信息完成界面导航 

5.界面通用对话框管理(多类型Message Box) 

6.便于进行需求和功能扩展(比如，在跳出页面之前添加逻辑处理等) 


二：NGUI和Unity版本

Unity4.x和NGUI3.6.7

三：注意问题

1.工程中包含了NGUI3.6.7版本

2.该框架的重点不关注NGUI或者UGUI，关注的重点是窗口管理和框架的设计，在设计过程依赖第三方的UI插件控制层级，动画，布局等，所以相同的思路，UGUI同样可以支持，后续添加UGUI版本。

四：问题

1. 该框架已经被用到实际项目开发中

2. 如果发现问题，可以留言，谢谢


**注意：**

本工程使用的Unity4.x版本，使用Unity5.x打开，由于使用的NGUI版本较低，会出现脚本错误，解决方法如下

1. 更换较新版本NGUI，建议3.9.x以上(可以查看NGUI的官方文档，对应哪一个版本支持Unity5.x版本)

2. 更换完毕，可能出现项目中使用的NGUI3.6.7版本API或者功能不存在，直接删除或者按照导入的新版本NGUI修改即可


**Updates**

2016.1.29 

1.事件系统(add event system to project.)


****
# English simple wiki
****

## Important information
1. Unity4.x和NGUI3.6.7(If you want using Unity3d 5.x version, just update the NGUI's right version too)

## UIFrameWork Target:
1. load window, Show, hide, close window
2. Animation Interface for your window hide/show animation
3. Window Depth, window common collider bg manaager
4. (!!!maybe the most goal)Manager your window navigation (when you click the return/back button you need not to care which window I should Go)
5. Add some very common MessageBox
6. Give you a total game example(just the window logic)
7. You can easily modify for your own game
8. Do more (event system , Decoupled your game code)


## Updating or the "future":
1. MVC? 
2. Add a common Event system to this framework( Decoupled goal for communication between the window and your game logic)
3. optimizing the code (such as GC problem, the code design and son on)
4. thinking......
5. Oh UGUI? Just change the window depth manager I think.

## Attention

some attention


## Last
1. If you find some cool idea just share with using
2. Edit the Issue page !!! or email me if you want some feed back(Really need feedback and your idea)

**Wish this simple tiny frame work can help some one who want to save his time develop the window logic**
