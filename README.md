# Router.NET

![stability-wip](https://img.shields.io/badge/stability-work_in_progress-lightgrey.svg)

## What is it

This is an AWS Lambda .NET Core function API which can be used for:
- routing your clients towards different servers
- informing clients whether they need to update or that there is maintenance going on the particular server

## Tutorial

If you are absolutely new to lambdas I recommend following this tutorial at first 
https://docs.aws.amazon.com/serverless-application-model/latest/developerguide/serverless-getting-started-hello-world.html

## Setup & deploy

To use the SAM CLI for AWS Lambda with .NET Core, you need the following tools:

* [SAM CLI](https://docs.aws.amazon.com/serverless-application-model/latest/developerguide/serverless-sam-cli-install.html)
* [.NET Core](https://www.microsoft.com/net/download)
* [Docker](https://hub.docker.com/search/?type=edition&offering=community)

To build and deploy your application for the first time, run the following:

```bash
sam build --template serverless.template
sam deploy --guided
```

The first command will build the source of your application.
It installs dependencies, creates a deployment package, and saves it in the `.aws-sam/build` folder.
The second command will package and deploy your application to AWS, with some prompts.

Later you can use just:
```bash
sam deploy
```

## Local Run

Build your application with the `sam build` command.

```bash
sam build --template serverless.template
```

Run it with:

```bash
sam local start-api
```

### DynamoDB

Simply run 

```bash
docker run -p 8000:8000 -d --rm amazon/dynamodb-local
```

to execute DynamoDB locally

see arguments explanation here:
https://docs.docker.com/engine/reference/commandline/run/

To connect to DynamoDB locally using GUI use [NoSQL Workbench](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/workbench.html)

### MongoDB

There is also implementation for MongoDB as a storage.