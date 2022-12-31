import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import {IMqttServiceOptions, MqttModule, MqttService} from 'ngx-mqtt';

export const connection: IMqttServiceOptions = {
  hostname: 'localhost',
  port: 64430,
  path: '/mqtt',
  clean: true, // Retain session
  connectTimeout: 4000, // Timeout period
  reconnectPeriod: 4000, // Reconnect period
  // Authentication information
  // clientId: 'mqttx_597046f4',
  // username: 'emqx_test',
  // password: 'emqx_test',
  // protocol: 'ws',
  protocol: 'wss',
}
@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    MqttModule.forRoot(connection)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
