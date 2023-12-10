namespace testingEnvironmentApp.Services.MqttService.Interfaces
{
    public interface IMqttMessageHelper
    {
        public  string GenerateTemplateJson(List<string> topics);
        public  string UpdateTopicValue(string json, string topicName, string newValue);
    }
}
