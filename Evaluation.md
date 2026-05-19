## Purpose

This document is an evaluation for AI Content Assistant 2.0. It contains criteria for good output, two tests with expected success as well an rate-limit error test and a short reflection.

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



## Full AI Saved Reponses
Response 1:
{
  "id": 1,
  "prompt": "How do I properly follow the REST principles, specifically the plural endpoint names?",
  "generatedText": "The REST (Representational State of Resource) principle is a widely adopted architectural style for designing networked applications. One key aspect of REST is using plural endpoint names to identify resources.\n\nHere are some guidelines on how to properly follow the REST principle of using plural endpoint names:\n\n1. **Use plural nouns**: In REST, resource names should be plural nouns because it follows the natural way of referring to objects in a database or data storage system.\n2. **Avoid singular verbs**: Instead of using singular verbs (e.g., \"GET /users\") use plural forms (e.g., \"GET /users\").\n3. **Be consistent**: Use the same verb form for all actions on a resource. For example, if you have a list of users, you would use \"GET /users\" to retrieve the list and \"POST /users\" to create a new user.\n4. **Avoid generic nouns**: Generic nouns like \"thing\" or \"item\" are not recommended because they don't provide any information about what kind of resource it is.\n\nHere are some examples of proper plural endpoint names:\n\n* GET /users (retrieve a list of users)\n* POST /users (create a new user)\n* GET /users/{id} (retrieve a specific user by ID)\n* PUT /users/{id} (update an existing user)\n* DELETE /users/{id} (delete a specific user)\n\nIn contrast, here are some examples of incorrect singular endpoint names:\n\n* GET /user (retrieve a single user) -> should be GET /users\n* POST /user (create a new user) -> should be POST /users\n* GET /item (retrieve an item) -> should be GET /items\n\nBy following these guidelines, you can ensure that your RESTful API endpoint names are clear, consistent, and easy to understand.",
  "createdAt": "2026-05-19T12:34:04.6934293Z"
}

Response 2:
{
  "id": 2,
  "prompt": "Would following the Rest principles make my teacher happy?",
  "generatedText": "The R principles you're referring to are likely the \"SOLID\" principles, which stand for:\n\n1. S - Single Responsibility Principle (SRP)\n2. O - Open/Closed Principle (OCP)\n3. L - Liskov Substitution Principle (LSP)\n4. I - Interface Segregation Principle (ISP)\n5. D - Dependency Inversion Principle (DIP)\n\nFollowing the SOLID principles can make your teacher happy in several ways:\n\n1. **Easier maintenance**: By writing clean, modular, and maintainable code, you'll reduce the time and effort required to fix bugs or modify existing functionality.\n2. **Improved readability**: SOLID principles promote clear and concise code, making it easier for others (including your teacher) to understand your codebase.\n3. **Reduced technical debt**: By adhering to best practices and avoiding anti-patterns, you'll minimize the need for future refactoring and reduce the amount of technical debt accumulated in the project.\n\nHowever, it's essential to note that whether following SOLID principles will directly make your teacher happy depends on:\n\n1. Your teacher's specific expectations and requirements.\n2. The context of your project and its goals.\n3. The level of autonomy you have in choosing programming practices.\n\nIf your teacher has emphasized the importance of adhering to coding standards, readability, and maintainability, following SOLID principles can indeed make them happy. But if they're primarily concerned with getting results quickly or meeting tight deadlines, they might not be as impressed with strict adherence to SOLID principles.\n\nUltimately, it's essential to understand your teacher's expectations and adjust your approach accordingly.",
  "createdAt": "2026-05-19T12:35:41.4622154Z"
}
