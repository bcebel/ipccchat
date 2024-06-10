using Azure;
using Microsoft.AspNetCore.Mvc;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Components.Forms;

namespace Jerdan_ShipAI.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        [HttpPost("Msg")]
        public IResult PostName([FromBody] string msg)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            string? oaiEndpoint = config["AzureOAIEndpoint"];
            string? oaiKey = config["AzureOAIKey"];
            string? oaiDeploymentName = config["AzureOAIDeploymentName"];

            if (string.IsNullOrEmpty(oaiEndpoint) || string.IsNullOrEmpty(oaiKey) || string.IsNullOrEmpty(oaiDeploymentName))
            {
                Console.WriteLine("Please check your appsettings.json file for missing or incorrect values.");
                return Results.NotFound();
            }

            OpenAIClient client = new OpenAIClient(new Uri(oaiEndpoint), new AzureKeyCredential(oaiKey));

            string systemMessage = "I am chatbot to help with Pitney Bowes shipping who knows about climate change. Focus on the data you were trained on from the Azure OpenAI model.";

            ChatCompletionsOptions chatCompletionsOptions = new ChatCompletionsOptions()
            {
                Messages =
                     {
                         new ChatRequestSystemMessage(systemMessage),
                         new ChatRequestUserMessage(msg),
                     },
                MaxTokens = 400,
                Temperature = 0.2f,
                DeploymentName = oaiDeploymentName
            };

            // Send request to Azure OpenAI model
            ChatCompletions response = client.GetChatCompletions(chatCompletionsOptions);

            // Print the response
            string completion = response.Choices[0].Message.Content;

            return Results.Ok(completion);
        }
    }
}
