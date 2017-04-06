#  Azure Notification Hub Template (測試傳送)

## 使用於 iOS 的推播格式之 C# 語法

### 自行要夾帶的其他內容定義方式

string templateBodyAPNS = "{\"aps\":{\"alert\":\"$(messageParam)\", \"sound\" : \"default\", \"args\":\"$(argsParam)\"}}";

### 最後的 Json 內容

{"data":{"message":"$(messageParam)", "args":"$(argsParam)"}}

### Azure Notification Hub 內的預設的推播內容格式

string templateBodyAPNS = "{\"aps\":{\"alert\":\"$(messageParam)\"}}";

### 最後的 Json 內容

{"aps":{"alert":"$(messageParam)"}}



## Azure

### 預設格式

 {"aps":{"alert":"Notification Hub test notification"}}

### 增加額外 Payload 的格式

{"aps":{"alert":"你的應用程式有兩個新紀錄產生", "sound" : "default", "args":"eyJOYXZpZ2F0aW9uUGFnZSI6IkRldGFpbFBhZ2UiLCJPdGhlckluZm9ybWF0aW9uIjoi6YCZ6KOh5Y+v5Lul5pS+5YWl5YW25LuW6aGN5aSW6LOH6KiKIiwiQ29udGVudFRpdGxlIjpudWxsLCJDb250ZW50VGV4dCI6bnVsbCwiU3VtbWFyeVRleHQiOm51bGwsIkluYm94U3R5bGVMaXN0IjpbXSwiU3R5bGUiOjAsIlZpc2liaWxpdHkiOjAsIlByaW9yaXR5IjowLCJDYXRlZ29yeSI6MCwiTGFyZ2VJY29uIjpmYWxzZSwiU291bmQiOmZhbHNlLCJWaWJyYXRlIjpmYWxzZX0="}}

In iOS, if the app is in the foreground, a different method is called than when in the app is in the background. ReceivedRemoteNotification is used when the app is backgrounded, DidReceiveRemoteNotification is called when the app is in the foreground.


