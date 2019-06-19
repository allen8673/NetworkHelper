# NetworkHelp 

## 1.Introduction
NetworkHelp以C#整合`dotNet HttpClient`，以及套件`Newtonsoft.Json`，讓使用者得以方便地使用Http `Post`,`Get`,`Put`,`Delete`等方法與Api連線，並且將其回應的Json轉成指定型別的物件。

## 2.Purpose
雖然`dotNet HttpClient`可以透過Http `Post`,`Get`,`Put`,`Delete`方法連接Web api，但是使用上卻相對複雜，其常見標準程式範例如下:

```csharp
using(HttpClient client = new HttpClient())
{
    try	
    {
        HttpResponseMessage response = await client.GetAsync("http://www.contoso.com/");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        List<Model> model = JsonConvert.DeserializeObject<List<Model>>(responseBody);
      }  
      catch(HttpRequestException e)
      {
         Console.WriteLine("Exception Caught!");	
         Console.WriteLine("Message :{0} ",e.Message);
      }
}
```

為了簡化上述程式，讓使用者可以更方便的處理Api的連接，以及將`Json Object`轉換為指定型態的物件，因此提出`NetworkHelp`，整合`dotNet HttpClient`以及套件`Newtonsoft.Json`，並將其簡化為簡單的方法供使用者使用，以簡潔程式，避免過多冗長的程式重複出現而造成系統難以維護。

## 3.Environment & Installation
適用語言: C#
適用框架: .Net Framework 4.6.1 以上(含)
建議開發工具: Visual Studio 2015 以上(含)
源碼位置: https://github.com/allen8673/NetworkHelper.git
安裝說明:
- 1 打開`NetworkHelper.Module.sln`進行編譯
![Step 1](Doc/Install_1.png )
![Step 2](Doc/Install_2.png )

- 2 將`NetworkHelper`編譯完成內容(通常在該專案資料夾中`bin`資料夾底下)加入至目標方案中
- 3 開啟目標方案，將`NetworkHelper.dll`加入至指定專案即可
![Step 3](Doc/Install_3.png )

## 4.Methods

`NetworkHelper`主要以`Connecter`物件作為功能介面。其中提供方法如下表:

|方法|說明|參數說明|
|:-|:-|:-|
|Get|以Http Get連接Api，並取得資訊|baseUri: Host url網址<br>api: Api相對網址(不含Host url)<br> failureAct: 指定錯誤發生時所要執行的動作<br> |
|Post|以Http Post連接Api，並傳送內容以取得資訊|baseUri: Host url網址<br>api: Api相對網址(不含Host url)<br> mediaType: 傳送資訊型態(xaml, jason等....)<br> requestHeader:傳送時Header的內容 <br/> failureAct: 指定錯誤發生時所要執行的動作<br>|
|PostJson|以Http Post連接Api，並以Json傳送內容以取得資訊|baseUri: Host url網址<br>api: Api相對網址(不含Host url)<br>request: 任意傳送內容<br> failureAct: 指定錯誤發生時所要執行的動作<br>|
|Put|以Http Put連接Api，並傳送內容以取得內容|api: Api相對網址(不含Host url)<br>request: 任意傳送內容<br> failureAct: 指定錯誤發生時所要執行的動作<br>|
|Upload|以Http Post上傳檔案|api: Api相對網址(不含Host url)<br> files:上傳檔案資訊<br> data: 傳送內容<br>failureAct: 指定錯誤發生時所要執行的動作<br>|
|Download| 下載檔案| downloadSetting: 下載檔案設定|

## 5.Sample
+ Connecter設定Base url
```csharp
Connecter.SetBaseUri(new Uri("your host url"));
```

+ Connecter.Get
範例1: 傳入`Api Url`參數至`Get`方法，取得Json object，並將其轉為指定型態物件
```csharp
Model model = await Connecter.Get<Model>("Sample/GetDemo");
```
範例2: 進一步將失敗時所需要執行的動作傳入至`Get`方法
```csharp
Model model = await Connecter.Get<Model>("Sample/GetDemo", (resp)=>
{
    Console.WriteLine(resp.StatusCode);
    return new model;
});
```

+ Connecter.Post
範例1: 傳入`Api Url`參數至`Post`方法，取得Json object，並將其轉為指定型態物件
```csharp
Model model = await Connecter.Post<Model>("Sample/PostDemo",
                new MediaTypeWithQualityHeaderValue("application/json"), 
                new RequestHeader { token = "your token" },
                new StringContent("json object contnt"));
```
範例2: 進一步將失敗時所需要執行的動作傳入至`Post`方法
```csharp
Model model = await Connecter.Post<Model>("Sample/PostDemo",
                new MediaTypeWithQualityHeaderValue("application/json"), 
                new RequestHeader { token = "your token" },
                new StringContent("json object contnt"),
                (resp)=> 
                {
                    Console.WriteLine(resp.StatusCode);
                    return new Model();
                });
```

+ Connecter.PostJson
範例1: 傳入`Api Url`參數至`PostJson`方法，取得Json object，並將其轉為指定型態物件
```csharp
Model model = await Connecter.PostJson<Model>("Sample/GetModel", new
{
    Property_1 = 1,
    Property_2 = "demo"
});
```

+ Connecter.Put
+ Connecter.Upload
+ Connecter.Download

