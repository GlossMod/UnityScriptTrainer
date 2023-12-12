# unity游戏 内置修改器

开的坑，一些unity游戏的内置修改器

下载地址： https://pan.aoe.top/ScriptTrainer

目前有：
 - [戴森球计划](https://mod.3dmgame.com/mod/173023)
 - [觅长生](https://mod.3dmgame.com/mod/176840)
 - [沙石镇时光](https://mod.3dmgame.com/mod/185597)
 - [暖雪](https://mod.3dmgame.com/mod/181716)
 - [疯狂游戏大亨2](https://mod.3dmgame.com/mod/188197)
 - [太吾绘卷](https://mod.3dmgame.com/mod/188315)
 - [犹格索托斯的庭院](https://mod.3dmgame.com/mod/203152)


### 编译

所需工具
- VS Code: https://code.visualstudio.com/
  - 并安装 VS Code 的 C# 相关插件 (可选)
- 最新的 .NET SDK: https://dotnet.microsoft.com/zh-cn/
- node: https://nodejs.org/ (可选)

生成:
- 将 `ScriptTrainer.csproj` 文件内的 `game_Location` 参数修改为你的游戏目录
- 控制台运行 `dotnet build ScriptTrainer` 进行打包
  - (如果有安装C#插件) 在左下角解决方案中右键"生成"
  - (如果有安装node) 在NPM 脚本中运行 `build` 