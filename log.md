This document outlines the steps I've taken to come up with/develop my solution.

# Steps taken 
* create empty git repo, add README and .gitignore
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
