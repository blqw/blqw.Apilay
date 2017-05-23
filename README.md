简易通用的第三方api调用框架

[更完整的示例](http://www.jianshu.com/p/94d435086ae1)

### Demo  
一个Bing的翻译接口 *（现在已经不能用了）*  
接口地址是: `https://api.datamarket.azure.com/Bing/MicrosoftTranslator/v1/Translate`  
授权方式采用`Authorization Basic`  
参数2个分别为`Text`（表示要翻译的文本）和`To`（表示翻译后的语言代码），比较特殊的是这2个参数需要使用一对单引号包起来  
且返回的是一个xml  

#### 首先定义一个翻译接口

```cs
class TranslateV1 : ApRequest<string>
{
    [HeaderValue]
    public string Authorization { get; set; }
    [QueryValue]
    public string Text { get; }
    [QueryValue]
    public string To { get; }

    public TranslateV1(string text, string to = "zh-CHS")
    {
        Text = $"'{text}'";
        To = $"'{to}'";
    }

    public override string Path => "/Bing/MicrosoftTranslator/v1/Translate";

    public override string GetData(int statusCode, byte[] content, Func<string, string> getHeader)
    {
        if (statusCode != 200)
        {
            return "翻译失败";
        }
        const string START = "<d:String m:type=\"Edm.String\">";
        const string END = "</d:String>";
        var str = Encoding.UTF8.GetString(content);
        var start = str.IndexOf(START, StringComparison.Ordinal);
        if (start < 0)
        {
            return "翻译失败";
        }
        start += START.Length;
        var end = str.IndexOf(END, start, StringComparison.Ordinal);
        return str.Substring(start, end - start);
    }
}
```
#### 继承`ApSession`实现一个会话类
如果需要保持`cookie`或`access`token`，可以在会话类中保存一个属性

```cs
class Bing : ApSession
{
    public Bing()
        : base(new ApWebInvoker())
    {
        ImportConfig(x => ConfigurationManager.AppSettings[x]);
        Invoker.BaseUrl = new Uri(Url);
    }

    [ImportConfig("Bing.Url")]
    public Uri Url { get; set; }

    [ImportConfig("Bing.Authorization")]
    public string Authorization { get; set; }


    public Task<string> TranslateToCN(string text)
    {
        return SendAsync(Url, new TranslateV1(text)
        {
            Authorization = Authorization
        });
    }
}
```

#### 添加配置文件

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <add key="Bing.Url" value="https://api.datamarket.azure.com"/>
    <add key="Bing.Authorization" value="Basic NjBhYzBhNmQtMzkwMi00YT*****"/>
  </appSettings>
</configuration>
```

#### 调用

```cs
public class Program
{
    static void Main(string[] args)
    {
        Translate();
    }

    static readonly Bing _session = new Bing();
    private static async void Translate()
    {
        var text = await _session.TranslateToCN("hello");
        Console.WriteLine(text);
    }
}
```

#### 更新日志
- [0.0.4] 2017.05.23
当参数值为null, 不列入参数