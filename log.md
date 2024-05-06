This document outlines the steps I've taken to come up with/develop my solution.

# Design decisions  
* decided to use one command type for both watering and lighting commands 
  * unused fields will just be null 
  * could make a case for using separate forms to send less data over the wire
    * having a single model allows for easy and fast development for now. I would revisit this for produdction code 


* fetching the recipe list on every request is very inefficient. 
  * you'd probably want to cache this and then only update this every once in a while 

# Steps taken 
* create empty git repo, add README and .gitignore
  - added `appsettings.Development.json` to .gitignore
    - REMARK: 
      - Depending on the dev setup this file could contains sensitive information. I'd opt for not commiting it to source control. 
        - for production: defo use secret management
        - for dev: ideally use secret managment. at least keep settings local and NEVER commit sensitive configs  
* create template WebAPI project using Rider 
* added data models representing incoming recipe data
* added data models representing the outgoing schedule data
* added basic endpoint returning some fake schedule data
* raw output data currently looks like this
```
{
  "events": [
    {
      "dateTimeUtc": "2024-04-30T21:42:40.866538Z",
      "commandType": 0,
      "waterAmount": 3,
      "lightIntensity": null
    },
    {
      "dateTimeUtc": "2024-04-30T21:42:40.867053Z",
      "commandType": 1,
      "waterAmount": null,
      "lightIntensity": 3
    }
  ]
}
```
* add json input data harcoded to the file
* add models to parse the json object into
* create and hook up an API controller 
* move logic into the api controller
  - REMARK: 
    - using one controller per request route-base (e.g. `api/v1/schedule` is the common practice to separate matters and keep the code clean 
* rename api endpoint to `api/v1/schedule` 
  - REMARK: 
    - follow common API naming convetion
    - prefixing with `v1` allows for easy tracking and upgrade of API logic while staying backwards compatible

* add unit testings 
  - decided to use FluentAssertions because I find them quite intuitive
    - for production code you would use the guideline that's used by your company. `Assert.That` is quite a common one that's used in .NET. 


