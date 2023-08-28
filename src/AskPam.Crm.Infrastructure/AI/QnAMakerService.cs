using AskPam.Crm.AI;
using AskPam.Crm.AI.Entities;
using AskPam.Exceptions;
using AskPam.Crm.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AskPam.Crm.Configuration;
using AskPam.Crm.Common;

namespace AskPam.Crm.AI
{
    public class QnAMakerService : IQnAMakerService
    {
        private ISettingManager _settingsManager;
        private string qnASubscriptionKey;

        public QnAMakerService(ISettingManager settingsManager)
        {
            _settingsManager = settingsManager;
            qnASubscriptionKey = _settingsManager.GetSettingValueForApplicationAsync(AppSettingsNames.AI.QnABotSubscriptionKey).Result;
        }

        public async Task<Guid> CreateKnowledgeBase(string name, IEnumerable<QnAPair> qnAPairs)
        {
            var client = new RestClient("https://westus.api.cognitive.microsoft.com/qnamaker/v2.0");

            var request = new RestRequest($"knowledgebases/create", Method.POST);

            // easily add HTTP Headers
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Ocp-Apim-Subscription-Key", qnASubscriptionKey);

            if (qnAPairs == null)
            {
                qnAPairs = new List<QnAPair>();
            }

            request.AddJsonBody(new
            {
                Name = name,
                QnAPairs = qnAPairs.Select(qna => new
                {
                    Question = qna.Question,
                    Answer = qna.Answer
                })
            });

            var taskCompletion = new TaskCompletionSource<IRestResponse<QnAResult>>();

            var handle = client.ExecuteAsync<QnAResult>(request, r => taskCompletion.SetResult(r));

            IRestResponse<QnAResult> response = await taskCompletion.Task;

            JObject dyn = JObject.Parse(response.Content);

            //Get kbId
            return Guid.Parse(dyn["kbId"].ToString());
        }

        public async Task Publish(Guid knowledgeBaseId)
        {
            var client = new RestClient("https://westus.api.cognitive.microsoft.com/qnamaker/v2.0");

            var request = new RestRequest($"knowledgebases/{knowledgeBaseId}", Method.PUT);

            // easily add HTTP Headers
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Ocp-Apim-Subscription-Key", qnASubscriptionKey);

            var taskCompletion = new TaskCompletionSource<IRestResponse>();

            var handle = client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            IRestResponse response = await taskCompletion.Task;

            if (response.ErrorException != null)
                throw new ApiException(response.ErrorException);
        }

        public async Task UpdateKnowledgeBase(
            Guid knowledgeBaseId,
            IEnumerable<QnAPair> newQnAPairs,
            IEnumerable<OldQnAPair> editQnAs,
            IEnumerable<QnAPair> delQnAs
        )
        {
            var client = new RestClient("https://westus.api.cognitive.microsoft.com/qnamaker/v2.0/");

            var request = new RestRequest($"knowledgebases/{knowledgeBaseId}", Method.PATCH);

            // easily add HTTP Headers
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Ocp-Apim-Subscription-Key", qnASubscriptionKey);

            var qnAsToAdd = new List<dynamic>();
            if (newQnAPairs != null)
            {
                qnAsToAdd.AddRange(newQnAPairs
                    .Select(qna => new
                    {
                        Question = qna.Question,
                        Answer = qna.Answer
                    }
                    )
                );
            }
            if (editQnAs != null)
            {
                qnAsToAdd.AddRange(
                    editQnAs.Select(qna => new
                    {
                        Question = qna.NewQuestion,
                        Answer = qna.NewAnswer
                    }
                    )
                );
            }

            var qnAsToDelete = new List<dynamic>();
            if (editQnAs != null)
            {
                qnAsToDelete.AddRange(
                    editQnAs.Select(qna => new
                    {
                        Question = qna.Old.Question,
                        Answer = qna.Old.Answer
                    }
                    )
                );
            }
            if (delQnAs != null)
            {
                qnAsToDelete.AddRange(
                    delQnAs.Select(qna => new
                    {
                        Question = qna.Question,
                        Answer = qna.Answer
                    }
                    )
                );
            }

            request.AddJsonBody(new
            {
                Add = new
                {
                    QnaPairs = qnAsToAdd
                },
                Delete = new
                {
                    QnaPairs = qnAsToDelete
                }
            });

            var taskCompletion = new TaskCompletionSource<IRestResponse<QnAResult>>();

            var handle = client.ExecuteAsync<QnAResult>(request, r => taskCompletion.SetResult(r));

            IRestResponse<QnAResult> response = await taskCompletion.Task;
        }


        public async Task<IEnumerable<QnAResult>> Ask(string question, Guid knowledgeBaseId)
        {
            var client = new RestClient("https://westus.api.cognitive.microsoft.com/qnamaker/v2.0");

            var request = new RestRequest($"knowledgebases/{knowledgeBaseId}/generateAnswer", Method.POST);

            // easily add HTTP Headers
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Ocp-Apim-Subscription-Key", qnASubscriptionKey);

            request.AddJsonBody(new AskRequest()
            {
                question = question
            }
            );

            var taskCompletion = new TaskCompletionSource<IRestResponse>();

            var handle = client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            var response = await taskCompletion.Task;
            JObject dyn = JObject.Parse(response.Content);

            if (!Util.IsSuccessStatusCode(response.StatusCode))
            {
                throw new ApiException($"{dyn["error"]["code"]} : {dyn["error"]["message"]}", response.StatusCode);
            }
            
            var askResponse = JsonConvert.DeserializeObject<AskResponse>(response.Content);

            return ConvertTo(askResponse.answers);

            IEnumerable<QnAResult> ConvertTo(Answer[] answers)
            {
                if (answers != null)
                {
                    return answers.Select(a => new QnAResult()
                    {
                        Answer = a.answer,
                        Questions = a.questions,
                        Score = a.score
                    }
                    );
                }
                return null;
            }
        }
    }
}
