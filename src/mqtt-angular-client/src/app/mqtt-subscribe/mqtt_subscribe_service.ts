// REF: https://www.emqx.com/en/blog/how-to-use-mqtt-in-angular
// REF: https://github.com/emqx/MQTT-Client-Examples/tree/master/mqtt-client-Angular.js/src/app
// import {
//   IMqttMessage,
//   IMqttServiceOptions,
//   MqttService,
//   IPublishOptions,
// } from 'ngx-mqtt';
// import { IClientSubscribeOptions } from 'mqtt-browser';
// import { Subscription } from 'rxjs';

// // @Component({
// //   selector: 'app-root',
// //   templateUrl: './app.component.html',
// //   styleUrls: ['./app.component.scss'],
// // })

// export class MqttSubscribeService {
//   constructor(private _mqttService: MqttService) {
//     this.client = this._mqttService;
//   }
//   private curSubscription: Subscription | undefined;
//   connection = {
//     hostname: 'localhost',
//     port: 1887,
//     path: '/',
//     clean: true, // Retain session
//     connectTimeout: 4000, // Timeout period
//     reconnectPeriod: 4000, // Reconnect period
//     // Authentication information
//     //  clientId: 'mqttx_597046f4',
//     //  username: 'emqx_test',
//     //  password: 'emqx_test',
//     protocol: 'mqtt',
//   };
//   subscription = {
//     topic: 'topic/mqttx',
//     qos: 0,
//   };
//   publish = {
//     topic: 'topic/browser',
//     qos: 0,
//     payload: '{ "msg": "Hello, I am browser." }',
//   };
//   receiveNews = '';
//   qosList = [
//     { label: 0, value: 0 },
//     { label: 1, value: 1 },
//     { label: 2, value: 2 },
//   ];
//   client: MqttService | undefined;
//   isConnection = false;
//   subscribeSuccess = false;

//   // Create a connection
//   createConnection() {
//     // Connection string, which allows the protocol to specify the connection method to be used
//     // ws Unencrypted WebSocket connection
//     // wss Encrypted WebSocket connection
//     // mqtt Unencrypted TCP connection
//     // mqtts Encrypted TCP connection
//     try {
//       this.client?.connect(this.connection as IMqttServiceOptions);
//     } catch (error) {
//       console.log('mqtt.connect error', error);
//     }
//     this.client?.onConnect.subscribe(() => {
//       this.isConnection = true;
//       console.log('Connection succeeded!');
//     });
//     this.client?.onError.subscribe((error: any) => {
//       this.isConnection = false;
//       console.log('Connection failed', error);
//     });
//     this.client?.onMessage.subscribe((packet: any) => {
//       this.receiveNews = this.receiveNews.concat(packet.payload.toString());
//       console.log(
//         `Received message ${packet.payload.toString()} from topic ${
//           packet.topic
//         }`
//       );
//     });
//   }

//   doSubscribe() {
//     const { topic, qos } = this.subscription;
//     this.curSubscription = this.client
//       ?.observe(topic, { qos } as IClientSubscribeOptions)
//       .subscribe((message: IMqttMessage) => {
//         this.subscribeSuccess = true;
//         console.log('Subscribe to topics res', message.payload.toString());
//       });
//   }

//   doUnSubscribe() {
//     this.curSubscription?.unsubscribe();
//     this.subscribeSuccess = false;
//   }

//   doPublish() {
//     const { topic, qos, payload } = this.publish;
//     console.log(this.publish);
//     this.client?.unsafePublish(topic, payload, { qos } as IPublishOptions);
//   }

//   destroyConnection() {
//     try {
//       this.client?.disconnect(true);
//       this.isConnection = false;
//       console.log('Successfully disconnected!');
//     } catch (error: any) {
//       console.log('Disconnect failed', error.toString());
//     }
//   }
// }
