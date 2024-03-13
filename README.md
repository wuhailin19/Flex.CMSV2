# Flex.CMS

#### 介绍
开放自由编辑的CMS系统

#### 软件架构
软件架构说明


#### 安装教程

1.  xxxx
2.  xxxx
3.  xxxx

#### 使用说明
1.  去掉appsettings_初始.json的_初始字样，并修改DataConfig节点下的数据库连接字符串
2.  在Flex.EFSqlServer中的DesignTimeSqlServerContextFactory类修改数据库连接字符串，此处的用于codefirst生成使用
3.  在Flex.EFSqlServer目录调起终端窗口并执行以下命令即可生成数据库
4.  dotnet ef migrations add Initmdata --context SqlServerContext
5.  dotnet ef database update --context SqlServerContext
6.  启动Flex.Web项目

#### 参与贡献

1.  Fork 本仓库
2.  新建 Feat_xxx 分支
3.  提交代码
4.  新建 Pull Request


#### 特技

1.  使用 Readme\_XXX.md 来支持不同的语言，例如 Readme\_en.md, Readme\_zh.md
2.  Gitee 官方博客 [blog.gitee.com](https://blog.gitee.com)
3.  你可以 [https://gitee.com/explore](https://gitee.com/explore) 这个地址来了解 Gitee 上的优秀开源项目
4.  [GVP](https://gitee.com/gvp) 全称是 Gitee 最有价值开源项目，是综合评定出的优秀开源项目
5.  Gitee 官方提供的使用手册 [https://gitee.com/help](https://gitee.com/help)
6.  Gitee 封面人物是一档用来展示 Gitee 会员风采的栏目 [https://gitee.com/gitee-stars/](https://gitee.com/gitee-stars/)
