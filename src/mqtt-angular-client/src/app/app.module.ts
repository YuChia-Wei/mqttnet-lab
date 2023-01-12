import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { IMqttServiceOptions, MqttModule } from 'ngx-mqtt';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { environment } from 'src/environments/environment';

//REF: https://github.com/emqx/MQTT-Client-Examples/blob/master/mqtt-client-Angular.js/src/app/app.module.ts
//default connection option
export const connection: IMqttServiceOptions = {
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
  protocol: 'wss',
  connectOnCreate: false,
};

@NgModule({
  declarations: [AppComponent],
  imports: [BrowserModule, AppRoutingModule, MqttModule.forRoot(connection), FormsModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
