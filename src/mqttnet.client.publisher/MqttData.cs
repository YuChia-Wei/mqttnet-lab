namespace mqttnet.client.publisher
{
    public record MqttData
    {
        public string Data { get; set; }
        public string TopicId { get; set; }
    }
}