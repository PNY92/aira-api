# Aira API
An open-source API server dedicated for handling researching tasks as Research Assistant

## Features
* **Chat Function:** Generate response from AI Models provided from OpenRouter

## How to run API server
* Start by configuring `config.json` to your preferred settings and API key
* After starting `AiraAPI.exe`, you will be able to visit API sites using `http://127.0.0.1:5000/api/{action}`

## List of API sites
* `http://127.0.0.1:5000/api/generate`

## Model Library
| Model Name | Calling Convention |
| ---------- | ------------------ |
| Deepseek V3 | deepseek/deepseek-chat:free |

## Generate a prompt
```cmd
curl --location "http://localhost:5000/api/generate" -H "Content-Type: application/json" -H "Accept: text/event-stream" -d "{\"role\": \"user\",\"model\": \"deepseek/deepseek-chat:free\",\"content\": \"What is the purpose of life\"}"
```
