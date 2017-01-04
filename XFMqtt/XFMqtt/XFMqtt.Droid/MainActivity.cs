using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Prism.Unity;
using Microsoft.Practices.Unity;
using uPLibrary.Networking.M2Mqtt;
using System.Net;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text;
using Prism.Events;
using Newtonsoft.Json;

namespace XFMqtt.Droid
{
    [Activity(Label = "XFMqtt", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        string MQTT_BROKER_ADDRESS = "192.168.31.153";
        string strValue = "Hello";
        IEventAggregator fooEventAggregator;
        MqttClient client;
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.tabs;
            ToolbarResource = Resource.Layout.toolbar;

            base.OnCreate(bundle);

            MtqqInit();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            var fooApp = new App(new AndroidInitializer());

            // 注入 Prism 事件聚合器實作物件
            fooEventAggregator = fooApp.Container.Resolve<IEventAggregator>();

            // 核心 PCL 專案 -> 原生專案
            //訂閱 Prism 事件聚合器的 SendMessageEvent 型別的事件，
            // 當有這個事件接收到之後，就會使用 MQTT 用戶端，將 MQTT 訊息送出去
            fooEventAggregator.GetEvent<SendMessageEvent>().Subscribe(x =>
            {
                // 將傳送的 MQTT 訊息進行編碼
                var fooValue = JsonConvert.SerializeObject(x);
                client.Publish("/home/temperature", Encoding.UTF8.GetBytes(fooValue), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            });

            LoadApplication(fooApp);
        }

        /// <summary>
        /// 進行 MQTT 通訊協定的初始化
        /// </summary>
        private void MtqqInit()
        {
            // 產生 MQTT 用戶端物件，需要傳入 MQTT 代理人(Broker)主機的 IP位址 
            client = new MqttClient(MQTT_BROKER_ADDRESS);

            // 註冊 MQTT訊息接收與發佈要處理的委派事件 
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

            // 產生 MQTT 通訊協定本身要用到的唯一識別碼
            string clientId = Guid.NewGuid().ToString();
            // 啟動 MQTT 用戶端與遠端 MQTT 代理人進行連線動作
            client.Connect(clientId);

            // 訂閱有興趣的主題 Topic，與指定 QoS 的傳送品質定義
            client.Subscribe(new string[] { "/home/temperature" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        /// <summary>
        /// MQTT訊息接收與發佈要處理的委派事件 
        /// 當 MQTT 訊息有發布或接收的時候，會執行這個委派方法這個方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            // 將接收到的 MQTT 訊息進行解碼
            var fooValue = Encoding.UTF8.GetString(e.Message);
            var fooMessageType = JsonConvert.DeserializeObject<MessageType>(fooValue);
            // 原生專案 -> 核心 PCL 專案
            // 使用 Prism 事件聚合器，將 ReceiveMessageEvent 型別的非同步訊息送出
            // 在核心 PCL 專案內的訂閱事件委派方法收到之後，會更新其檢視模型
            fooEventAggregator.GetEvent<ReceiveMessageEvent>().Publish(fooMessageType);
        }

    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IUnityContainer container)
        {

        }
    }
}

