# Router.NET

![stability-stable](https://img.shields.io/badge/stability-stable-green.svg)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## What is it

First, this is a sample .NET Core AWS Lambda application with DynamoDB and MongoDB storage implementations.

Secondly, this is an API for giving you client apps the server URL they have to work with.

It can be handy for example in the following scenario:

Say you have an iOS app, and you have to release a new version: 0.18.0. 
This new version has to be released to the App Store via the review process. You build this app, and it targets the Production server.
For the time of the review process, you want it to use some non-production server called Review.
So you instruct Router to redirect iOS clients of version 0.18.0 to the Review server.
When the review process is done and restores Router to target 0.18.0 to production.

There are many more cases like enabling server maintenance or requesting clients to update via this tool.

See example [Postman collection](./Local.postman_collection.json)
To initialize it call APIs with default presets:
* Set Route Target Configuration
* Set Routing Configuration
* then use Get Server Address to emulate client app calls 

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

## Storages

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

Any Mongo hosting is fine but for ease of use you can set up the [MongoDB Atlas](https://www.mongodb.com/) Free Tier.
