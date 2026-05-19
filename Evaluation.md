## Purpose

This document records an evaluation for AI Content Assistant 2.0. It contains criteria for good output, two tests with expected success as well an rate-limit error test and a short reflection.

## Criteria for good output

Good output is relevant to the user prompt, clear and grammatically correct, complete enough to cover the requested points, factually correct or explicitly marked as assumption, safe (no API keys or internal errors exposed), and structured when the backend/UI needs to parse it. Good output should also be checked for hallucinations and consistency: the model must not invent facts, and repeated calls with the same prompt should produce stable answers in form and content.

## AI Content Generation Tests: Expected Success

Prompt 1: "How do I properly follow the REST principles, specifically the plural endpoint names?" 
Saved response (shortened): "Use plural nouns for resources, for example GET /users and POST /users. Be consistent and use /users/{id} for single resources; avoid generic names like /item or /thing." 

Prompt 2: "Would following the Rest principles make my teacher happy?" 
Saved response (shortened): "The R principles you're referring to are likely the \"SOLID\" principles, which stand for:\n\n1. S - Single Responsibility Principle (SRP)\n2. O - Open/Closed Principle (OCP)\n3."

## Error Test: Expected 429 (Rate Limit)

Request body: { "prompt": "hello" } Response body: { "title": "Too Many Requests", "status": 429, "detail": "You have exceeded the allowed number of requests. Please try again later." } The inbound limiter returns RFC7807 ProblemDetails as intended.

## Reflection

The first test returned the expected, relevant answer. The second test did not perform well because the model sometimes misinterprets intent when prompts lack context. The model seems to be sensitive to prompt wording (garbage in => garbage out) and can therefore drift off topic. All of the tests also show the intended ProblemDetails response following the RFC7807 standard. 
