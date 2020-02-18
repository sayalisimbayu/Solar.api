using solar.github.errorlog.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Octokit;
using System;

namespace solar.github
{
    public static class GitHub
    {
        public static void createIssue(Exception ex, dynamic input, IHeaderDictionary httpHeaders)
        {
            //SimbIssue simbIssue = new SimbIssue
            //{
            //    Headers = httpHeaders,
            //    input = input,
            //    exception = ex
            //};
            //var tokenAuth = new Credentials("sayalisimbayu", "Sw@miS@maratha11"); // NOTE: not real token
            //var client = new GitHubClient(new ProductHeaderValue("sayalisimbayu")); // More on GitHubClient can be found in "Getting Started"
            //client.Credentials = tokenAuth;
            //var repositories = client.Repository.GetAllForCurrent().Result;
            //long repoId = 0;
            //for (int i = 0; i < repositories.Count; i++)
            //{
            //    if (repositories[i].Name == "POS-API")
            //    {
            //        repoId = repositories[i].Id;
            //    }
            //}
            //var createIssue = new NewIssue(String.Format("Automated Issue Logger: {0}", ex.Message));
            //createIssue.Body = JsonConvert.SerializeObject(simbIssue, Formatting.Indented);
            //if (repoId > 0)
            //{
            //    var issue = client.Issue.Create(repoId, createIssue).Result;
            //    //issue.HtmlUrl
            //}
        }

        public static void createIssue(Exception ex, object p, object headers)
        {
            throw new NotImplementedException();
        }
    }
}
