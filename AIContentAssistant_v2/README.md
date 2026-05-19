# AI Content Assistant

This repository contains two ASP.NET Core Web API projects that work together. Content API (Service A) is the public-facing API that receives prompts, stores generated content, and exposes CRUD endpoints. LLM Proxy API (Service B) is an internal LLM proxy that communicates with the chosen Large Language Model (such as Ollama or OpenAI) and returns generated text to Service A. Both services must run at the same time for the system to function.

## How to run both projects locally:

This project uses Ollama as the LLM provider, so you must have Ollama installed and running on your machine before starting the APIs. Install Ollama from https://ollama.com and make sure the Ollama service is running. You also need to pull the model you want to use, for example by running “ollama pull llama3” in your terminal. After that, open the solution in Visual Studio or VS Code. In Visual Studio, right‑click the solution, choose “Set Startup Projects…”, select “Multiple startup projects”, and set both ServiceA and ServiceB to “Start”. In VS Code, open two terminals and run “dotnet run” inside each project folder. Service A usually runs on https://localhost:7268 and Service B on https://localhost:7013. You can open Scalar for Service A at https://localhost:7268/scalar/v1. Make sure Ollama is running in the background, otherwise Service B will not be able to generate text.

## Setting up User Secrets:

Because this project uses Ollama, you do not need an external API key. Ollama runs locally and does not require authentication. However, Service A and Service B still use a shared secret to authenticate internal communication. Navigate to the ServiceA project folder and run “dotnet user-secrets init” followed by “dotnet user-secrets set "ServiceB:ApiKey" "YOUR_SHARED_SECRET"”. Then navigate to the Service B project folder and run “dotnet user-secrets init” followed by “dotnet user-secrets set "ServiceA:ApiKey" "YOUR_SHARED_SECRET"”. Both services must use the same value. You must also configure the base URL for Ollama inside Service B using User Secrets. Run “dotnet user-secrets set "Llm:BaseUrl" "http://localhost:11434/api/"”. This is the default API endpoint for Ollama running locally. 

## Custom Exception Middleware:

Service A includes a custom exception middleware that converts thrown exceptions into standardized ProblemDetails responses following RFC 7807. The middleware is placed at the top of the pipeline using UseCustomExceptionHandler. When an exception occurs anywhere in the request pipeline, the middleware captures the exception, maps it to an appropriate HTTP status code, and creates a ProblemDetails object containing status, title, detail, and the request path. This ensures consistent and predictable error responses across the entire API.

## How to trigger an error to see the middleware in action:

You can trigger the middleware by requesting a non‑existing resource, for example “GET /api/AiContent/99999”. You can also send an invalid request body or temporarily stop Service B and call the “/generate” endpoint. When an error occurs, the API returns a structured ProblemDetails response such as: { "status": 404, "title": "Not Found", "detail": "The requested content item does not exist.", "instance": "/api/AiContent/99999" }.

Technologies used include ASP.NET Core Web API, Scalar API documentation, custom middleware, User Secrets, an in‑memory repository for Service A, and an LLM provider for Service B.

This project is created for educational purposes.
