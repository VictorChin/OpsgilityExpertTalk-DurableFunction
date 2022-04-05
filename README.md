# client Functions exposes 3 Http Endpoint
- ClientFunctionHttp: [GET] http://localhost:7071/api/activities/{activity}
  - activity: [ "TimerSample", "FanOut" ]  
- EntityHttp: [GET] http://localhost:7071/api/entity/{account}/{action}/{amount?}
  - account : Name of account, serve as EntityID
  - action : [  InitializeLoan,AddTransaction,GetBalance,ListTransaction  ]
- GetLoanBalance: [GET] http://localhost:7071/api/LoanBalance/{account}
  - use *Non-Durable* Azure Function to access stateful Entity
  



# After Cloning the repo, please add local.settings.json to BasicActivity Project with following content:
```
{
    "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "[StorageConnectionString]",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "containerPath": "https://mssa0404.blob.core.windows.net/images-vc",
    "TwilioAccountSid": "[twilioAccountSid]",
    "AuthToken": "[twilioAuthToken]"
  }
}
```
