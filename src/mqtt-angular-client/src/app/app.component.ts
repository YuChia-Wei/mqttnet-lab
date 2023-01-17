//REF: https://github.com/emqx/MQTT-Client-Examples/blob/master/mqtt-client-Angular.js/src/app/app.component.ts

import { Component } from '@angular/core';
import {
  IMqttMessage,
  IMqttServiceOptions,
  MqttService,
  IPublishOptions,
} from 'ngx-mqtt';
import { IClientSubscribeOptions } from 'mqtt-browser';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
// import { MqttSubscribeService } from './mqtt-subscribe/mqtt_subscribe_service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'mqtt-angular-client';

  constructor(private _mqttService: MqttService) {
    this.client = this._mqttService;
  }

  ngOnInit(): void {
    this.createConnection();
  }

  private curSubscription: Subscription | undefined;

  connection = {
    // 如果要使用同網域下 (透過 Gateway 轉送) 的 mqtt broker 的話可以這樣設定
    // window.location.hostname 的參考來源為 https://stackoverflow.com/a/56058977
    // hostname: window.location.hostname,
    // 使用環境參數決定服務位置
    hostname: environment.mqtthost,
    port: 443,
    path: '/mqtt',
    clean: true, // Retain session
    connectTimeout: 4000, // Timeout period
    reconnectPeriod: 4000, // Reconnect period
    // Authentication information
    //  clientId: '',
    //  username: '',
    //  password: '',
    protocol: 'wss' //ws|wss
  };

  //接收設定，topic 可以考慮使用動態，依據每個使用者身分進行組合
  subscription = {
    topic: 'topic/mqttx',
    qos: 0,
  };

  //發送設定，topic 可以考慮使用動態，依據每個使用者身分進行組合
  publish = {
    topic: 'topic/mqttx',
    qos: 0,
    payload: '{ "msg": "Hello, I am browser., im new!!!!" }',
  };

  receiveNews = "i'm waiting to receive new message!";

  qosList = [
    { label: 0, value: 0 }, // 0：最多傳送一次（at most once）
    { label: 1, value: 1 }, // 1：至少傳送一次（at least once）
    { label: 2, value: 2 }, // 2：確實傳送一次（exactly once）
  ];

  client: MqttService | undefined;
  isConnection = false;
  subscribeSuccess = false;

  inputPayload: string = '';
  inputTopic: string = '';

  subscribeTopic: string = '';
  subscribedTopic: string = 'not yet subscribed!';

  // Create a connection
  createConnection() {
    // Connection string, which allows the protocol to specify the connection method to be used
    // ws Unencrypted WebSocket connection
    // wss Encrypted WebSocket connection
    // 備註：IMqttServiceOptions 目前看起來不支援以下兩種協議
    // mqtt Unencrypted TCP connection
    // mqtts Encrypted TCP connection
    try {
      this.client?.connect(this.connection as IMqttServiceOptions);
    } catch (error) {
      console.log('mqtt.connect error', error);
    }
    this.client?.onConnect.subscribe(() => {
      this.isConnection = true;
      console.log('Connection succeeded!');
    });
    this.client?.onError.subscribe((error: any) => {
      this.isConnection = false;
      console.log('Connection failed', error);
    });
    this.client?.onMessage.subscribe((packet: IMqttMessage) => {
      this.receiveNews = packet.payload.toString();
      console.log(
        `Received message: ${packet.payload.toString()} from topic ${packet.topic}`
      );
    });
  }

  doSubscribe(subscribeTopic: string) {
    console.log(`subscribe topic id: ${subscribeTopic}`);
    this.subscribedTopic = subscribeTopic;
    const { topic, qos } = this.subscription;
    this.curSubscription = this.client
      ?.observe(subscribeTopic, { qos } as IClientSubscribeOptions)
      .subscribe((message: IMqttMessage) => {
        this.subscribeSuccess = true;
        console.log('Subscribe to topics res', message.payload.toString());
      });
  }

  doUnSubscribe() {
    this.curSubscription?.unsubscribe();
    this.subscribeSuccess = false;
    this.subscribedTopic = 'not yet subscribed!';
    this.receiveNews = "i'm waiting to receive new message!";
  }

  doPublish(publishTopic: string, publishPayload: string) {
    const { topic, qos, payload } = this.publish;
    // console.log(this.publish);
    console.log(
      `publish topic id: ${publishTopic} and payload ${publishPayload}`
    );
    this.client?.unsafePublish(publishTopic, publishPayload, {
      qos,
    } as IPublishOptions);
  }

  destroyConnection() {
    try {
      this.client?.disconnect(true);
      this.isConnection = false;
      console.log('Successfully disconnected!');
    } catch (error: any) {
      console.log('Disconnect failed', error.toString());
    }
  }
}
