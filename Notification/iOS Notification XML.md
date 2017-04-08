#  Azure Notification Hub Template (測試傳送)

## 使用於 iOS 的推播格式之 C# 語法

### 自行要夾帶的其他內容定義方式

string templateBodyAPNS = "{\"aps\":{\"alert\":\"$(messageParam)\", \"sound\" : \"default\", \"args\":\"$(argsParam)\"}}";

### 最後的 Json 內容

{"data":{"message":"$(messageParam)", "args":"$(argsParam)"}}

### Azure Notification Hub 內的預設的推播內容格式

string templateBodyAPNS = "{\"aps\":{\"alert\":\"$(messageParam)\"}}";
string templateBodyAPNS = "{\"aps\":{\"alert\":\"$(messageParam)\", \"content-available\" : 1}}";

### 最後的 Json 內容

{"aps":{"alert":"$(messageParam)"}}
{"aps":{"alert":"$(messageParam)", "content-available":1}}


## Azure

### 預設格式

 {"aps":{"alert":"Notification Hub test notification"}}

### 增加額外 Payload 的格式

{"aps":{"alert":"你的應用程式有兩個新紀錄產生", "sound" : "default", "args":"eyJOYXZpZ2F0aW9uUGFnZSI6IkRldGFpbFBhZ2UiLCJPdGhlckluZm9ybWF0aW9uIjoi6YCZ6KOh5Y+v5Lul5pS+5YWl5YW25LuW6aGN5aSW6LOH6KiKIiwiQ29udGVudFRpdGxlIjpudWxsLCJDb250ZW50VGV4dCI6bnVsbCwiU3VtbWFyeVRleHQiOm51bGwsIkluYm94U3R5bGVMaXN0IjpbXSwiU3R5bGUiOjAsIlZpc2liaWxpdHkiOjAsIlByaW9yaXR5IjowLCJDYXRlZ29yeSI6MCwiTGFyZ2VJY29uIjpmYWxzZSwiU291bmQiOmZhbHNlLCJWaWJyYXRlIjpmYWxzZX0="}}

{"aps":{"alert":"Something new has happened in your app!", "content-available" : 1}}


'aps' {
  'content-available': 1,
  'alert': 'Something new has happened in your app!''
}

In iOS, if the app is in the foreground, a different method is called than when in the app is in the background. 

ReceivedRemoteNotification is used when the app is backgrounded, DidReceiveRemoteNotification is called when the app is in the foreground.

`It seems that there is no way to handle the notification if the app is terminated.`

[Apple Background Execution](https://developer.apple.com/library/content/documentation/iPhone/Conceptual/iPhoneOSProgrammingGuide/BackgroundExecution/BackgroundExecution.html#//apple_ref/doc/uid/TP40007072-CH4-SW57)

`Understanding When Your App Gets Launched into the Background`

Apps that support background execution may be relaunched by the system to handle incoming events. 

If an app is terminated for any reason other than the user force quitting it, the system launches the app when one of the following events happens:


...
For background download apps:
...

A push notification arrives for an app and the payload of the notification contains the content-available key with a value > of 1.

I ended up with the following concept:

Each app installation gets a unique tag when the user registers to the remote push notification.

Our server which send the remote push notifications by unique tag to the users app now stores the notifications into the database (temporary).

When app is in foreground or background:

Push notification are handled by app itself. Saving the notification into the local app database with the method "DidReceivedNotification".

When app is terminated (closed manually by user):

The notifications are shown as a banner (handled by iOS itself)

When the user start the app I contact our server to get the missed notifications by a timestamp of the last stored notification in the app (I only get the missed notifications)

After getting the push notifications the server deletes them.



