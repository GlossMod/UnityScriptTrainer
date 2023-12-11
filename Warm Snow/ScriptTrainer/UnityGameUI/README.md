# UnityGameUI

使用 GameObject 构建的一个开源UI库

目前还有些不完善的第一方还请见谅

### 使用方法：
将本项目以子模块的形式添加到您的项目
然后 `using UnityGameUI`

### 例子：
```csharp
private void CreateUI()
{
    int width = Mathf.Min(Screen.width, 740);
    int height = (Screen.height < 400) ? Screen.height : (450);
    // 以父级画布中心点为0, 0 
    // 让按钮放在左上角则需要设置为 -width / 2, height / 2
    // 再根据按钮的宽度和高度进行偏移
    int elementX = -width / 2 + 120;
    int elementY = height / 2 - 60;

    GameObject canvas = UIControls.createUICanvas();
    GameObject uiPanel = UIControls.createUIPanel(canvas, height.ToString(), width.ToString(), null);   // 宽高以字符串的新式传入
    uiPanel.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF"); // 设置背景颜色

    // 创建按钮
    string backgroundColor = "#8C9EFFFF";
    Vector3 localPosition = new Vector3(elementX, elementY, 0);
    GameObject button = UIControls.createUIButton(uiPanel, backgroundColor, "按钮1", btn1OnClick, localPosition);
    // 可以根据自己的需求修改按钮样式
    button.AddComponent<Shadow>().effectColor = UIControls.HTMLString2Color("#000000FF");   // 添加阴影
    button.GetComponent<Shadow>().effectDistance = new Vector2(2, -2);              // 设置阴影偏移
    button.GetComponentInChildren<Text>().fontSize = 14;     // 设置字体大小           
    button.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 30);    // 设置按钮大小
}

private void btn1OnClick()
{
    Debug.Log("按钮1点击");
}
```

更多例子可以查看 https://github.com/GlossMod/UnityScriptTrainer